using System;
using System.Collections;
using FMODUnity;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Trailer : MonoBehaviour
{
    InputSystem_Actions inputAction;
    [SerializeField] npcAnimations animationToPlay;
    [Title("The string below will be shown on the npc!")]
    [TextArea]
    [SerializeField] string textTheNpcWillSay;
    [SerializeField] bool shouldITalk;

    [Title("Other")]
    [SerializeField] GameObject npcCanvas;
    [SerializeField] TextMeshProUGUI textToChange;
    [SerializeField] Animator anim;
    [SerializeField] StudioEventEmitter sound;
    [SerializeField] float waitTimeBeforeEnd;

    void Awake()
    {
        inputAction = new InputSystem_Actions();
        inputAction.Enable();
        inputAction.Player.Interact.performed += x => npcStartTalkAndJump();
    }

    private void npcStartTalkAndJump()
    {
        if (!shouldITalk) return;
        switch (animationToPlay)
        {
            case npcAnimations.hopping:
                anim.SetBool("IsLooped", true);
                anim.SetBool("IsHopping", true);
                break;
            case npcAnimations.die:
                anim.SetBool("DoDie", true);
                break;
            case npcAnimations.idle:
                anim.SetBool("IsIdle", true);
                break;
        }
        Debug.Log("Ran");
        
        npcCanvas.SetActive(true);
        textToChange.text = textTheNpcWillSay;
        sound.Play();
        StartCoroutine(TimeToWait(waitTimeBeforeEnd));

        //Start anim


    }

    IEnumerator TimeToWait(float time)
    {
        yield return new WaitForSeconds(time);
        npcCanvas.SetActive(false);
        anim.SetBool("IsHopping", false);
        anim.SetBool("IsLooped", false);
        anim.SetBool("IsIdle", true);
        sound.Stop();
        textToChange.text = string.Empty;
    }
}

public enum npcAnimations
{
    hopping, die, idle
}
