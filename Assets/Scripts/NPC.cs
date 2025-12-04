using System;
using UnityEngine;
using UnityEngine.AI;
public class NPC : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected NavMeshAgent navmesh_agent;
    [SerializeField] protected float range; //radius of sphere
    [SerializeField] protected Transform centrePoint; //centre of the area the agent wants to move around in
    protected Action current_state;    
    protected bool is_player_nearby = false;
    protected Coroutine delayed_check_cor = null;
    [SerializeField] protected float normal_speed = 3f, state_change_cooldown = 5f;

    protected void SetNewDestinationNormal() {
        if (RandomPoint(centrePoint.position, range, out Vector3 point)) { //pass in our centre point and radius of area            
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
            navmesh_agent.SetDestination(point);
        }
    }    

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, 1 << NavMesh.GetAreaFromName("Walkable"))) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    protected void NormalState() {
        if(navmesh_agent.remainingDistance <= navmesh_agent.stoppingDistance) { //done with path 
            SetNewDestinationNormal();
        }
    }

    public virtual void IsPlayerNearby(bool is_nearby) {

    }
    
}
