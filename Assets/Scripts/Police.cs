
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Police : NPC
{
    private Transform player;
    private bool is_dancing = false;
    [SerializeField] private Collider body_collider;
    [SerializeField] private float follow_speed = 7.5f;
    [SerializeField] private GameObject notice_text;
    void Start()
    {
        player = GameManager.GetPlayerTransform;
        current_state = NormalState;
    }

    void Update() {
        current_state.Invoke();        
    }

    private void FollowPlayer() {
        navmesh_agent.destination = player.position;
    }

    public override void IsPlayerNearby(bool is_nearby) {
        is_player_nearby = is_nearby;
        CheckStates();
    }

    private void CheckStates() {
        if(is_dancing) {
            return;
        }
        if(is_player_nearby) {
            if(delayed_check_cor != null) {
                Debug.Log("stopping corouting");
                StopCoroutine(delayed_check_cor);
            }
            if(current_state == FollowPlayer) return;

            AudioManager.PlaySound("beep");
            notice_text.SetActive(true);

            StartCoroutine(GameManager.GetInstance.DelayedAction(2f, ()=> {
                notice_text.SetActive(false);
            }));
            

            
            animator.SetBool("following", true);

            navmesh_agent.speed = follow_speed;
            current_state = FollowPlayer;

        }else {
            Debug.Log("starting cor");
            delayed_check_cor = StartCoroutine(GameManager.GetInstance.DelayedAction(state_change_cooldown, () => {
                Debug.Log("doing corouting");
                animator.SetBool("following", false);
                animator.SetBool("dance", false);
                is_dancing = false;

                navmesh_agent.isStopped = false;
                navmesh_agent.speed = normal_speed;
                SetNewDestinationNormal();
                current_state = NormalState;
            }));
            
        } 
    }

    private void Dancing() {}

    public void Dance() {
        if(is_dancing) return;

        current_state = Dancing;

        is_player_nearby = false;        

        is_dancing = true;
        animator.SetTrigger("dance");
        navmesh_agent.isStopped = true;
        body_collider.enabled = false;

        StartCoroutine(GameManager.GetInstance.DelayedAction(4.46f, ()=> {
            animator.SetBool("following", false);
            is_dancing = false;
            navmesh_agent.isStopped = false;
            body_collider.enabled = true;
            current_state = NormalState;
            navmesh_agent.speed = normal_speed;
            SetNewDestinationNormal();
        }));
    }
}
