using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PocketItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Transform hand;
    //Rigidbody rb;
    //Transform initialParent;
    //[SerializeField] float addedForce = 500;
    //bool isFalling = false;
    //Vector3 velocity = Vector3.zero;
    //[SerializeField] float gravity = -9.8f;
    //[SerializeField] float slideFriction = 0.1f; // Controls sliding smoothness

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    print("begin drag");
    //    transform.SetParent(hand);
    //    transform.SetAsFirstSibling();
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    isFalling = false;
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    transform.SetParent(initialParent);
    //    isFalling = true;
    //}

    //void Start()
    //{
    //    hand = FindObjectOfType<Hand>().transform;
    //    rb = GetComponent<Rigidbody>();
    //    initialParent = transform.parent;
    //}

    //void Update()
    //{
    //    if (isFalling)
    //    {
    //        // Apply gravity
    //        velocity += new Vector3(0, gravity * Time.deltaTime, 0);
    //        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    print("Collided with: " + collision.gameObject.name);

    //    Vector3 normal = collision.GetContact(0).normal;

    //    // If it's a ground collision, stop falling
    //    if (Vector3.Dot(normal, Vector3.up) > 0.5f)
    //    {
    //        isFalling = false;
    //        velocity.y = 0;
    //    }
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    Vector3 normal = collision.GetContact(0).normal;


    //    // Handle sliding along walls
    //    if (Mathf.Abs(Vector3.Dot(normal, Vector3.right)) > 0.5f)
    //    {
    //        print("Sliding along wall");
    //        velocity = Vector3.Reflect(velocity, normal) * (1 - slideFriction); // Redirect movement
    //    }
    //    else if (Mathf.Abs(Vector3.Dot(normal, Vector3.forward)) > 0.5f)
    //    {
    //        print("Sliding along a vertical wall");
    //        velocity = Vector3.Reflect(velocity, normal) * (1 - slideFriction);
    //    }

    //    // If velocity is too low, apply a tiny push to keep it moving
    //    if (velocity.magnitude < 0.1f)
    //    {
    //        velocity += normal * 0.05f;
    //    }

    //    rb.MovePosition(rb.position + velocity * Time.deltaTime);
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    print("Collision exited with: " + collision.gameObject.name);

    //    // Only restart falling if leaving the ground, not a wall
    //    if (Vector3.Dot(collision.GetContact(0).normal, Vector3.up) > 0.5f)
    //    {
    //        isFalling = true;
    //    }
    //}

    Transform hand;
    Rigidbody rb;
    Transform initialParent;
    [SerializeField] float addedForce = 500;
    bool isFalling = false;
    Vector3 velocity = Vector3.zero;
    [SerializeField] float gravity = -9.8f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("begin drag");
        transform.SetParent(hand);
        transform.SetAsFirstSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {

        rb.isKinematic = true;
        isFalling = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rb.isKinematic = false;
        transform.SetParent(initialParent);
        isFalling = true;
        //rb.AddForce(new Vector3(0,-addedForce,0), ForceMode.VelocityChange);
    }

    // Start is called before the first frame update
    void Start()
    {
        hand = FindObjectOfType<Hand>().transform;
        rb = GetComponent<Rigidbody>();
        initialParent = transform.parent;

    }

    // Update is called once per frame
    void Update()
    {


        //if (isFalling)
        //{
        //    // Simulate gravity manually
        //    velocity += new Vector3(0, gravity * Time.deltaTime, 0);
        //    //transform.position += velocity * Time.deltaTime;
        //    rb.MovePosition(rb.position + velocity * Time.deltaTime);
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //print("collide");
    //    //isFalling = false; // Stop falling when it hits something
    //    //velocity = Vector3.zero;


    //    velocity.y = 0;  // Stop vertical falling
    //    velocity.x -= 500f; // Reduce X movement slowly


    //    // If the velocity is very small, stop completely
    //    if (Mathf.Abs(velocity.x) < 0.1f && Mathf.Abs(velocity.z) < 0.1f)
    //    {
    //        isFalling = false;
    //        velocity = Vector3.zero;
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    print("collide");
    //    isFalling = false; // Stop falling when it hits something
    //    //velocity = Vector3.zero;
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    print("collide stay");
    //    Vector3 normal = collision.contacts[0].normal;

    //    if (Mathf.Abs(Vector3.Dot(normal, Vector3.up)) > 0.5f) // If touching a wall
    //    {
    //        print("wall");
    //        velocity.x = 10;
    //        transform.position += normal * 10f;
    //        isFalling = true;// Push slightly away from the wall
    //    }
    //    if (Mathf.Abs(Vector3.Dot(normal, Vector3.right)) > 0.5f) // If touching a wall
    //    {
    //        print("wall");
    //        transform.position += normal * 50f; // Push slightly away from the wall
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    print("collide");
    //    isFalling = false; // Stop falling when it hits something
    //    //velocity = Vector3.zero;
    //}
}
//    //private void OnCollisionEnter(Collision collision)
//    //{
//    //    rb.AddForce(new Vector3(0, -addedForce, 0), ForceMode.VelocityChange);
//    //}
//}
