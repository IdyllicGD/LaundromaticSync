using Unity.VisualScripting;
using UnityEngine;

public class LightSwitchToggle : MonoBehaviour
{
    public GameObject lightSwitch;
    private bool LightsON = true;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleLights();
        }
        
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
