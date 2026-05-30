using UnityEngine;

public class GetMousePosition : MonoBehaviour
{
    public InputSystem_Actions inputActions;
    [SerializeField] Vector2 mousePosition;


    private void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        inputActions.BuildMode.MousToScreenPosition.performed += x => mousePosition = x.ReadValue<Vector2>();
    }

    
}
