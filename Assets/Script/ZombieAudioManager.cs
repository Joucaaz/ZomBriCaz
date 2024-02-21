using UnityEngine;

public class ZombieAudioManager : MonoBehaviour
{
    public AudioClip[] zombieSound;
    

    public float soundDistanceThreshold = 10f;

    private Transform playerTransform;
    [SerializeField]
    private Transform zombiesParent;
    private GameObject[] zombies;
    public PlayerMovement2 player;
    void Start()
    {
        
        playerTransform = Camera.main.transform;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement2>();

    }


    void Update()
    {
        if (zombiesParent != null)
        {
            int childCount = zombiesParent.childCount;
            zombies = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                zombies[i] = zombiesParent.GetChild(i).gameObject;
            }

            // Vous avez maintenant un tableau de tous les enfants du parent.
        }

        foreach (GameObject zombie in zombies)
        {
            float distanceToPlayer = CalculateDistanceToPlayer(zombie);
            
            if (distanceToPlayer < soundDistanceThreshold && player && !player.pauseManager.isGamePaused)
            {
                PlayZombieSound(zombie, distanceToPlayer);
            }
        }
    }

    float CalculateDistanceToPlayer(GameObject zombie)
    {
        // Code pour calculer la distance du zombie au joueur
        return Vector3.Distance(zombie.transform.position, playerTransform.position);
    }

    void PlayZombieSound(GameObject zombie, float distanceToPlayer)
    {
        if(!zombie.GetComponent<Zombie>().isDead){
            AudioSource zombieAudio = zombie.GetComponent<AudioSource>();

            if (zombieAudio == null)
            {
                zombieAudio = zombie.AddComponent<AudioSource>();
                
                zombieAudio.spatialBlend = 1f; // 3D sound
                zombieAudio.maxDistance = soundDistanceThreshold; // Maximum distance for attenuation
            }

            // Calcul de la direction du son (panning)
            Vector3 directionToPlayer = playerTransform.position - zombie.transform.position;
            float pan = Mathf.Clamp(directionToPlayer.x / soundDistanceThreshold, -1f, 1f);

            // Calcul de l'intensité du son (atténuation en fonction de la distance)
            float volume = 1f - (distanceToPlayer / soundDistanceThreshold);

            // Correction pour normaliser la valeur entre 0 et 1
            volume = Mathf.Clamp01(volume);

            // Appliquer le pan et le volume
            zombieAudio.panStereo = pan;
            zombieAudio.volume = volume;

            if (!zombieAudio.isPlaying)
            {
                int randomZombieSound = Random.Range(0,3);
                zombieAudio.clip = zombieSound[randomZombieSound];
                zombieAudio.Play();
            }
        }
        
    }
}
