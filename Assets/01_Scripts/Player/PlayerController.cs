using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;

    // ==== SuperJump용 추가 ====
    public float jumpBoost = 5f;
    private float defaultJumpPower;        // 기본 점프 힘 저장용
    private Coroutine jumpBoostRoutine;    // 코루틴 중복 방지용

    public LayerMask groundLayerMask;
    private AnimatorHandler animatorHandler;
    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    [HideInInspector]
    public Action inventory;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        defaultJumpPower = jumpPower;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animatorHandler = GetComponent<AnimatorHandler>();
    }

    void FixedUpdate()
    {
        Move();
        if (animatorHandler != null)
        {
            bool isMoving = curMovementInput.magnitude > 0.1f;
            animatorHandler.Move(isMoving);
        }
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }
    public float GetCurrentSpeed()
    {
        return _rigidbody.velocity.magnitude;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
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

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.7f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void ApplyJumpBoost(float multiplier, float duration)
    {
        // 이미 버프가 켜져 있으면 먼저 종료
        if (jumpBoostRoutine != null)
        {
            StopCoroutine(jumpBoostRoutine);
        }

        jumpBoostRoutine = StartCoroutine(JumpBoostCoroutine(multiplier, duration));
    }

    private IEnumerator JumpBoostCoroutine(float multiplier, float duration)
    {
        // 점프 힘을 배수만큼 증가
        jumpPower = defaultJumpPower * multiplier;

        // duration 동안 유지
        yield return new WaitForSeconds(duration);

        // 끝나면 원래 값으로 복구
        jumpPower = defaultJumpPower;
        jumpBoostRoutine = null;
    }
}