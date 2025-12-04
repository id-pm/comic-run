using UnityEngine;
using UnityEngine.AI;

public class Citizen : NPC
{
    [SerializeField] private float run_distance = 10f, crazy_speed;
    [SerializeField] private ParticleSystem gas;
    
    public bool IsLaughing => is_laughing;
    void Start() {        
        current_state = NormalState;
    }
    
     void Update() {
        current_state.Invoke();
    }

    private void CrazyState() {

        if(navmesh_agent.remainingDistance <= navmesh_agent.stoppingDistance) { //done with path 
            SetNewDestinationCrazy();
        }
        if(navmesh_agent.velocity == Vector3.zero) {
            SetNewDestinationNormal();
        }    
	
    }   
    private void SetNewDestinationCrazy() {
        Transform player = GameManager.GetPlayerTransform;
        Vector3 dir_from_player = (transform.position - player.position).normalized * run_distance;
        Vector3 randomPoint = transform.position+dir_from_player + UnityEngine.Random.insideUnitSphere * range; //random point in a sphere 
        
        randomPoint.y = 0f;
        Debug.DrawRay(transform.position, dir_from_player, Color.red, 5.0f);
        
		
		// NavMeshHit hit stores the output in a variable called hit

		// 5 is the distance to check, assumes you use default for the NavMesh Layer name
		NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 5, NavMesh.AllAreas); 
		navmesh_agent.SetDestination(hit.position);

        Debug.DrawRay(hit.position, Vector3.up, Color.red, 10.0f);
    }

    
    
    public override void IsPlayerNearby(bool is_nearby) {
        is_player_nearby = is_nearby;
        CheckStates();
    }

    private void CheckStates() {
        if(is_player_nearby) {

            if(delayed_check_cor != null) {
                Debug.Log("stopping corouting");
                StopCoroutine(delayed_check_cor);
            }
            if(current_state == CrazyState) return;

            if(is_laughing) {
                return;
            }
            
            animator.SetBool("is_crazy", true);
            navmesh_agent.speed = crazy_speed;
            SetNewDestinationCrazy();
            current_state = CrazyState;

        }else {
            Debug.Log("starting cor");
            delayed_check_cor = StartCoroutine(GameManager.GetInstance.DelayedAction(state_change_cooldown, () => {
                Debug.Log("doing corouting");
                animator.SetBool("is_crazy", false);
                animator.SetBool("is_laughing", false);
                is_laughing = false;
                gas.Clear();
                gas.Stop();

                navmesh_agent.isStopped = false;
                navmesh_agent.speed = normal_speed;
                SetNewDestinationNormal();
                current_state = NormalState;
            }));
            
        } 
    }
    private bool is_laughing = false;
    private void LaughState() {  
        
    }   

    public void MakeLaugh() {
        if(is_laughing) return;

        gas.Play();

        int r = Random.Range(1, 6);

        AudioManager.PlaySound($"laugh{r}");
        GameManager.AddLaughed();

        is_laughing = true;

        if(delayed_check_cor != null) {
            StopCoroutine(delayed_check_cor);
        }

        animator.SetBool("is_laughing", true);
        current_state = LaughState;
        navmesh_agent.isStopped = true;
        
    }
}
