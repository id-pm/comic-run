using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject ButtonBack;
    [SerializeField] GameObject NoiseImage;
    [SerializeField] GameObject ScoreTab;
    [SerializeField] GameObject SaveScoreTab;
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject Smile;
    [SerializeField] TMP_InputField name_input;
    [SerializeField] SettingsMenu settings;
    private AudioSource NoiseSound;
    private static UIController ui;
    private Animator smileAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        smileAnimator = Smile.GetComponent<Animator>();
        if(ui == null) {
            ui = this;
        }else {
            Destroy(this);
            Debug.LogError("There is another UIController");
            Debug.Break();
        }
        //ui = FindObjectOfType<UIController>();
        NoiseSound = NoiseImage.GetComponent<AudioSource>();
        SaveScoreTab.SetActive(false);
        ScoreTab.SetActive(false);
        NoiseImage.SetActive(false);
        ButtonBack.SetActive(false);

        //SettingsPanel.SetActive(false);

        
    }
    public void LetsSmile()
    {
        if(smileAnimator != null)
        {
            smileAnimator.Play("smile");
        }
    }
    public static void ShowPlayerNameInput(string saved_name = "")
    {
        ui.SaveScoreTab.SetActive(true);
        ui.name_input.text = saved_name;
        ui.smileAnimator.Play("notsmile");
    }
    public static void SaveSCore()
    {
        //Debug.Log($"score: {GameManager.GetScore} name: {ui.name_input.text}");
        ScoresFileController.SaveScore(GameManager.GetScore, ui.name_input.text);
        GameManager.SetPlayerName(ui.name_input.text); // зберігаю ім'я на потім
        GameManager.ResetScore();
        GameManager.UpdateBillboard();

        // повернутися в меню
    }
    public void SetActiveButtonBack(bool state)
    {
        ButtonBack.SetActive(state);
        MainMenu.SetActive(!state);
    }
    public void SetActiveMenu(bool state)
    {
        MainMenu.SetActive(state);
    }
    public static void SetActiveMenuFromMenu()
    {
        ui.MainMenu.SetActive(true);
        SwitchCamera.ChangeAnimation();
        ui.ScoreTab.SetActive(false);
    }
    public static void SetScoreTabActive(bool state)
    {
        ui.ScoreTab.SetActive(state);
    }
    private void _getNoise()
    {
        NoiseImage.SetActive(true);
        NoiseSound.Play();
        StartCoroutine(GameManager.GetInstance.DelayedAction(0.5f, () =>
        {
            NoiseImage.SetActive(false);
        }));
        
    }
    public static void GetNoise()
    {
        ui._getNoise();
    }

    public void ExitGame() {
        Application.Quit();
    }
    public void OpenSettings()
    {
        PauseMenu.isActive = true;
        SettingsMenu.isGeneralMenu = false;
        SettingsPanel.SetActive(true);
        SettingsMenu.isActive = true;
    }
    public static void CloseSettings(bool isGeneral)
    {
        ui.SettingsPanel.SetActive(false);
        ui.MainMenu.SetActive(isGeneral);
        ui.PausePanel.SetActive(!isGeneral);
        SettingsMenu.isActive = false;
    }
}
