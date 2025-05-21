using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    private Vector2 movementDirection;

    [Header("Look")]
    [SerializeField] private Transform CameraContainer;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    private float camXRot;
    private Vector2 mouseDelta;

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
        Movement();
    }

    private void LateUpdate()
    {
        Look();
    }

    void Movement()
    {
        Vector3 dir = transform.forward * movementDirection.y + transform.right * movementDirection.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void Look()
    {
        camXRot += mouseDelta.y * lookSensitivity;
        camXRot = Mathf.Clamp(camXRot, minXLook, maxXLook);
        CameraContainer.localEulerAngles = new Vector3(-camXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            movementDirection = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            movementDirection = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && OnGround())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool OnGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f + transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f + transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f + transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f + transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))
            {
                return true;
            }
        }

        return false;
    }
}
