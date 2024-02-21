
using UnityEngine;
using UnityEngine.AI;

public class ScriptDamage : MonoBehaviour
{
    public enum collisionType { head, body, arms};
    public collisionType damageType;
    public GameObject gameOplayer;
    public PlayerMovement2 player;
    private ChargerUI chargerUI;
    [SerializeField] private Zombie zombieController;
    private NavMeshAgent nav;
    


    // Start is called before the first frame update
    void Start()
    {
        nav = zombieController.GetComponent<NavMeshAgent>();
        gameOplayer = GameObject.FindGameObjectWithTag("Player");
        player = gameOplayer.GetComponent<PlayerMovement2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hit(float value)
    {
        try
        {
            zombieController.zombieHealth -= value;
            player.showHit(zombieController.zombieHealth);
            if (zombieController.zombieHealth <= 0 && zombieController.isDead == false)
            {               
                zombieController.isDead = true;
                zombieController.zombieDie();
            }
        }
        catch 
        {
            Debug.Log("error hit");
        }
        
    }

}
