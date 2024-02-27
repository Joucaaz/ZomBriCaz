using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEventReload : StateMachineBehaviour
{
    public float minimumNormalizedTime = 0.99f;
    public int newBulletsInside = 40;
    private bool enTrain = false;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement2 playerMovement = animator.gameObject.GetComponentInParent<MouseLook>().GetComponentInParent<PlayerMovement2>();
        WeaponController weaponController = animator.gameObject.GetComponentInParent<WeaponController>();
        
        if (stateInfo.normalizedTime >= minimumNormalizedTime && !enTrain)
        {
            
            // Debug.Log("test");
            if (playerMovement != null)
            {
                weaponController.bulletsTotal = weaponController.bulletsTotal - (weaponController.maxBulletsInOneMagazine - weaponController.bulletsInside);
                if(weaponController.bulletsTotal <= 0){
                    weaponController.bulletsTotal = 0;
                }
                weaponController.bulletsInside = newBulletsInside;
                playerMovement.reloadingEnCours = false;
                enTrain = true;
                
            }
                
            else
            {
                //Debug.LogWarning("Le GameObject du joueur n'a pas �t� r�f�renc�.");
            }
        }
        else{
            // playerMovement.reloadingEnCours = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        enTrain = false;
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
