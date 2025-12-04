using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public void ShowGameOverPanel() {
        // game over
        UIController.ShowPlayerNameInput(GameManager.GetPlayerName);
    }
    public void HeadacheSound() {
        AudioManager.PlaySound("headache");
    }
}
