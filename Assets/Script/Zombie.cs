using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public GameObject player; // R�f�rence au personnage que le zombie doit suivre
    public Animator zombieAnimator;
    public float stopDistance = 0.5f; // Distance � laquelle le zombie attaque le personnage
    private NavMeshAgent zombieNavMesh;
    public float zombieHealth;
    private bool isAttacking = false;
    private float minSpeed = 4.0f;
    private float moySpeed = 4.5f;

    private float maxSpeed = 6.0f;

    private float distance;
    private bool isRunning;
    private bool isMoving;

    public bool isDead = false;
    private float soust;
    public bool zombieAttack = false;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement2 playerMovement2 = player.GetComponent<PlayerMovement2>();
        zombieNavMesh = GetComponent<NavMeshAgent>();
        // Debug.Log(zombieNavMesh.name);

        zombieNavMesh.speed = Random.Range(minSpeed, maxSpeed); // Vitesse al�atoire entre 1.0 et 3.0
        //zombieNavMesh.angularSpeed = Random.Range(120.0f, 360.0f); // Vitesse angulaire al�atoire
        zombieNavMesh.acceleration = zombieNavMesh.speed;
        if (zombieNavMesh.speed < moySpeed){
            isRunning = false;
            isMoving = true;
        }
        else{
            isRunning = true;
            isMoving = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf && zombieNavMesh.enabled)
        {
                
    
                distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
                //isRunning = (zombieNavMesh.velocity.magnitude > 1) && (zombieNavMesh.speed >= 1.5f);
                //isMoving = (zombieNavMesh.velocity.magnitude > 1) && (zombieNavMesh.speed < 1.5f);
                if(distance <= stopDistance){
                    zombieAttackHit();
                    zombieNavMesh.destination = gameObject.transform.position ;
                }
                else{
                    zombieNavMesh.destination = player.transform.position ;
                }
                if(zombieAttack){
                    zombieStopWalk();
                    
                    // zombieNavMesh.velocity = Vector3.zero;
                }
                else{
                    zombieWalk();
                    
                }
                // if (zombieAnimator.GetCurrentAnimatorStateInfo(1).IsName("Attack") || zombieAnimator.GetCurrentAnimatorStateInfo(1).IsName("Attack2"))
                // {
                //     zombieNavMesh.velocity = Vector3.zero;
                //     if (distance <= stopDistance)
                //     {
                //         zombieStopWalk();
                        
                //         zombieAttackHit();
                //     }
                //     else if(zombieHealth > 0)
                //     {
                        
                //         zombieWalk();
                //     }
                // }
                // else{
                //     Debug.Log("non");

                // }

                
                
            /*
            Collider[] colliders = Physics.OverlapSphere(transform.position, collisionRadius, zombieLayer);

            foreach (Collider collider in colliders)
            {
                if (collider.transform != transform)
                {
                    // Les zombies se trouvent trop pr�s les uns des autres, faites quelque chose pour les �viter
                    Vector3 avoidanceDirection = (transform.position - collider.transform.position).normalized;
                    Vector3 newDestination = transform.position + avoidanceDirection * stopDistance;
                    zombieNavMesh.SetDestination(newDestination);
                }
            }
            */

        }

    }

    public void ActivateVelocity(){

    }
    private void zombieWalk()   
    {
        if(zombieNavMesh.speed >= moySpeed){
            zombieAnimator.SetBool("isRunning", isRunning);
        }
        else{
            zombieAnimator.SetBool("isMoving", isMoving);
        }
    }
    private void zombieStopWalk()
    {
        if(zombieNavMesh.speed >= moySpeed){
            zombieAnimator.SetBool("isRunning", !isRunning);
        }
        else{
            zombieAnimator.SetBool("isMoving", !isMoving);
        }
    }


    private void zombieAttackHit()
    {
        int randomValue = Random.Range(0, 2);
        zombieAttack = true;
        // float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
        //Debug.Log(isAttacking);
        if (!zombieAnimator.GetCurrentAnimatorStateInfo(1).IsName("Attack") && !zombieAnimator.GetCurrentAnimatorStateInfo(1).IsName("Attack2"))
        {
            isAttacking = false;
        }

        if (!isAttacking){
            
            if (randomValue == 0)
            {
                isAttacking = true;
                zombieAnimator.Play("Attack");
            }
            else
            {
                isAttacking = true;
                zombieAnimator.Play("Attack2");
            }
        }
        
    }

    
    public void zombieDie()
    {
    
        Debug.Log(player.gameObject);
        Debug.Log(player.GetComponent<PlayerMovement2>().money);
        
        

        int randomValue = Random.Range(0, 2);
        if (randomValue == 0)
        {
            zombieAnimator.SetBool("isDead", true);
        }
        else
        {
            zombieAnimator.SetBool("isDeadFront", true);
        }
        zombieAnimator.SetBool("isRunning", false);
        zombieAnimator.SetBool("isMoving", false);
        zombieAnimator.SetLayerWeight(1, 0f);
        zombieNavMesh.enabled = false;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        //player.GetComponent<PlayerMovement2>().money += 10;
        Destroy(gameObject, 30f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            Debug.Log("Player hit");
        }
    }
}
