using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CharacterController))]
public class ClickToMove : MonoBehaviour
{
    [SerializeField] private InputAction mouseClickAction;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] float playerSpeed = 10f;

    private Camera mainCamera;
    private Coroutine coroutine;
    private Vector3 targetPosition;

    private int groundLayer;

    private CharacterController characterController;
    private Rigidbody rb;

    private void Awake()
    {
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void OnEnable()
    {
        mouseClickAction.Enable();
        mouseClickAction.performed += Move;
    }

    private void OnDisable()
    {
        mouseClickAction.performed -= Move;
        mouseClickAction.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray: ray, hitInfo: out var hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer) == 0)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
            targetPosition = hit.point;
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);

            // transform.position = destination;
            Vector3 direction = target - transform.position;
            Vector3 movement = direction.normalized * playerSpeed * Time.deltaTime;
            //  characterController.Move(movement);

            rb.velocity = direction.normalized * playerSpeed;
            Debug.Log(direction);
            //rb.AddForce(direction * playerSpeed);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 1);
    }
}
