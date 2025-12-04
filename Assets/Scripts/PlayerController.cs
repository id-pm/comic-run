using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float rotation_speed;
    [SerializeField] private int max_hp = 5;
    [SerializeField] private Collider body_collider;
    [SerializeField] private GameObject triggers;
    [SerializeField] private CitizenRadar radar;
    [SerializeField] private PlayerOnCollision player_collision;
    [SerializeField] PlayerAttackCollider player_attack;
    private int current_hp;
    private int hash_speed, hash_gas;

    // Update is called once per frame
    void Start()
    {
        
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        hash_speed = Animator.StringToHash("speed");
        hash_gas = Animator.StringToHash("gas");
        current_hp = max_hp;
    }
    
    void Update()
    {
        // Handle player input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log(vertical);

        // Calculate movement and rotation
        //Vector3 moveDirection = new Vector3(horizontal, 0f, speedForward).normalized;
        //Vector3 movement = moveDirection * speed * Time.deltaTime;
        
        Quaternion rotation = Quaternion.Euler(0f, rotation_speed * horizontal  * Time.deltaTime, 0f);

        float res_speed = speed + vertical*speed/2;


        animator.SetFloat(hash_speed, res_speed, 0.2f, Time.deltaTime);

        // Apply movement and rotation to the Rigidbody
        //rb.MovePosition(rb.position + transform.forward * res_speed * Time.deltaTime);
        rb.velocity = transform.forward * res_speed;// * Time.deltaTime;
        rb.MoveRotation(rb.rotation * rotation);

        //transform.position += transform.forward * speed;
        //transform.rotation = transform.rotation * rotation; 
    }
    public void TurnColliderOn() {
        body_collider.enabled = true;
        triggers.SetActive(true);
    }
    public void StandUp() {
        animator.SetTrigger("stand_up");
    }

    public void ChangeSpeed(float speed_mod, float rot_speed_mod) {
        speed += speed_mod;
        rotation_speed += rot_speed_mod;
    }

    public void ResetStats() {
        Debug.Log("asd");
        ResetHP();
        player_attack.ResetGas();
        player_collision.ResetInvulnerability();
        animator.Rebind();
        
    }

    private void ResetHP() {
        current_hp = max_hp;
    }


    public bool TakeDamage() {
        HpController.MinusOne();
        int r = Random.Range(0, 2);
        if(r == 0) {
            AudioManager.PlaySound("punch1");
        }else {
            AudioManager.PlaySound("punch2");
        }
        if(--current_hp <= 0) {
           
            Die();

            // animation dependance :(

            
            return false;
        }else {
            return true;
        }
    }

    public void Die(bool real = true) {
        PauseMenu.isActive = false;
        rb.velocity = Vector3.zero;
        if(real) {
            animator.SetTrigger("fall");            
        }else {
            animator.Rebind();
        }
        this.enabled = false;
        body_collider.enabled = false;
        triggers.SetActive(false);
        radar.ResetAllNPCs();
    }

    public void Gas() {
        animator.SetTrigger(hash_gas);
    }
}
