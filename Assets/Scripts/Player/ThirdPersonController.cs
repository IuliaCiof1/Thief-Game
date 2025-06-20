using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    CharacterController chController;
    [SerializeField] float movementSpeed = 2;
    [SerializeField] float rotationSpeed = 2;
    GameObject playerModel;
    [SerializeField] Animator animator;
   [SerializeField] PlayerActions playerActions;


    public Transform currentPlatform;
    public Vector3 lastPlatformPosition;
    public Quaternion lastPlatformRotation;

    public bool inTram { get; set; }

    float gravity = 9.8f;
    private float verticalVelocity = 0;
    public bool stopMovement { get; set; }

    void Start()
    {
        chController = GetComponent<CharacterController>();
        playerModel = animator.gameObject;
    }

    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");
        float zDir = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(xDir, 0, zDir);

        
        Vector3 velocity;
        velocity = move.normalized * movementSpeed;

        if (stopMovement)
        {
            velocity = Vector2.zero;
        }

        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move.normalized);

            //Rotate smoothly to this target:
            playerModel.transform.rotation = 
                Quaternion.Slerp(animator.gameObject.transform.rotation, targetRotation, rotationSpeed);
            animator.SetTrigger("RunForward");
        }
        else if (!playerActions.actionInProgress) { animator.SetTrigger("Idle");}

        if (chController.isGrounded)
        {
            verticalVelocity = 0; // grounded character has vertical velocity 0   
        }
        else
        {
            // apply gravity acceleration to vertical speed:
            verticalVelocity -= gravity * Time.deltaTime;
            velocity.y = verticalVelocity;
        }
      
        //Handle tram player movement
        Vector3 platformDelta = Vector3.zero;
        Quaternion rotationDelta = Quaternion.identity;


        if (inTram)
        {
            // Calculate delta movement
            platformDelta = currentPlatform.position - lastPlatformPosition;

            // Calculate delta rotation
            rotationDelta = currentPlatform.rotation * Quaternion.Inverse(lastPlatformRotation);

            // Rotate player's position around platform pivot
            Vector3 localOffset = transform.position - currentPlatform.position;
            Vector3 rotatedOffset = rotationDelta * localOffset;
            Vector3 newPosition = currentPlatform.position + rotatedOffset;

            // Apply rotation
            transform.rotation = rotationDelta * transform.rotation;

            // Apply movement
            Vector3 movement = newPosition - transform.position;
            chController.Move(movement + velocity * Time.deltaTime + platformDelta);

            // Update for next frame
            lastPlatformPosition = currentPlatform.position;
            lastPlatformRotation = currentPlatform.rotation;
        }
        else
            chController.Move(Time.deltaTime * velocity);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Detect if player is on a moving platform
        if (hit.collider.TryGetComponent(out Tram tram))
        {
            currentPlatform = hit.collider.transform;
            lastPlatformPosition = hit.collider.transform.position;
            lastPlatformRotation = hit.collider.transform.rotation;        }
    }

}