using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerOnCollision : MonoBehaviour
{
    [SerializeField] private float pill_duration = 5f, speed_change = 3, rot_speed_change=100;
    [SerializeField] PlayerController player;
    [SerializeField] PlayerAttackCollider attack_collider;
    [SerializeField] ParticleSystem particle;
    [SerializeField] private float invulnerable_time = 3f;
    
    private bool can_be_attacked = true;
   
    [SerializeField] private LayerMask pill, obstacle, police, gas;

    public void ResetInvulnerability() {
        can_be_attacked = true;
    }

    private void OnCollisionEnter(Collision other) {      
        
        if(pill == (pill | (1 << other.gameObject.layer))) {
            player.ChangeSpeed(speed_change, rot_speed_change);
            StartCoroutine(GameManager.GetInstance.DelayedAction(pill_duration, () => {
                player.ChangeSpeed(-speed_change, -rot_speed_change);                
            }));
            // повертаємо пігулку в пул
            PillPool.GetInstance.ReturnPill(other.gameObject);
            AudioManager.PlaySound("taken");
        }

        if(gas == (gas | (1 << other.gameObject.layer))) {
            
            if(attack_collider.RestoreGas()) {
                PillPool.GetInstance.ReturnGas(other.gameObject);
                AudioManager.PlaySound("taken");
            }
        }

        if(police == (police | (1 << other.gameObject.layer))) {
            other.gameObject.GetComponent<Police>().Dance();

            if(can_be_attacked) {

                // make player invulnerable
                can_be_attacked = false;

                // play particle
                particle.Play();

                // decrement hp
                if(player.TakeDamage()) { // if still alive

                    // boost speed
                    player.ChangeSpeed(2, 0);

                    // return stats back after delay
                    StartCoroutine(GameManager.GetInstance.DelayedAction(invulnerable_time, ()=> {
                        can_be_attacked = true;
                        player.ChangeSpeed(-2, 0);
                    }));
                }
                

            }

            
        }
    }

    
}
