using System.Collections.Generic;
using UnityEngine;

public class SpecialPoint : MonoBehaviour
{
    public List<GameObject> allowedCars = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        CarMovement car = other.GetComponentInParent<CarMovement>();
        if (car != null && allowedCars.Contains(other.gameObject))
        {
            car.TriggerSpecialPoint(transform);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1f);
    }
}
