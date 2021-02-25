using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior : ScriptableObject
{
    //Calculates where the flock agent should be in context to other colliders in the scene and the general flock
    public abstract Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);
}
