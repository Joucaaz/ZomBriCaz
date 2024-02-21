using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Camera playerCamera; // R�f�rence � la cam�ra du joueur.
    public float distance = 10f; // Distance � laquelle le curseur sera positionn�.

    void Update()
    {
        // Obtenez le point de vis�e � partir de la cam�ra.
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        Vector3 targetPoint = ray.GetPoint(distance);

        // Orientez le curseur vers le point de vis�e.
        transform.LookAt(targetPoint);
    }
}
