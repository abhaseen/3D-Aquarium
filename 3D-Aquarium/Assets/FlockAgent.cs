using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensures we have a collider attached to the object
[RequireComponent(typeof(SphereCollider))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    SphereCollider agentCollider; //collider object
    public SphereCollider AgentCollider { get { return agentCollider; } } //public accessor - access without assignment

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<SphereCollider>(); //this is the only time it assigns it at the very beginning
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    //movement function
    public void Move(Vector3 velocity) //velocity in this context is the heading
    {
        transform.forward = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime; //casting to vector3 removes ambiguity
    }
}
