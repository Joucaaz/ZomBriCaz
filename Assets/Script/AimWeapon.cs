using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeapon : MonoBehaviour
{
    public Camera mainCamera; // Cam�ra principale
    public Transform standardCameraPosition; // Position standard de la cam�ra
    public Transform aimCameraPosition; // Position de vis�e de la cam�ra
    public float transitionDuration = 1.0f; // Dur�e de la transition en secondes

    private bool isAiming = false;
    private float transitionStartTime;

    private void Start()
    {
        // Assurez-vous que la cam�ra de vis�e est d�sactiv�e au d�part
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isAiming)
        {
            isAiming = true;
            transitionStartTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(1) && isAiming)
        {
            isAiming = false;
            transitionStartTime = Time.time;
        }

        if (isAiming)
        {
            // Calcule la progression de la transition
            float progress = (Time.time - transitionStartTime) / transitionDuration;
            // Interpole progressivement la position et la rotation de la cam�ra
            mainCamera.transform.position = Vector3.Lerp(standardCameraPosition.position, aimCameraPosition.position, progress);
            mainCamera.transform.rotation = Quaternion.Slerp(standardCameraPosition.rotation, aimCameraPosition.rotation, progress);
        }
        else
        {
            float progress = (Time.time - transitionStartTime) / transitionDuration;
            // Une fois que le clic droit est rel�ch�, retour � la position standard
            mainCamera.transform.position = Vector3.Lerp(aimCameraPosition.position, standardCameraPosition.position, progress);
            mainCamera.transform.rotation = Quaternion.Slerp(aimCameraPosition.rotation, standardCameraPosition.rotation, progress);
        }
    }
}
