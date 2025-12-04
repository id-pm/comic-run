using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    private int countTrigger = 0;
    public CarMovement carObj;
    [SerializeField] private LayerMask police, car, human, player;
    private bool needSkip = false, wait = false;

    private void OnTriggerEnter(Collider other)
    {
        if (police == (police | (1 << other.gameObject.layer)))
        {
            carObj.dontMove = true;
            countTrigger++;
        }
        else if (!needSkip && car == (car | (1 << other.gameObject.layer)))
        {
            bool thisCarTurning = Mathf.Abs(transform.rotation.eulerAngles.y % 90) > 1;
            bool otherCarTurning = Mathf.Abs(other.transform.rotation.eulerAngles.y % 90) > 1;

            if (!(!thisCarTurning && otherCarTurning))
            {
                carObj.dontMove = true;
                countTrigger++;
                StartCoroutine(WaitForSecondsThenMove(1f, 1.5f));
                other.gameObject.GetComponent<Car>().Skip();
            }
        }
        else if (human == (human | (1 << other.gameObject.layer)))
        {
            carObj.dontMove = true;
            countTrigger++;
        }
        else if (player == (player | (1 << other.gameObject.layer)))
        {
            carObj.dontMove = true;
            countTrigger++;
        }
        StartCoroutine(GameManager.GetInstance.DelayedAction(5, () =>
        {
            carObj.dontMove = false;
        }));
    }
    public void Skip()
    {
        StartCoroutine(HandleSpecialPoint());
    }
    System.Collections.IEnumerator HandleSpecialPoint()
    {
        needSkip = true;
        yield return new WaitForSeconds(0.5f);
        needSkip = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (police == (police | (1 << other.gameObject.layer)))
        {
            countTrigger--;
        }
        else if (car == (car | (1 << other.gameObject.layer)))
        {
            countTrigger--;
        }
        else if (human == (human | (1 << other.gameObject.layer)))
        {
            countTrigger--;
        }
        else if (player == (player | (1 << other.gameObject.layer)))
        {
            countTrigger--;
        }

        if (countTrigger == 0 && !wait)
        {
            carObj.dontMove = false;
        }
    }

    private System.Collections.IEnumerator WaitForSecondsThenMove(float minWaitTime, float maxWaitTime)
    {
        wait = true;
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        if (countTrigger == 0)
        {
            carObj.dontMove = false;
        }
        wait = false;
    }
}
