using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DronerController : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float forceBalance;
    [SerializeField] private float liftSpeed;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float pitchSpeed;//Lift Up Down
    [SerializeField] private float rollSpeed;//Tilt Left Right
    [SerializeField] private float yawSpeed;//Rotate Left Right

    private Rigidbody rb;
    private bool isMoving;
    private bool isBalancing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        gameInput.droneInputAction.Droner.Move.canceled += Move_canceled;
        gameInput.droneInputAction.Droner.Move.performed += Move_performed;
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isMoving = true;
        isBalancing = false;
    }

    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isMoving = false;
        isBalancing = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            HandleMovementRightJoystick();
        }
        if (isBalancing)
        {
            BalanceDrone();
        }
        HandleLookLeftJoystick();
    }
    private Vector3 GetMoveDirection()
    {
        Vector2 moveInput = gameInput.GetMoveInput();

        return new Vector3(moveInput.x, 0f, moveInput.y);
    }
    private void HandleMovementRightJoystick()
    {
        Vector3 moveDirec = GetMoveDirection();

        float angleAxisY = transform.eulerAngles.y;

        transform.Rotate(pitchSpeed * moveDirec.z * Time.deltaTime, 0f, -rollSpeed * moveDirec.x * Time.deltaTime);

        Vector3 newEulerAngle = transform.eulerAngles;

        if (newEulerAngle.x > 30f && newEulerAngle.x < 180f)
        {
            newEulerAngle.x = 30f;
        }
        else if (newEulerAngle.x < 330f && newEulerAngle.x > 180f)
        {
            newEulerAngle.x = 330f;
        }
        if (newEulerAngle.z > 30f && newEulerAngle.z < 180f)
        {
            newEulerAngle.z = 30f;
        }
        else if (newEulerAngle.z < 330f && newEulerAngle.z > 180f)
        {
            newEulerAngle.z = 330f;
        }
        newEulerAngle.y = angleAxisY;

        transform.eulerAngles = newEulerAngle;      

        transform.position += moveDirec * moveSpeed * Time.deltaTime;
    }
    private void BalanceDrone()
    {
        Vector3 newEulerAngle = transform.eulerAngles;

        if (newEulerAngle.x <= 31f)
        {
            newEulerAngle.x -= forceBalance * Time.deltaTime;
        }
        if (newEulerAngle.x >= 329f)
        {
            newEulerAngle.x += forceBalance * Time.deltaTime;
        }
        if (newEulerAngle.z <= 31f)
        {
            newEulerAngle.z -= forceBalance * Time.deltaTime;
        }
        if (newEulerAngle.z >= 329f)
        {
            newEulerAngle.z += forceBalance * Time.deltaTime;
        }
        if (Mathf.Abs(newEulerAngle.x) < .5f)
        {
            newEulerAngle.x = 0f;
        }
        if (Mathf.Abs(newEulerAngle.z) < .5f)
        {
            newEulerAngle.z = 0f;
        }
        if (newEulerAngle.z ==0 && newEulerAngle.x == 0)
        {
            isBalancing = false;
        }
        transform.eulerAngles = newEulerAngle;
    }
    private Vector3 GetLookDirection()
    {
        Vector2 moveInput = gameInput.GetLookInput();

        return new Vector3(moveInput.x, 0f, moveInput.y);
    }
    private void HandleLookLeftJoystick()
    {
        Vector3 lookDirec = GetLookDirection();

        transform.position += new Vector3(0f, lookDirec.z * liftSpeed * Time.deltaTime, 0f);

        float angleAxisX = transform.eulerAngles.x;

        float angleAxisZ = transform.eulerAngles.z;

        transform.Rotate(0f, tiltSpeed * lookDirec.x * Time.deltaTime, 0f);

        Vector3 newEulerAngle = transform.eulerAngles;

        newEulerAngle.x = angleAxisX;

        newEulerAngle.z = angleAxisZ;

        transform.eulerAngles = newEulerAngle;
    }
    private void OnDisable()
    {
        gameInput.droneInputAction.Droner.Move.canceled -= Move_canceled;
        gameInput.droneInputAction.Droner.Move.performed -= Move_performed;
    }
}
