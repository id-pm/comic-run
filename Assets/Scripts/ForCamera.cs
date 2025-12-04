using UnityEngine;

public class ForCamera : MonoBehaviour
{
    [SerializeField] GameObject HTPNavigate;
    [SerializeField] GameObject HTPGameObject;
    [SerializeField] GameObject ButtonLeft;
    [SerializeField] GameObject ButtonRight;
    [SerializeField] GameObject ButtonBack;
    private static int _slide = 1;
    private void Start()
    {
        HTPGameObject.SetActive(false);
        HTPNavigate.SetActive(false);
        ButtonLeft.SetActive(false);
    }
    public void ActivatePlayer()
    {
        GameManager.StartPlayer();
    }
    public void ActivateButtons()
    {
        ButtonBack.SetActive(false);
        HTPNavigate.SetActive(true);
    }
    public void BackToMenu()
    {
        HTPGameObject.SetActive(false);
        HTPNavigate.SetActive(false);
        ButtonLeft.SetActive(false);
        ButtonRight.SetActive(true);
        _slide = 1;
    }
    public void ActivateHTP()
    {
        HTPGameObject.SetActive(true);
    }
    public void PressButton(bool isRight)
    {
        if(isRight)
        {
            _slide++;
            SwitchCamera.CameraAnimator.Play("fly" + _slide);
        }
        else
        {
            _slide--;
            if (_slide == 2)
            {
                SwitchCamera.CameraAnimator.Play("fly32");
            }
            else if (_slide == 1)
            {
                SwitchCamera.CameraAnimator.Play("fly1");
            }
        }
        if(_slide == 1)
        {
            ButtonLeft.SetActive(false);
        }
        else if(_slide == 3)
        {
            ButtonRight.SetActive(false);
        }
        else
        {
            ButtonLeft.SetActive(true);
            ButtonRight.SetActive(true);
        }
    }
}
