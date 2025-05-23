using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    CharacterController chController;
    [SerializeField] float movementSpeed = 2;
    [SerializeField] float rotationSpeed = 2;
    [SerializeField] Animator animator;
    [SerializeField] PlayerActions playerActions;


    public Transform currentPlatform;
    public Vector3 lastPlatformPosition;
    public Quaternion lastPlatformRotation;

    public bool inTram { get; set; }



float gravity = 9.8f;
private float vSpeed = 0; // current vertical velocity

    // Start is called before the first frame update
    void Start()
    {
        

        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;


        chController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float xDir = Input.GetAxis("Horizontal");
        float zDir = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(xDir, 0, zDir);

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move.normalized);

            //Rotate smoothly to this target:
            animator.gameObject.transform.rotation = Quaternion.Slerp(animator.gameObject.transform.rotation, targetRotation, rotationSpeed);

            animator.SetTrigger("RunForward");
           
        }
        else if (!playerActions.actionInProgress)
        {
            //print(playerActions.actionInProgress);
            animator.SetTrigger("Idle");
        }


        Vector3 platformDelta = Vector3.zero;
        Quaternion rotationDelta = Quaternion.identity;

        //Gravity
        Vector3 vel =  move.normalized * movementSpeed;
        if (chController.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...   
        }
  
        // apply gravity acceleration to vertical speed:
        vSpeed -= gravity * Time.deltaTime;
        vel.y = vSpeed;

        if (inTram)
        {
            //platformDelta = currentPlatform.position - lastPlatformPosition;
            //lastPlatformPosition = currentPlatform.position;
            //transform.rotation = currentPlatform.rotation;
            //chController.Move(move.normalized * Time.deltaTime * movementSpeed + platformDelta);

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
            chController.Move(movement + move.normalized * movementSpeed * Time.deltaTime + platformDelta);

            // Update for next frame
            lastPlatformPosition = currentPlatform.position;
            lastPlatformRotation = currentPlatform.rotation;

        }
        else
            chController.Move(Time.deltaTime  * vel);

        //Vector3 rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime*180, 0);
        // print(rotation);
        // Quaternion targetRotation = Quaternion.LookRotation(transform.up, new Vector3(transform.rotation.x, transform.rotation.y, xDir));
        // transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation, rotationSpeed);

        // chController.rot


        //Now we create a target rotation, by creating a direction vector: (This would be just be inputVector in this case).
        //Quaternion targetRotation = Quaternion.LookRotation(move.normalized);
        //print(targetRotation);
        ////Rotate smoothly to this target:
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);

        //chController.Move(move.normalized * Time.deltaTime * movementSpeed);

    }
    //private void OnCollisionEnter(Collision hit)
    //{
    //    print("collide with sth " + hit.gameObject.name);

    //    // Detect if player is on a moving platform
    //    if (hit.collider.TryGetComponent(out Tram tram))
    //    {
    //        print("collide with tram");
    //        currentPlatform = hit.collider.transform;
    //        lastPlatformPosition = hit.collider.transform.position;
    //    }
    //}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
     
        // Detect if player is on a moving platform
        if (hit.collider.TryGetComponent(out Tram tram))
        {
           
            currentPlatform = hit.collider.transform;
            lastPlatformPosition = hit.collider.transform.position;
        }
    }

}