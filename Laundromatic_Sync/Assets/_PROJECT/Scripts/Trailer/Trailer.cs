using System;
using System.Collections;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trailer : MonoBehaviour
{
    InputSystem_Actions inputAction;

    [SerializeField] string textTheNpcWillSay;
    [SerializeField] TextMeshProUGUI textToChange;
    [SerializeField] Animator anim;
    [SerializeField] Animation animationToPlay;
    [SerializeField] StudioEventEmitter sound;
    [SerializeField] float waitTimeBeforeEnd;

    void Awake()
    {
        inputAction = new InputSystem_Actions();
        inputAction.Player.Interact.performed += x => npcStartTalkAndJump();
    }

    private void npcStartTalkAndJump()
    {
        textToChange.text = textTheNpcWillSay;
        sound.Play();
        animationToPlay.Play();
        //Start anim


    }

    IEnumerator TimeToWait(float time)
    {
        yield return new WaitForSeconds(time);
        animationToPlay.Stop();
        sound.Stop();
        textToChange.text = string.Empty;
    }
}
