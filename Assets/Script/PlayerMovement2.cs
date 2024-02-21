    using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerMovement2 : MonoBehaviour
{

    private PlayerInput defaultPlayerActions;
    [SerializeField] public UnityEngine.CharacterController controller;
    [SerializeField] public Animator characterAnimator;

    public float walkSpeed = 5f;
    [SerializeField] public float initialWalkSpeed = 5f;
    [SerializeField] public float walkSpeedCrouching = 2.5f;
    [SerializeField] public float runSpeed = 10f;

    Vector3 velocity;
    public float gravity = -9.81f;

    public float jumpForce = 2f;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public bool isAiming = false;


    public int primarySecondary = 0;

    public Camera mainCamera;
    public Camera snipCamera;
    public Camera cameraUI;
    public GameObject scopedCamera;
    public GameObject normalParent;
    public GameObject snipScopParent;
    private float transitionStartTimeAiming;
    private float transitionStartTimeCrouching;
    private float initialCrouchPositon;
    public float crouchPosition;

    public int numberWeapons;
    public int currentNumberWeapon;
    [SerializeField] public GameObject[] ListWeapons;

    public Inventory inventory;
    public GameObject scopeOverlay;
    private float timeScopeSniper = 0.25f;
    public GameObject CameraweaponsCamera;
    private float scopedFOV = 10f;
    private float normalFOV;
    public GameObject weaponListGameObject;
    // public float normalMouseSensitivity;

    public int bulletsInside;
    public int bulletsTotal;

    public float healthPlayer;

    private float timeBetweenShots;
    private float timeForRecoil;
    private float lastShotTime = 0f;
    private float lastReloadTime = 0f;
    private float timeBetweenReload;

    public WeaponController weaponController;
    public TextMeshProUGUI gameOver;

    public ChargerUI chargerUI;
    public SlowMotion slowMotion;

    public int money;

    private Coroutine scopedSniperCoroutine; // Variable pour stocker la coroutine
    private bool isRecovering;
    
    public Canvas canvaBlood;
    public GameObject gameManager;
    public bool reloadingEnCours = false;
    private bool weaponChanged = false;
    [SerializeField] private AudioClip hitmarkerSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] public PauseManager pauseManager;
    private PauseManager pauseScript;
    public bool isRunning;

    private void Awake(){
        defaultPlayerActions = GetComponent<PlayerInput>();
        // defaultPlayerActions = new ActionZombieBriCazV2();
        // defaultPlayerActions.Enable();
    }
    void Start()
    {
        gameManager = GameObject.Find("GameManager"); 
        for(int i=0; i<Gamepad.all.Count; i++){
            Debug.Log(Gamepad.all[i].name);
        }
        pauseScript = pauseManager.GetComponent<PauseManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialCrouchPositon = controller.height;
        inventory = gameObject.GetComponent<Inventory>();
        // normalMouseSensitivity = weaponListGameObject.GetComponent<MouseLook>().mouseSensitivity;
        slowMotion = gameObject.GetComponent<SlowMotion>();
        gameOver.enabled = false;
    }
    
    void Update()
    {   
        // mainCamera = Camera.main;
        // Debug.Log(mainCamera);
        weaponController = inventory.GetItem(primarySecondary).GetComponent<WeaponController>();
        bulletsTotal = weaponController.bulletsTotal;
        bulletsInside = weaponController.bulletsInside;
        timeBetweenShots = weaponController.timeBetweenShots;
        timeForRecoil = weaponController.timeForRecoil;
        timeBetweenReload = weaponController.timeBetweenReload;
        // float x = Input.GetAxis("Horizontal");
        // float y = Input.GetAxis("Vertical");
        // Vector2 movementInput = defaultPlayerActions.Player.Move.ReadValue<Vector2>();
        // Vector2 movementInput = defaultPlayerActions.actions["Move"].ReadValue<Vector2>();

        if(!pauseManager.isGamePaused && !pauseManager.boolShowSkins){
            Vector2 movementInput = UserInput.instance.MoveInput;

            float x = movementInput.x;
            float y = movementInput.y;
            bool isMoving = x != 0 || y != 0;
            bool isMovingForward = y > 0.8f;

            // bool isRunning = Input.GetButton("Run");
            // bool isRunning = defaultPlayerActions.actions["Run"].ReadValue<float>() == 1.0f;
            isRunning = UserInput.instance.RunningInput;

            // bool isChangingWeapon = Input.GetButtonDown("ChangeWeapon");
            bool isChangingWeapon = false;
            if (UserInput.instance.ChangingWeaponInput) //defaultPlayerActions.actions["ChangeWeapon"].triggered
            {
                isChangingWeapon = true;
            }

            // bool isShooting = Input.GetButton("Fire");
            // bool isShooting = defaultPlayerActions.actions["Fire"].ReadValue<float>() == 1.0f;
            bool isShooting = UserInput.instance.FireReleasedInput;
            
            // bool isShootingCoup = Input.GetButtonDown("Fire");
            bool isShootingCoup = false;
            if (UserInput.instance.FireInput) //defaultPlayerActions.actions["Fire"].triggered
            {
                isShootingCoup = true;
            }
            

            // bool isReloading = Input.GetButtonDown("Reload");
            bool isReloading = false;
            if (UserInput.instance.ReloadInput) //defaultPlayerActions.actions["Reload"].triggered
            {
                isReloading = true;
            }

            bool isInspecting = false;
            if (UserInput.instance.InspectInput) //defaultPlayerActions.actions["Reload"].triggered
            {
                isInspecting = true;
            }
            
            // float w = Input.GetAxis("MouseScrollWheel");
            // float w = defaultPlayerActions.actions["MouseScrollWheel"].ReadValue<float>();
            float w = UserInput.instance.MouseScrollWheelInput;
            if (w > 0f && primarySecondary != 0)
            {
                buttonPrimaryToSecondary2();
            }
            else if (w < 0f && primarySecondary != 1)
            {
                buttonPrimaryToSecondary1();
            }

            if (isChangingWeapon)
            {
                buttonPrimaryToSecondary();
            }

            // InputChooseWeapon();

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (UserInput.instance.JumpInput && isGrounded) //defaultPlayerActions.actions["Jump"].triggered
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
            // if (Input.GetButtonDown("Jump") && isGrounded)
            // {
            //     velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            // }

            // bool isCrouching = Input.GetButton("Crouching");
            // bool isCrouching = defaultPlayerActions.actions["Crouching"].ReadValue<float>() == 1.0f;
            // bool isCrouching = UserInput.instance.CrouchingInput;
            // callCrouch(isCrouching);

            // bool AimingButton = defaultPlayerActions.actions["Aiming"].ReadValue<float>() == 1.0f;
            bool AimingButton = UserInput.instance.AimingInput;
            if (AimingButton && !isAiming)
            {
                Aiming();
            }
            else if (!AimingButton && isAiming)
            {
                notAiming();
            }
            // if (Input.GetButtonDown("Aiming") && !isAiming)
            // {
            //     Aiming();
                
            // }
            // else if (Input.GetButtonUp("Aiming") && isAiming)
            // {
            //     notAiming();
            // }
            if (isMoving)
            {
                if (isRunning && isMovingForward && !reloadingEnCours)
                {
                    if(isAiming){
                        characterAnimator.SetBool("isRunning", false);
                        characterAnimator.SetBool("isWalking", true);
                        Vector3 moveAimRun = transform.right * x + transform.forward * y;
                        controller.Move(moveAimRun * walkSpeed * Time.deltaTime);
                    }
                    else{
                        characterAnimator.SetBool("isRunning", true);
                        characterAnimator.SetBool("isWalking", true);
                        Vector3 move = transform.right * x + transform.forward * y;
                        controller.Move(move * runSpeed * Time.deltaTime);
                    }
                    
                }
                else
                {
                    characterAnimator.SetBool("isRunning", false);
                    characterAnimator.SetBool("isWalking", true);
                    Vector3 move = transform.right * x + transform.forward * y;
                    controller.Move(move * walkSpeed * Time.deltaTime);
                }

            }
            else
            {
                characterAnimator.SetBool("isWalking", false);
                characterAnimator.SetBool("isRunning", false);
            }

            if(isShooting && bulletsInside != 0 && (inventory.GetItem(primarySecondary) == ListWeapons[2] || inventory.GetItem(primarySecondary) == ListWeapons[3] || inventory.GetItem(primarySecondary) == ListWeapons[4]))
            {
                Shoot();
            }
            else if (isShootingCoup && bulletsInside != 0 && (inventory.GetItem(primarySecondary) == ListWeapons[0] || inventory.GetItem(primarySecondary) == ListWeapons[1] || inventory.GetItem(primarySecondary) == ListWeapons[5]))
            {
                Shoot();
            }
            else if (isShooting && bulletsInside == 0)
            {
                if(bulletsTotal  > 0 && !reloadingEnCours )
                {
                    reloadingEnCours = true;
                    Reload();
                }
                
            }
            if (isReloading && !isRunning && bulletsInside < inventory.GetItem(primarySecondary).GetComponent<WeaponController>().maxBulletsInOneMagazine)
            {
                if (bulletsTotal > 0)
                {
                    // reloadingEnCours = true;
                    Reload();
                } 
            }
            if(gameObject.transform.position.y <= -6.2017){
                playerDie();
            }
            if(isInspecting && !isRunning ){
                InspectWeapon();
            }
        }
        

        
    }

    private void InspectWeapon()
    {
        characterAnimator.Play("Inspect");

    }

    private void Reload() 
    {
        if(Time.time - lastReloadTime >= timeBetweenReload){
            characterAnimator.Play("Reload");
            SoundManager.Instance.PlaySound(inventory.GetItem(primarySecondary).GetComponent<WeaponController>().reloadClip);
            lastReloadTime = Time.time;
        }
        
    }

    private void Shoot()
    {
        

        if (Time.time - lastShotTime >= timeBetweenShots)
        {
            characterAnimator.Play("Shoot");

            if (inventory.GetItem(primarySecondary) == ListWeapons[1])
            {
                    inventory.GetItem(primarySecondary).GetComponent<WeaponController>().FireShotgun();
            }
            else if(inventory.GetItem(primarySecondary) == ListWeapons[5]){
                if(isAiming){
                    inventory.GetItem(primarySecondary).GetComponent<WeaponController>().FireAiming();
                }
                else{
                    inventory.GetItem(primarySecondary).GetComponent<WeaponController>().Fire();
                }
            }
            else if (isAiming)
            {
                if (Time.time - lastShotTime >= timeForRecoil)
                {
                    inventory.GetItem(primarySecondary).GetComponent<WeaponController>().FireAiming();

                }
                else
                {
                    inventory.GetItem(primarySecondary).GetComponent<WeaponController>().FireAimingWithRecoil();
                }
            }
            else
            {
                inventory.GetItem(primarySecondary).GetComponent<WeaponController>().Fire();
            }
            lastShotTime = Time.time;
            bulletsInside = bulletsInside - 1;
            inventory.GetItem(primarySecondary).GetComponent<WeaponController>().bulletsInside = bulletsInside;
        }
    }

    private void notAiming()
    {

        isAiming = false;
        characterAnimator.SetBool("isAiming", false);
        // weaponListGameObject.GetComponent<MouseLook>().mouseSensitivity = normalMouseSensitivity;
        if (inventory.GetItem(primarySecondary) == ListWeapons[5])
        {
            onUnscopedSniper();
        }
        cameraUI.enabled = true;
        //walkSpeed = initialWalkSpeed;
    }

    private void Aiming()
    {
        isAiming = true;
        characterAnimator.SetBool("isAiming", true);
        walkSpeed = inventory.GetItem(primarySecondary).GetComponent<WeaponController>().walkSpeedAiming;
        // weaponListGameObject.GetComponent<MouseLook>().mouseSensitivity = weaponListGameObject.GetComponent<MouseLook>().mouseSensitivity / 2;
        if (inventory.GetItem(primarySecondary) == ListWeapons[5])
        {
            if (scopedSniperCoroutine != null)
            {
                StopCoroutine(scopedSniperCoroutine);
            }
            scopedSniperCoroutine = StartCoroutine(onScopedSniper());
        }
        cameraUI.enabled = false;
        // cameraUISnip.enabled = false;
    }

    void onUnscopedSniper()
    {
        scopeOverlay.SetActive(false);
        CameraweaponsCamera.SetActive(true);
        // weaponListGameObject.GetComponent<MouseLook>().mouseSensitivity = normalMouseSensitivity;
        // mainCamera.GetComponent<Camera>().fieldOfView = normalFOV;
        // normalParent.SetActive(true);
        snipScopParent.SetActive(false);
        // normalParent.SetActive(true);
        // mainCamera.SetActive(true);
        // scopedCamera.SetActive(false);

    }
    IEnumerator onScopedSniper()
    {
        yield return new WaitForSeconds(timeScopeSniper);
        if (!isAiming)
        {
            onUnscopedSniper();
            yield break;
        }
        scopeOverlay.SetActive(true);
        snipScopParent.SetActive(true);
        // normalParent.SetActive(false);

        // normalParent.SetActive(false);
        CameraweaponsCamera.SetActive(false);
        // weaponListGameObject.GetComponent<MouseLook>().mouseSensitivity = weaponListGameObject.GetComponent<MouseLook>().mouseSensitivity / 3;
        // normalFOV = mainCamera.GetComponent<Camera>().fieldOfView;
        // mainCamera.GetComponent<Camera>().fieldOfView = scopedFOV;
        
        
    }

    // private void InputChooseWeapon()
    // {

    //     if (Input.GetKeyDown(KeyCode.Alpha0))
    //     {
    //         inventory.AddItem(ListWeapons[0], primarySecondary);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Alpha1))
    //     {
    //         inventory.AddItem(ListWeapons[1], primarySecondary);
    //     }

    //     else if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         inventory.AddItem(ListWeapons[2], primarySecondary);
    //     }

    //     else if (Input.GetKeyDown(KeyCode.Alpha3))
    //     {
    //         inventory.AddItem(ListWeapons[3], primarySecondary);
    //     }

    //     else if (Input.GetKeyDown(KeyCode.Alpha4))
    //     {
    //         inventory.AddItem(ListWeapons[4], primarySecondary);
    //     }

    //     else if (Input.GetKeyDown(KeyCode.Alpha5))
    //     {
    //         inventory.AddItem(ListWeapons[5], primarySecondary);
    //     }

    // }

    public void buttonPrimaryToSecondary()
    {
        if (primarySecondary == 0)
        {
            if (inventory.GetItem(primarySecondary + 1) != null)
            {
                ChangeWeapon(inventory.GetItem(primarySecondary), inventory.GetItem(primarySecondary + 1));
                primarySecondary = 1;

            }

        }
        else if(primarySecondary == 1)
        {
            if (inventory.GetItem(primarySecondary - 1) != null)
            {
                ChangeWeapon(inventory.GetItem(primarySecondary), inventory.GetItem(primarySecondary - 1));
                primarySecondary = 0;
            }
        }

    }

    public void buttonPrimaryToSecondary1()
    {
            if (inventory.GetItem(primarySecondary + 1) != null)
            {
                ChangeWeapon(inventory.GetItem(primarySecondary), inventory.GetItem(primarySecondary + 1));
                primarySecondary = 1;

            }

    }

    public void buttonPrimaryToSecondary2()
    {
            if (inventory.GetItem(primarySecondary - 1) != null)
            {
                ChangeWeapon(inventory.GetItem(primarySecondary), inventory.GetItem(primarySecondary - 1));
                primarySecondary = 0;
            }

    }

    private void callCrouch(bool isCrouching)
    {
        
        if (isCrouching)
        {
            controller.height = Mathf.Lerp(controller.height, crouchPosition, 0.001f * Time.deltaTime);
            if (controller.height - crouchPosition <= crouchPosition)
            {
                controller.height = crouchPosition;
            }
            walkSpeed = walkSpeedCrouching;
        }
        else if (!isCrouching)
        {
            float lastHeight = controller.height;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, initialCrouchPositon))
            {
                if (hit.distance <  initialCrouchPositon + 0.55 - crouchPosition)
                {
                    controller.height = crouchPosition + hit.distance;
                    //controller.height = Mathf.Lerp(controller.height, crouchPosition + hit.distance, 0.001f * Time.deltaTime);
                }
                else
                {
                    controller.height = Mathf.Lerp(controller.height, initialCrouchPositon, 0.001f * Time.deltaTime);
                }
            }
            else
            { 
                controller.height = Mathf.Lerp(controller.height, initialCrouchPositon, 0.001f * Time.deltaTime);
                if (controller.height + initialCrouchPositon >= initialCrouchPositon)
                {
                    controller.height = initialCrouchPositon;
                }


            }
            if (!isAiming)
            {
                walkSpeed = initialWalkSpeed;
            }
            else
            {
                walkSpeed = inventory.GetItem(primarySecondary).GetComponent<WeaponController>().walkSpeedAiming;
            }
            //transform.position += new Vector3(0, (controller.height - lastHeight / 2), 0);
        }

    }
    /*
    public void ChooseWeapon(int primarySecondary)
    {
        if (numberWeapons != currentNumberWeapon)
        {
            switch (numberWeapons)
            {
                case 0:
                    ChangeWeapon(0);
                    break;
                case 1:
                    ChangeWeapon(1);
                    break;
                case 2:
                    ChangeWeapon(2);
                    break;
                case 3:
                    ChangeWeapon(3);
                    break;
                case 4:
                    ChangeWeapon(4);
                    break;
                case 5:
                    ChangeWeapon(5);
                    break;
            }
        }
    }*/
    public void ChangeWeapon(GameObject beforeWeapon, GameObject afterWeapon)
    {
        StartCoroutine(ChangeWeaponCoroutine(beforeWeapon, afterWeapon));
    }

    private IEnumerator ChangeWeaponCoroutine(GameObject beforeWeapon, GameObject afterWeapon)
    {

        // Jouez l'animation "DrawReverse"
        characterAnimator.Play("DrawReverse");

        yield return new WaitForSeconds(beforeWeapon.GetComponent<WeaponController>().endDraw);

        beforeWeapon.SetActive(false);
        afterWeapon.SetActive(true);
        characterAnimator = afterWeapon.GetComponent<Animator>();
    }

    private float durationDrawReverse(int currentNumberWeapons)
    {
        switch (currentNumberWeapon)
        {
            case 0:
                return 0.36f;
            case 1:
                return 0.45f;
            case 2:
                return 0.45f;
            case 3:
                return 0.58f;
            case 4:
                return 0.56f;
            case 5:
                return 0.55f;

        }
        return 0.0f;
    }
    public void playerDie(){
        // StartCoroutine(EndGameGoToMenu());
        gameOver.enabled = true;
        slowMotion.StartSlowMotion();
        int kills = chargerUI.zombieSpawner.nbKills;
        int waves = chargerUI.zombieSpawner.numberOfWave;
        string time = chargerUI.timerText.text;
        SaveGameData(time, kills, waves);
        StartCoroutine(saveGameInfo(time, kills, waves));
    }
    public IEnumerator saveGameInfo(string time, int kills, int waves){
        yield return new WaitForSeconds(.5f);
        StartCoroutine(PostRequestGame("https://joudcazeaux.fr/ZomBriCaz/zombricazGameSave.php", PlayerPrefs.GetString("PlayerID"), kills, waves, time));
        Debug.Log("heyjeveuxallermenu");
        slowMotion.StopSlowMotion();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameManager.GetComponent<GameManager>().loadHome();
    }

    public void showHit(float value){
        if(value > 0){
            chargerUI.hitTouch.GetComponent<CanvasGroup>().alpha = 1.0f;
            SoundManager.Instance.PlaySound(hitmarkerSound);
            StartCoroutine(HideHitUI(chargerUI.hitTouch.GetComponent<CanvasGroup>(), 0.5f));
        }
        else{
            chargerUI.hitDead.GetComponent<CanvasGroup>().alpha = 1.0f;
            SoundManager.Instance.PlaySound(deathSound);
            StartCoroutine(HideHitUI(chargerUI.hitDead.GetComponent<CanvasGroup>(), 0.5f));
        }
        
        
    }
    IEnumerator HideHitUI(CanvasGroup uiElement, float delay)
        {
            float elapsedTime = 0f;
            float startAlpha = uiElement.alpha;

            while (elapsedTime < delay)
            {
                uiElement.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / delay);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            uiElement.alpha = 0f;
            // uiElement.enabled = false;
        }
    // public void goToHome(){
        
    //     Cursor.lockState = CursorLockMode.None;
    //     Cursor.visible = true;
    //     SceneManager.LoadScene(0);
    // }
    void SaveGameData(string time, int kills, int waves)
    {
        // Créez une instance de la classe GameData
        GameData gameData = new GameData
        {
            time = time,
            kills = kills,
            waves = waves
        };

        // Convertissez l'objet GameData en JSON
        string jsonData = JsonUtility.ToJson(gameData);

        // Enregistrez le JSON dans les PlayerPrefs (peut également être sauvegardé dans un fichier)
        PlayerPrefs.SetString("GameData", jsonData);
        PlayerPrefs.Save();

        SaveHighScore(gameData);
    }

    IEnumerator PostRequestGame(string uri, string name, int kills, int waves, string duration)
    {
        Debug.Log("test10");
        WWWForm form = new WWWForm();
        form.AddField("nameUser", name);
        form.AddField("kills", kills);
        form.AddField("wave", waves);
        form.AddField("duration", duration);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur lors de la requête POST : " + webRequest.error);
            }
            else
            {
                Debug.Log("Requête POST réussie : " + webRequest.downloadHandler.text);
            }
        }
    }

    
    void SaveHighScore(GameData gameData)
    {
        

        // Charger les données des meilleurs scores depuis PlayerPrefs
        string jsonData = PlayerPrefs.GetString("HighScores", "{}");
        HighScoreData highScoreData = JsonUtility.FromJson<HighScoreData>(jsonData);

        // Debug.Log("new JSON Data from PlayerPrefs: " + jsonData);
        // Debug.Log("newTimer: " + highScoreData.bestScore1);
        // Debug.Log("newTimer: " + highScoreData.bestScore1.time);

        // Debug.Log("newTimer : " + highScoreData.bestScore1);
        // Debug.Log("newTimer : " + highScoreData.bestScore1.time);
        // Comparer avec les trois meilleures performances
        TimeSpan newTime = TimeSpan.ParseExact(gameData.time, "mm':'ss", CultureInfo.InvariantCulture);
        // Debug.Log("newTimer : " + newTime);
        TimeSpan bestTime1 = TimeSpan.Zero;
        TimeSpan bestTime2 = TimeSpan.Zero;
        TimeSpan bestTime3 = TimeSpan.Zero;

        if (!string.IsNullOrEmpty(highScoreData.bestScore1.time))
        {
            bestTime1 = TimeSpan.ParseExact(highScoreData.bestScore1.time, "mm':'ss", CultureInfo.InvariantCulture);
        }

        if (!string.IsNullOrEmpty(highScoreData.bestScore2.time))
        {
            bestTime2 = TimeSpan.ParseExact(highScoreData.bestScore2.time, "mm':'ss", CultureInfo.InvariantCulture);
        }

        if (!string.IsNullOrEmpty(highScoreData.bestScore3.time))
        {
            bestTime3 = TimeSpan.ParseExact(highScoreData.bestScore3.time, "mm':'ss", CultureInfo.InvariantCulture);
        }

        // Debug.Log("newTimer : " + bestTime1);

        if (string.IsNullOrEmpty(highScoreData.bestScore1.time) || TimeSpan.Compare(newTime, bestTime1) > 0 ||
            (string.IsNullOrEmpty(highScoreData.bestScore1.time) && newTime == bestTime1 && gameData.waves > highScoreData.bestScore1.waves) ||
            (string.IsNullOrEmpty(highScoreData.bestScore1.time) && newTime == bestTime1 && gameData.waves == highScoreData.bestScore1.waves && gameData.kills > highScoreData.bestScore1.kills))
        {
            highScoreData.bestScore3 = highScoreData.bestScore2;
            highScoreData.bestScore2 = highScoreData.bestScore1;
            highScoreData.bestScore1 = gameData;
        }
        else if (string.IsNullOrEmpty(highScoreData.bestScore2.time) || TimeSpan.Compare(newTime, bestTime2) > 0 ||
            (string.IsNullOrEmpty(highScoreData.bestScore2.time) && newTime == bestTime2 && gameData.waves > highScoreData.bestScore2.waves) ||
            (string.IsNullOrEmpty(highScoreData.bestScore2.time) && newTime == bestTime2 && gameData.waves == highScoreData.bestScore2.waves && gameData.kills > highScoreData.bestScore2.kills))
        {
            highScoreData.bestScore3 = highScoreData.bestScore2;
            highScoreData.bestScore2 = gameData;
        }
        else if (string.IsNullOrEmpty(highScoreData.bestScore3.time) || TimeSpan.Compare(newTime, bestTime3) > 0 ||
            (string.IsNullOrEmpty(highScoreData.bestScore3.time) && newTime == bestTime3 && gameData.waves > highScoreData.bestScore3.waves) ||
            (string.IsNullOrEmpty(highScoreData.bestScore3.time) && newTime == bestTime3 && gameData.waves == highScoreData.bestScore3.waves && gameData.kills > highScoreData.bestScore3.kills))
        {
            highScoreData.bestScore3 = gameData;
        }

        



        // Convertir l'objet HighScoreData en JSON
        string newJsonData = JsonUtility.ToJson(highScoreData);

        // Enregistrer le JSON dans PlayerPrefs
        PlayerPrefs.SetString("HighScores", newJsonData);
        PlayerPrefs.Save();
    }
    public void callBlood(){
        StartCoroutine(HealthRecoveryRoutine());
    }
    // public IEnumerator HealthRecoveryRoutine()
    // {

    //         yield return new WaitForSeconds(8f); // Attendre 8 secondes entre chaque vérification

    //         // Vérifier si la santé du joueur est inférieure à 150
    //         if (healthPlayer < 150f && !isRecovering)
    //         {
    //             isRecovering = true;
    //             healthPlayer += 50f;

    //             healthPlayer = Mathf.Min(healthPlayer, 150f);

    //             isRecovering = false;
    //         }
    // }

    public IEnumerator HealthRecoveryRoutine()
    {
        float duration = 8f;

        // Attendre 8 secondes avant de commencer la récupération de santé

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;

            // Vérifier la récupération de la santé ici
            

                // Interpoler l'opacité de 1 à 0 sur la durée spécifiée
                float newAlpha = Mathf.Lerp(1f, 0.5f, elapsedTime / duration);

                // Appliquer la nouvelle opacité au CanvasGroup
                canvaBlood.GetComponent<CanvasGroup>().alpha = newAlpha;

               
        }
        if (healthPlayer < 150f && !isRecovering)
            {
                isRecovering = true;
                healthPlayer += 50f; 
                healthPlayer = Mathf.Min(healthPlayer, 150f);

                isRecovering = false;
            }

        // Assurer que l'opacité est bien à zéro à la fin
        
    }

    /*
    private void TimeAnimmationDrawBegin(int currentNumberWeapon)
    {
        switch (currentNumberWeapon)
        {
            case 0:
                
                characterAnimator.Play("DrawReverse", 0, 17);
                break;
            case 1:
                characterAnimator.Play("Draw", 0, 15);
                break;
            case 2:
                characterAnimator.Play("Draw", 0, 12);
                break;
            case 3:
                characterAnimator.Play("Draw", 0, 6);
                break;
            case 4:
                characterAnimator.Play("Draw", 0, 3);
                break;
            case 5:
                characterAnimator.Play("Draw", 0, 7);
                break;
        }
    }

    private void TimeAnimmationDrawReverseEnd(int currentNumberWeapon)
    {
        switch (currentNumberWeapon)
        {
            case 0:
                characterAnimator.PlayInFixedTime("DrawReverse", 0, 0.0f);
                break;
            case 1:
                characterAnimator.PlayInFixedTime("DrawReverse", 0, 0.36f);
                break;
            case 2:
                characterAnimator.PlayInFixedTime("DrawReverse", 0, 0.36f);
                break;
            case 3:
                characterAnimator.PlayInFixedTime("DrawReverse", 0, 0.36f);
                break;
            case 4:
                characterAnimator.PlayInFixedTime("DrawReverse", 0, 0.36f);
                break;
            case 5:
                characterAnimator.PlayInFixedTime("DrawReverse", 0, 0.36f);
                break;
        }
    }*/
}
