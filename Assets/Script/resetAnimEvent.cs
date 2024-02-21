
using UnityEngine;

public class resetAnimEvent : MonoBehaviour
{
    public void DestroyText(){
        // Debug.Log(gameObject);
        Destroy(gameObject, 0.5f);
    }
}
