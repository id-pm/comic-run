using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackCollider : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private ParticleSystem gas;
    [SerializeField] private int max_gas = 10, gas_on_restore = 3;
    [SerializeField] Slider gas_slider;
    private int current_gas;

    private void Start() {
        ResetGas();
    } 
    private void OnTriggerEnter(Collider other) { 
        
        if(other.TryGetComponent(out Citizen citizen)) {
            if(current_gas <= 0) {
                current_gas = 0;

                // сказати гравцю про відсутність газу

                return;
            }
            if(citizen.IsLaughing) return;
            AudioManager.PlaySound("spray");
            player.Gas();
            gas.Clear();
            gas.Stop();
            gas.Play();
            // make citizen laugh
            citizen.MakeLaugh(); 
            gas_slider.value = --current_gas;
        }else {
            
        }
    }
    
    public bool RestoreGas() {
        if(current_gas >= max_gas) {
            return false;
        }
        if(current_gas + gas_on_restore > max_gas) {
            current_gas = max_gas;
        }else {
            current_gas += gas_on_restore;
        }
        gas_slider.value = current_gas;
        return true;
    }

    public void ResetGas() {
        gas_slider.value = gas_slider.maxValue = current_gas = max_gas;
    }
}
