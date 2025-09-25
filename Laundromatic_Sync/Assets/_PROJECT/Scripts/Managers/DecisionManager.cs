using Sirenix.OdinInspector;
using UnityEngine;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager instance;

    [Title("Choices made", null, TitleAlignments.Centered)]
    public bool helpedCop;
    public bool helpedKaren;
    public bool helpedLifegaurd;
    [Title("Npc's Met", null, TitleAlignments.Centered)]
    public bool talkedToLifeGaurd;
    public bool talkedToKaren;
    public bool talkedToCop;

    void Start()
    {
        instance = this;
    }
}
