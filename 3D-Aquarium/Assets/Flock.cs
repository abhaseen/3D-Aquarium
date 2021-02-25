using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //handling prefabs and setting up initial variables
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior; //takes care of which behaviour to use

    //setting how many agents will be spawned
    [Range(10, 500)]
    public int startingCount = 250;
    //setting the density
    const float AgentDensity = 0.08f;

    //flock behavior
    //drive factor
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    //radius for neghbors or collision obstacles
    [Range(1f, 20f)]
    public float neighborRadius = 1.5f;

    //avoidance radius
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    //these variables will save mathematical computation time by storing the squares of values needed
    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        //instantiating the flock
        for (int i = 0; i < startingCount; i++) {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //iterate agents in list of agents
        foreach (FlockAgent agent in agents) {
            List<Transform> context = GetNearbyObjects(agent);
            //change color based on how many neighbors - FOR DEMO ONLY
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector3 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent) {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        //iterate through the colliders
        foreach (Collider c in contextColliders) {
            //take transform collider, add to list
            if (c != agent.AgentCollider) {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
