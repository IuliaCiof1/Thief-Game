using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandParent : MonoBehaviour
{
    [SerializeField] StealTimer stealTimer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PocketWall"))
        {
            stealTimer.Value -= stealTimer.MaxValue/ 3;
            print("You touched the wall");
        }
    }
}
