using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] int AnimCount;
    [SerializeField] float TimeToSwitch = 5f;
    [SerializeField] Camera PlayerCamera;
    [SerializeField] GameObject car;
    [SerializeField] GameObject player;
    private static SwitchCamera sc;
    public static Animator CameraAnimator;
    private int random;
    List<int> ints = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        if(sc == null)
        {
            sc = this;
        }else {
            Destroy(this);
            Debug.LogError("There is another SwitchCamera");
            Debug.Break();
        }
        //CarCamera.gameObject.SetActive(false);
        CameraAnimator = PlayerCamera.GetComponent<Animator>();
        CameraAnimator.speed = 0f;
        ChangeAnim();
    }
    public void StartGame()
    {
        if (random == 7) ChangeAnim();
        StopAllCoroutines();
        PlayerCamera.gameObject.transform.SetParent(player.transform, false);
        CameraAnimator.Play("camera" + random);
        CameraAnimator.speed = 1f;
    }
    public static void ChangeAnimation()
    {
        sc.ChangeAnim();
        CameraAnimator.speed = 0f;
    }
    private void ChangeAnim(bool notSeven = false)
    {
        UIController.GetNoise();
        if (notSeven)
        {
            random = Random.Range(0, 7);
        }
        else
        {
            random = GetInt();
        }
        if (random == 7)
        {
            PlayerCamera.gameObject.transform.SetParent(car.transform, false);
            CameraAnimator.Play("oncar");
        }
        else
        {
            PlayerCamera.gameObject.transform.SetParent(player.transform, false);
            CameraAnimator.Play("camera" + random);
        }
        StartCoroutine(GameManager.GetInstance.DelayedAction(TimeToSwitch, () =>
        {
            ChangeAnim();
        }));
    }
    public void GoToHTP()
    {
        while (random == 7) ChangeAnim();
        StopAllCoroutines();
        CameraAnimator.Play("htp" + random);
        CameraAnimator.speed = 1f;
    }
    public void GoToBoard()
    {
        while(random == 7)  ChangeAnim();
        StopAllCoroutines();
        CameraAnimator.Play("board" + random);
        CameraAnimator.speed = 1f;
    }
    public void BackFromBoard()
    {
        CameraAnimator.speed = 0f;
        ChangeAnim();
    }
    private int GetInt()
    {
        if(ints.Count == 0)
        {
            ints = CreateShuffledList(AnimCount);
        }
        if(random != ints[0])
        {
            random = ints[0];
            ints.RemoveAt(0);
        }
        else
        {
            ints.Clear();
            GetInt();
        }
        return random;
    }
    List<int> CreateShuffledList(int maxInt)
    {
        List<int> resultList = new List<int>();

        // Заполняем список от 0 до maxInt
        for (int i = 1; i <= maxInt; i++)
        {
            resultList.Add(i);
        }

        // Перемешиваем список
        ShuffleList(resultList);

        return resultList;
    }

    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rng = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
