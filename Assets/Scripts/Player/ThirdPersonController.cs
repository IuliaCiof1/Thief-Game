using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    CharacterController chController;
    [SerializeField] float playerSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        chController = GetComponent<CharacterController>();   
    }

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");
        float zDir = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(xDir, 0, zDir);

        chController.Move(move.normalized * Time.deltaTime * playerSpeed);

    }
}
