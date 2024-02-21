using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private GameObject parentHoleBullet;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] public AudioClip reloadClip;
    [SerializeField] public Sprite weaponImage;
    public ParticleSystem flash;

    public Transform weaponTransform;
    public Camera camera;
    public Transform cursorUI;

    private ScriptDamage damage;
    public float zombieDamageToPlayer;

    //public GameObject knife;

    public float endDraw;

    public Vector3 offsetDistance;
    public Vector3 offsetDistanceUI;
    public Vector3 offsetDistanceAiming;
    public Vector3 offsetDistanceAimingUI;

    public int bulletsInside;
    public int reloadingConsidered;
    public int maxBulletsInOneMagazine;
    public int numberofMagazine;

    public int bulletsTotal;

    // public int BulletsTotal
    // {
    //     get => bulletsTotal;
    //     set => bulletsTotal = value >= 0 ? value : 0;
    // }
    public float timeBetweenShots = 0.10f;
    public float timeForRecoil = 0.50f;
    private float lastShotTime = 0f;
    public float timeBetweenReload = 0.10f;

    public float recoilAmount;
    public float recoilAmountAim;

    public float walkSpeedAiming;

    public int bulletsTotalInitial;

    public float distanceOfShot;

    void Start()
    {
        maxBulletsInOneMagazine = bulletsInside;
        bulletsTotal = maxBulletsInOneMagazine * numberofMagazine;
        bulletsTotalInitial = bulletsTotal;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // camera = Camera.main;
    }

    public void Fire()
    {
        damage = null;
        flash.Play();
        SoundManager.Instance.PlaySound(shotClip);

        // float recoilAngleX = UnityEngine.Random.Range(-recoilAmount, recoilAmount);
        // float recoilAngleY = UnityEngine.Random.Range(-recoilAmount, recoilAmount);
        // float recoilAngleZ = UnityEngine.Random.Range(-recoilAmount, recoilAmount);

        GameObject obj;

        Vector3 screenPos = camera.WorldToScreenPoint(cursorUI.position);
        float offsetX = UnityEngine.Random.Range(-recoilAmount, recoilAmount);
        float offsetY = UnityEngine.Random.Range(-recoilAmount, recoilAmount);
        screenPos += new Vector3(offsetX, offsetY, 0f);
        Ray ray = camera.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        RaycastHit hit;
        int collisionCount = 0;
        while (Physics.Raycast(ray, out hit, distanceOfShot) && collisionCount < 3)
        {
            if (hit.collider is MeshCollider || hit.collider.gameObject.layer == LayerMask.NameToLayer("Zombie")){
                if (!checkHit(hit))
                {
                    obj = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    obj.transform.position += obj.transform.forward / 1000;
                    obj.transform.SetParent(parentHoleBullet.transform);
                    Destroy(obj, 15f);

                }
                collisionCount++;
                
            }
            ray.origin = hit.point + ray.direction * 0.01f;
            
        }

    }
    public void FireAiming()
    {
        damage = null;
        flash.Play();
        SoundManager.Instance.PlaySound(shotClip);

        GameObject obj;

        Vector3 screenPos = camera.WorldToScreenPoint(cursorUI.position);
        Ray ray = camera.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        RaycastHit hit;
        int collisionCount = 0;
        while (Physics.Raycast(ray, out hit, distanceOfShot) && collisionCount < 3)
        {
            if (hit.collider is MeshCollider || hit.collider.gameObject.layer == LayerMask.NameToLayer("Zombie")){
                if (!checkHit(hit))
                {
                    obj = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    obj.transform.position += obj.transform.forward / 1000;
                    obj.transform.SetParent(parentHoleBullet.transform);
                    Destroy(obj, 15f);

                }
                collisionCount++;
                
            }
            ray.origin = hit.point + ray.direction * 0.01f;
            
        }

    }
    public void FireAimingWithRecoil()
    {
        damage = null;
        flash.Play();
        SoundManager.Instance.PlaySound(shotClip);

        GameObject obj;

        Vector3 screenPos = camera.WorldToScreenPoint(cursorUI.position);
        float offsetX = UnityEngine.Random.Range(-recoilAmountAim, recoilAmountAim);
        float offsetY = UnityEngine.Random.Range(-recoilAmountAim, recoilAmountAim);
        screenPos += new Vector3(offsetX, offsetY, 0f);
        Ray ray = camera.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        RaycastHit hit;
        int collisionCount = 0;
        while (Physics.Raycast(ray, out hit, distanceOfShot) && collisionCount < 3)
        {
            if (hit.collider is MeshCollider || hit.collider.gameObject.layer == LayerMask.NameToLayer("Zombie")){
                if (!checkHit(hit))
                {
                    obj = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    obj.transform.position += obj.transform.forward / 1000;
                    obj.transform.SetParent(parentHoleBullet.transform);
                    Destroy(obj, 15f);

                }
                collisionCount++;
                
            }
            ray.origin = hit.point + ray.direction * 0.01f;
            
        }
    }

    public void FireShotgun()
    {
        damage = null;
        flash.Play();
        SoundManager.Instance.PlaySound(shotClip);

        for (int i = 0; i < 3; i++)
        {

            GameObject obj;
            
            Vector3 screenPos = camera.WorldToScreenPoint(cursorUI.position);
            float offsetX = UnityEngine.Random.Range(-recoilAmountAim, recoilAmountAim);
            float offsetY = UnityEngine.Random.Range(-recoilAmountAim, recoilAmountAim);
            screenPos += new Vector3(offsetX, offsetY, 0f);
            Ray ray = camera.ScreenPointToRay(screenPos);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
            RaycastHit hit;

            // Effectuez le raycast pour chaque rayon.
            while (Physics.Raycast(ray, out hit, distanceOfShot))
            {
                if (hit.collider is MeshCollider || hit.collider.gameObject.layer == LayerMask.NameToLayer("Zombie")){
                    if (!checkHit(hit))
                    {
                        obj = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        obj.transform.position += obj.transform.forward / 1000;
                        obj.transform.SetParent(parentHoleBullet.transform);
                        Destroy(obj, 15f);
                    }
                }

                // Avancez le point de d�part du rayon pour continuer le raycast.
                ray.origin = hit.point + ray.direction * 0.01f;
            }
        }

    }

    private bool checkHit(RaycastHit hit)
    {
        try
        {
            damage = hit.transform.GetComponent<ScriptDamage>();
            // Debug.Log(damage.damageType);
            switch (damage.damageType)
            {
                case ScriptDamage.collisionType.head:
                    damage.hit(zombieDamageToPlayer);
                    return true;
                case ScriptDamage.collisionType.body:
                    damage.hit(zombieDamageToPlayer / 2);
                    return true;
                case ScriptDamage.collisionType.arms:
                    damage.hit(zombieDamageToPlayer / 4);
                    return true;
            }
        }
        catch
        {
            return false;

        }
        return false;

    }
}
