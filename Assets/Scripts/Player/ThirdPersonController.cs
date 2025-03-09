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

        chController.Move(move.normalized * Time.deltaTime * movementSpeed);

    }
}