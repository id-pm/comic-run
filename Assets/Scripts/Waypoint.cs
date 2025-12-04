using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public LayerMask waypointLayer;
    void Awake()
    {
        FindNeighbors();
    }
    public List<Transform> Neighbors = new();
    void FindNeighbors()
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 5000f, waypointLayer))
            {
                Debug.DrawRay(transform.position, direction * 5000f, Color.red, 2f);
                Waypoint waypoint = hit.collider.GetComponent<Waypoint>();
                if (waypoint != null)
                {
                    Neighbors.Add(waypoint.transform);
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1f);
        if (Neighbors != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform waypoint in Neighbors)
            {
                Gizmos.DrawLine(transform.position, waypoint.position);
            }
        }
    }
}
