using UnityEngine;

public class GameInput : MonoBehaviour
{
    public DroneInputAction droneInputAction;
    private void Awake()
    {
        droneInputAction = new DroneInputAction();      
    }
    public Vector2 GetMoveInput()
    {
        return droneInputAction.Droner.Move.ReadValue<Vector2>();
    }
    public Vector2 GetLookInput()
    {
        return droneInputAction.Droner.Look.ReadValue<Vector2>();
    }
    private void OnEnable()
    {
        droneInputAction.Droner.Enable();
    }
    private void OnDisable()
    {
        droneInputAction.Droner.Disable();
    }
}
