using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraControlActions cameraActions;
    private InputAction movement;
    private Transform cameraTransform;

    //[BoxGroup("Horizontal Translation")]
    [SerializeField]
    private float maxSpeed = 5f;
    private float speed;
    //[BoxGroup("Horizontal Translation")]
    [SerializeField]
    private float acceleration = 10f;
    //[BoxGroup("Horizontal Translation")]
    [SerializeField]
    private float damping = 15f;

    //[BoxGroup("Vertical Translation")]
    [SerializeField]
    private float stepSize = 2f;
    //[BoxGroup("Vertical Translation")]
    [SerializeField]
    private float zoomDampening = 7.5f;
    //[BoxGroup("Vertical Translation")]
    [SerializeField]
    private float minHeight = 5f;
    //[BoxGroup("Vertical Translation")]
    [SerializeField]
    private float maxHeight = 50f;
    //[BoxGroup("Vertical Translation")]
    [SerializeField]
    private float zoomSpeed = 2f;

    //[BoxGroup("Rotation")]
    [SerializeField]
    private float maxRotationSpeed = 1f;

    //[BoxGroup("Edge Movement")]
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.05f;

    //value set in various functions 
    //used to update the position of the camera base object.
    private Vector3 targetPosition;

    private float zoomHeight;

    //used to track and maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    //tracks where the dragging action started
    Vector3 startDrag;

    private void Awake()
    {
        cameraActions = new CameraControlActions();
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(transform);

        lastPosition = transform.position;
        movement = cameraActions.Camera.Movement;
        cameraActions.Camera.RotateCamera.performed += RotateCamera;
        cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
        cameraActions.Camera.Enable();
    }

    private void OnDisable()
    {
        cameraActions.Disable();
        cameraActions.Camera.RotateCamera.performed -= RotateCamera;
        cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
    }

    private void Update()
    {
        GetKeyboardMovement();
        CheckMouseAtScreenEdge();

        DragCamera();
        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();
    }

    void UpdateVelocity()
    {
        horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = transform.position;
    }

    void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight() + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
        {
            targetPosition += inputValue;
        }
    }

    Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0;

        return right;
    }

    Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;

        return forward;
    }

    void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }


    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed) return;

        float value = inputValue.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);

    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if(Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;

            if (zoomHeight < minHeight) zoomHeight = minHeight;
            else if (zoomHeight > maxHeight) zoomHeight = maxHeight;
        }
    }

    void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);

        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);

        cameraTransform.LookAt(transform);
    }

    void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
        {
            moveDirection += -GetCameraRight();
        }
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        if(mousePosition.y < edgeTolerance * Screen.height)
        {
            moveDirection += -GetCameraForward();
        }
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
        {
            moveDirection += GetCameraForward();
        }

        targetPosition += moveDirection;
    }

    void DragCamera()
    {
        if (!Mouse.current.rightButton.isPressed) return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(plane.Raycast(ray, out float distance))
        {
            if(Mouse.current.rightButton.wasPressedThisFrame)
            {
                startDrag = ray.GetPoint(distance);
            }
            else
            {
                targetPosition += startDrag - ray.GetPoint(distance);
            }
        }
    }
}
