using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CharacterController))]
public class FireTeam : MonoBehaviour
{
    [SerializeField] private InputAction mouseClickAction;
    [SerializeField] float playerSpeed = 10f;

    private Camera mainCamera;
    private Coroutine coroutine;
    private Vector3 targetPosition;

    private CharacterController characterController;
    private Rigidbody rb;

    private void Awake()
    {
        mainCamera = Camera.main;
        //characterController = GetComponent<CharacterController>();
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

        if(Physics.Raycast(ray: ray, hitInfo: out var hit) && hit.collider)
        {
            if (coroutine != null) StopCoroutine(coroutine);
            StartCoroutine(PlayerMoveTowards(hit.point));
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;

        while(Vector3.Distance(transform.position, target) > 1f)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);

            // transform.position = destination;
            Vector3 direction = target - transform.position;
            Vector3 movement = direction.normalized * playerSpeed * Time.deltaTime;
            //characterController.Move(movement);

            rb.velocity = direction.normalized * playerSpeed;

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 1);
    }
} 
