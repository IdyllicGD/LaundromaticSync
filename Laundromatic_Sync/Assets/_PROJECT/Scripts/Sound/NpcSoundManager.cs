using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class NpcSoundManager : MonoBehaviour
{
    [Title("Npc Sounds", null, TitleAlignments.Centered)]
    [SerializeField] StudioEventEmitter talkingSound;

    public void PlaySound()
    {
        talkingSound.Play();
    }
    public void StopSound()
    {
        talkingSound.Stop();
    }
}
