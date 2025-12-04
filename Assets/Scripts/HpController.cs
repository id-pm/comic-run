using UnityEngine;


public class HpController : MonoBehaviour
{
    [SerializeField] GameObject[] hearts;
    private static int CountHp;
    private static HpController hpController;
    private void Start()
    {
        if (hpController == null)
        {
            hpController = this;
        }
        else
        {
            Destroy(this);
            Debug.LogError("There is another UIController");
            Debug.Break();
        }
        CountHp = hearts.Length;
    }
    public void ReturnAll()
    {
        CountHp = hearts.Length;
        foreach (var item in hearts)
        {
            item.SetActive(true);
        }
    }
    public static void MinusOne()
    {
        if(CountHp > 0)
        {
            hpController.hearts[CountHp - 1].SetActive(false);
            CountHp--;
        }
    }
}
