using UnityEngine;

public class GestionAmbiance : MonoBehaviour
{
    public AudioClip ambiancePort;
    public AudioClip ambianceCity;
    public AudioClip ambianceGarage;
    public AudioClip ambianceCenter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MapPort"))
        {
            SoundManager.Instance.PlayAmbiance(ambiancePort);
        }
        else if (other.CompareTag("MapCity"))
        {
            SoundManager.Instance.PlayAmbiance(ambianceCity);
        }
        else if (other.CompareTag("MapGarage"))
        {
            SoundManager.Instance.PlayAmbiance(ambianceGarage);
        }
        else if (other.CompareTag("MapCenter"))
        {
            SoundManager.Instance.PlayAmbiance(ambianceCenter);
        }
    }
}
