using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject PausePanel;
    private bool isRestart = true, isPause = false;
    public static bool isActive = false;
    
    void Start()
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        PausePanel.SetActive(false);
    }
    public void OpenPanel(bool p)
    {
        if (p)
        {
            animator.Play("openMenu");
        }
        else
        {
            animator.Play("closeMenu");
        }

    }
    public void IsRestart(bool restartValue)
    {
        isRestart = restartValue;
    }
    public void Resume()
    {
        isPause = false;
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }
    public void Confirm()
    {
        animator.speed = 1f;
        Resume();
        GameManager.KillPLayer();
        GameManager.ResetPlayerTransform();
        isPause = false;
        if (isRestart)
        {
            GameManager.StartPlayer();
        }
        else
        {
            UIController.SetActiveMenuFromMenu();
        }

    }
    private void Update()
    {
        if (!isActive || SettingsMenu.isActive) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
            if(isPause)
            {
                Time.timeScale = 0f;
                PausePanel.SetActive(true);
            }
            else
            {
                Resume();
            }
        }
    }
}
