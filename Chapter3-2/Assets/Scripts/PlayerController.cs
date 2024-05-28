using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float JumpPower;
    private Vector2 curMovementInput;
    public LayerMask GroundLayerMask;

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

    private Rigidbody _rigidbody;

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
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= MoveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
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
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGorunded())
        {
            _rigidbody.AddForce(Vector2.up * JumpPower, ForceMode.Impulse);
        }
    }

    private bool IsGorunded()
    {
        Debug.DrawRay(transform.position + (transform.forward * 0.5f) + (transform.up * 0.01f), Vector3.down, Color.red);
        Debug.DrawRay(transform.position + (-transform.forward * 0.5f) + (transform.up * 0.01f), Vector3.down, Color.red);
        Debug.DrawRay(transform.position + (transform.right * 0.5f) + (transform.up * 0.01f), Vector3.down, Color.red);
        Debug.DrawRay(transform.position + (-transform.right * 0.5f) + (transform.up * 0.01f), Vector3.down, Color.red);

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
}
