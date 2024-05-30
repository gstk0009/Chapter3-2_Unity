using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float EquipItemSpeed;
    public float UseConsumableItemSpeed;
    private Vector2 curMovementInput;
    private Vector3 beforeDirection;
    public LayerMask GroundLayerMask;

    [Header("Jump")]
    public float JumpPower;
    public float EquipItemJump;
    public float UseConsumableJump;
    public float useJumpStamina;
    public float jumpRate;
    private bool jumping;

    [Header("Run")]
    public float runPower;
    public float userunStamina;
    private bool running;

    [Header("Look")]
    public Transform CameraContrainer;
    public float MinXLook;
    public float MaxXLook;
    private float camCurXRot;
    public float LookSensitivity;
    private Vector2 mouseDelta;
    public bool CanLook = true;

    [Header("Camera")]
    public Camera FirstPerson;
    public Camera ThirdPerson;

    public Action Inventory;
    public Rigidbody _rigidbody;

    private bool isRiding;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
        if (running)
            Running();
    }

    private void LateUpdate()
    {
        if (CanLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= (MoveSpeed + EquipItemSpeed + UseConsumableItemSpeed);
        dir.y = _rigidbody.velocity.y;

        if (dir != Vector3.zero)
        {
            _rigidbody.velocity = dir;
            beforeDirection = dir;
        }

        else
        {
            if (dir != beforeDirection)
            {
                _rigidbody.velocity = dir;
                beforeDirection = dir;
            }
        }
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * LookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, MinXLook, MaxXLook);
        CameraContrainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * LookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnCameraChange(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (FirstPerson.isActiveAndEnabled)
            {
                FirstPerson.gameObject.SetActive(false);
                ThirdPerson.gameObject.SetActive(true);
            }
            else
            {
                FirstPerson.gameObject.SetActive(true);
                ThirdPerson.gameObject.SetActive(false);
            }
            CanLook = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGorunded() && !jumping)
        {
            if (PlayerManager.Instance.Player.condition.UseStamina(useJumpStamina))
            {
                _rigidbody.AddForce(Vector2.up * (JumpPower + EquipItemJump + UseConsumableJump), ForceMode.Impulse);
                Invoke("OnCanJump", jumpRate);
            }
        }
    }

    private void OnCanJump()
    {
        jumping = false;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            running = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            running = false;
        }
        
    }

    private void Running()
    {
        if (PlayerManager.Instance.Player.condition.UseStamina(userunStamina))
            _rigidbody.AddForce(transform.forward * runPower, ForceMode.Impulse);
        else
            running = false;
    }

    private bool IsGorunded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.5f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.5f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.5f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.5f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1.3f, GroundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Inventory?.Invoke();
            ToggleCursor();
        }
    }

    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        CanLook = !toggle;
    }
}
