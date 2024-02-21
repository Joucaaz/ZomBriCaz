using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Camera playerCamera; // Référence à la caméra du joueur.
    public float distance = 10f; // Distance à laquelle le curseur sera positionné.

    void Update()
    {
        // Obtenez le point de visée à partir de la caméra.
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        Vector3 targetPoint = ray.GetPoint(distance);

        // Orientez le curseur vers le point de visée.
        transform.LookAt(targetPoint);
    }
}
