using FMODUnity;
using UnityEngine;

public class OnTouchPlaySound : MonoBehaviour
{
    [SerializeField] StudioEventEmitter soundToPlay;

    void OnCollisionEnter(Collision collision)
    {
        soundToPlay.Play();
    }
}
