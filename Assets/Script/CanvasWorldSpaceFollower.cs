using UnityEngine;

public class CanvasWorldSpaceFollower : MonoBehaviour
{
    public Transform target; // L'objet que vous souhaitez suivre
    public Vector3 offset;   // Un décalage optionnel pour ajuster la position du Canvas


    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
         
    }
    
}
