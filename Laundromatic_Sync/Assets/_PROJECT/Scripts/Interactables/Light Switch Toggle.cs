using _PROJECT._Jacques.Scripts.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class LightSwitchToggle : MonoBehaviour, Iinteractable
{
    public GameObject lightSwitch;
    private bool LightsON = true;

    public void Interact()
    {
        ToggleLights();
    }

    public void ToggleLights()
    {
        LightsON = !LightsON;

        foreach (Transform child in transform)
        {
            Light light = child.GetComponent<Light>();
            if (light != null) light.enabled = LightsON;
        }

    }
}
