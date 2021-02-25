using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteeredCohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior
{

    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        //add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector3)item.position;
        }
        //calculates median
        cohesionMove /= filteredContext.Count;

        //create offset from agent position
        cohesionMove -= (Vector3)agent.transform.position;
        if (float.IsNaN(currentVelocity.x) || float.IsNaN(currentVelocity.y) || float.IsNaN(currentVelocity.z)) { currentVelocity = Vector3.zero; } //this line is added to prevent the SmoothDamp from returning a NaN vector
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }
}
