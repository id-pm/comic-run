using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private LayerMask police, car, human, player;
    [SerializeField] private Transform currentWaypoint;
    [SerializeField] private Transform previousWaypoint;
    [SerializeField] private Transform nextWaypoint;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float shortenDistance = 5f;
    [SerializeField] private float laneOffset = 2f;
    [SerializeField] private float stopDuration = 3f;
    public bool dontMove = false;
    private float rotationSpeed = 1.5f, currentSpeed;
    private List<Transform> waypoints = new List<Transform>();
    private Vector3 currentPosition, rightDirection;
    private float turnAngleThreshold = 10;
    private bool isStopped = false, movingToSpecialPoint = false;
    void Start()
    {
        if (currentWaypoint != null)
        {
            previousWaypoint = currentWaypoint;
            waypoints = new List<Transform>(currentWaypoint.GetComponent<Waypoint>().Neighbors);
            waypoints.Remove(previousWaypoint);
            nextWaypoint = waypoints[Random.Range(0, waypoints.Count)];
            rightDirection = Vector3.Cross(Vector3.up, currentWaypoint.position - transform.position).normalized;
            currentPosition = currentWaypoint.position + rightDirection * laneOffset;
        }
        speed += Random.Range(-2f, 2f);
        currentSpeed = speed;
    }

    void Update()
    {
        if (dontMove || isStopped) return;
        if (currentWaypoint != null)
        {
            Vector3 direction = currentPosition - transform.position;
            direction.y = 0;
            float angle = Vector3.Angle(transform.forward, direction);
            if (angle > turnAngleThreshold)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 3f, Time.deltaTime * speed);
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 2f);
            }
            transform.Translate(currentSpeed * Time.deltaTime * Vector3.forward);
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            if (Vector3.Distance(transform.position, currentPosition) < 2f)
            {
                if (movingToSpecialPoint)
                {
                    StartCoroutine(HandleSpecialPoint());
                }
                else
                {
                    ChooseNextWaypoint();
                }
            }
        }
    }

    void ChooseNextWaypoint()
    {
        if (currentWaypoint == null) return;
        if(nextWaypoint != null)
        {
            waypoints = new List<Transform>(nextWaypoint.GetComponent<Waypoint>().Neighbors);
            waypoints.Remove(currentWaypoint);
        }
        previousWaypoint = currentWaypoint;
        currentWaypoint = nextWaypoint;
        nextWaypoint = waypoints[Random.Range(0, waypoints.Count)];
        rightDirection = Vector3.Cross(Vector3.up, currentWaypoint.position - transform.position).normalized;
        currentPosition = currentWaypoint.position + rightDirection * laneOffset;
        Vector3 previousToCurrent = currentWaypoint.position - previousWaypoint.position;
        Vector3 currentToNext = nextWaypoint.position - currentWaypoint.position;
        float crossProduct = Vector3.Cross(previousToCurrent, currentToNext).y;
        if (crossProduct > 0)
        {
            Vector3 shortenVector = previousToCurrent.normalized * shortenDistance;
            currentPosition -= shortenVector;
        }
    }

    IEnumerator HandleSpecialPoint()
    {
        isStopped = true;
        movingToSpecialPoint = false;
        rightDirection = Vector3.Cross(Vector3.up, currentWaypoint.position - transform.position).normalized;
        currentPosition = currentWaypoint.position + rightDirection * laneOffset;
        yield return new WaitForSeconds(stopDuration);
        isStopped = false;
    }
    public void TriggerSpecialPoint(Transform specialPoint)
    {
        Debug.Log(specialPoint.position);
        movingToSpecialPoint = true;
        currentPosition = specialPoint.position;
    }
}
