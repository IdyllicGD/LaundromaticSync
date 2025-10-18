using UnityEngine;
using UnityEngine.UI;

public class ResetUiLayout : MonoBehaviour
{
    public void ResetUi()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
