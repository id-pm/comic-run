using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GetInstance => _i;
    private static GameManager _i;
    [SerializeField] private PlayerController player;
    [SerializeField] private ScoreBillboard billboard;
    
    [SerializeField] private TextMeshProUGUI score_text;  
    [SerializeField] private Transform start_player_pos;

    private int laughed = 0;
    private string player_name = string.Empty;
    
    private void Awake() {
        if(_i == null) {
            _i = this;
        }else {
            Destroy(this);
            Debug.LogError("There is another GameManager...");
            Debug.Break();
        }
        player.enabled = false;
        ResetPlayerTransform();
        
    }

    public IEnumerator DelayedAction(float delay, Action act) {
        yield return new WaitForSeconds(delay);
        act();
    }

    public static void StartPlayer() {
        _i.player.StandUp();
        _i.StartCoroutine(_i.DelayedAction(5.4f, ()=> {
            _i.player.enabled = true;
            _i.player.TurnColliderOn();
            PauseMenu.isActive = true;
        }));
        
    }

    public static void KillPLayer() {
        _i.player.Die(false);
    }

    public static void ResetPlayerTransform() {
        _i.player.transform.position = _i.start_player_pos.position;
        _i.player.transform.rotation = _i.start_player_pos.rotation;
        _i.player.ResetStats();
        ResetScore();
    }

    public static Transform GetPlayerTransform => _i.player.transform;

    public static void AddLaughed() {

        _i.laughed++;
        UpdateScore();
        
    }

    private static void UpdateScore() {
        _i.score_text.text = $"{_i.laughed}";
    }

    public static int GetScore => _i.laughed;
    public static void ResetScore() {
        _i.laughed = 0;
        UpdateScore();
    }
    public static void SetPlayerName(string p_n) {
        _i.player_name = p_n;
    }
    public static string GetPlayerName => _i.player_name;

    public static void UpdateBillboard() {
        _i.billboard.UpdateScores();
    }
}
