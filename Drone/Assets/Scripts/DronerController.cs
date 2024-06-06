using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;
public class DronerController : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float forceBalance;
    [SerializeField] private float liftSpeed;
    [SerializeField] private float pitchSpeed;//Lift Up Down
    [SerializeField] private float rollSpeed;//Tilt Left Right
    [SerializeField] private float yawSpeed;//Rotate Left Right
    [SerializeField] private float moveSpeedDefault;
    [SerializeField] private Transform joystickHandleRight;

    private Rigidbody rb;
    private bool isMoving;
    private bool isBalancing;
    private bool isLooking;

    private float timeToModifySpeed = 3f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        gameInput.droneInputAction.Droner.Move.canceled += Move_canceled;
        gameInput.droneInputAction.Droner.Move.performed += Move_performed;
        gameInput.droneInputAction.Droner.Look.performed += Look_performed;
        gameInput.droneInputAction.Droner.Look.canceled += Look_canceled;
    }

    private void Look_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isLooking = false;
        rb.velocity = Vector3.zero;
    }

    private void Look_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isLooking = true;
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
        rb.velocity = Vector3.zero;
        moveSpeed = 0f;
        timeToModifySpeed = 3f;
    }

    private void Update()
    {
        if (isBalancing)
        {
            BalanceDrone();
        }

        ChangeSpeed();
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            HandleMovementRightJoystick();
        }
        if (isLooking)
        {
            HandleLookLeftJoystick();
        }
    }
    private Vector3 GetMoveDirection()
    {
        Vector2 moveInput = gameInput.GetMoveInput().normalized;

        return new Vector3(moveInput.x, 0f, moveInput.y);
    }
    private void HandleMovementRightJoystick()
    {
        Vector3 moveDirec = GetMoveDirection();

        float angleAxisY = transform.eulerAngles.y;

        transform.Rotate(pitchSpeed * moveDirec.z, 0f, -rollSpeed * moveDirec.x);

        Vector3 newEulerAngle = transform.eulerAngles;

        if (newEulerAngle.x > 5f && newEulerAngle.x < 180f)
        {
            newEulerAngle.x = 5;
        }
        else if (newEulerAngle.x < 355f && newEulerAngle.x > 180f)
        {
            newEulerAngle.x = 355f;
        }
        if (newEulerAngle.z > 5f && newEulerAngle.z < 180f)
        {
            newEulerAngle.z = 5f;
        }
        else if (newEulerAngle.z < 355f && newEulerAngle.z > 180f)
        {
            newEulerAngle.z = 355f;
        }
        newEulerAngle.y = angleAxisY;

        transform.eulerAngles = newEulerAngle;

        Vector3 dir = transform.forward * moveDirec.z + transform.right * moveDirec.x;

        dir.y = 0;

        rb.velocity = dir * moveSpeed;
    }
    private void BalanceDrone()
    {
        Vector3 currentEulerAngles = transform.eulerAngles;
        Quaternion currentRotation = transform.rotation;

        // Mục tiêu là quay về (0, currentY, 0)
        Quaternion targetRotation = Quaternion.Euler(0, currentEulerAngles.y, 0);

        float balanceSpeed = forceBalance * Time.deltaTime;

        // Sử dụng Slerp để làm mượt quá trình cân bằng
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, balanceSpeed);

        // Kiểm tra nếu drone đã gần như cân bằng
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
        {
            transform.rotation = targetRotation;
            isBalancing = false;
        }
    }
    private Vector3 GetLookDirection()
    {
        Vector2 moveInput = gameInput.GetLookInput().normalized;

        return new Vector3(moveInput.x, 0f, moveInput.y);
    }
    private void HandleLookLeftJoystick()
    {
        Vector3 lookDirec = GetLookDirection();

        rb.velocity = new Vector3(rb.velocity.x, lookDirec.z * liftSpeed, rb.velocity.z);

        float angleAxisX = transform.eulerAngles.x;

        float angleAxisZ = transform.eulerAngles.z;

        transform.Rotate(0f, yawSpeed * lookDirec.x, 0f);

        Vector3 newEulerAngle = transform.eulerAngles;

        newEulerAngle.x = angleAxisX;

        newEulerAngle.z = angleAxisZ;

        transform.eulerAngles = newEulerAngle;
    }
    private void OnDisable()
    {
        gameInput.droneInputAction.Droner.Move.canceled -= Move_canceled;
        gameInput.droneInputAction.Droner.Look.canceled -= Look_canceled;
        gameInput.droneInputAction.Droner.Move.performed -= Move_performed;
        gameInput.droneInputAction.Droner.Look.performed -= Look_performed;
    }
    public float GetMoveVelocity()
    {
        return Mathf.Round(moveSpeed);
    }
    private void ChangeSpeed()
    {
        if (isMoving)
        {
            if (timeToModifySpeed > 0f)
            {
                moveSpeed += Time.deltaTime * 20f;
                timeToModifySpeed -= Time.deltaTime;
                moveSpeed = Mathf.Clamp(moveSpeed, 0f, moveSpeedDefault);
            }
        }
    }
}
