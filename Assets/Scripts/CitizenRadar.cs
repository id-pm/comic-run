using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenRadar : MonoBehaviour
{
     private List<NPC> npcs_in_trigger;
    private void Start() {
        npcs_in_trigger = new List<NPC>();
    }
    
    private void OnTriggerEnter(Collider other) {
        Debug.Log($"npc radar enter: {other.name}");
        if(other.TryGetComponent(out NPC npc)) {          
            npcs_in_trigger.Add(npc);  
            npc.IsPlayerNearby(true);
        }else {
            
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log($"npc radar exit: {other.name}");
        if(other.TryGetComponent(out NPC npc)) {   
            npcs_in_trigger.Remove(npc);         
            npc.IsPlayerNearby(false);
        }else {
            
        }
    }

    public void ResetAllNPCs() {
        if(npcs_in_trigger == null) return;
        
        int count = npcs_in_trigger.Count;
        for(int i = 0; i < count; i++) {
            npcs_in_trigger[i].IsPlayerNearby(false);
            if(npcs_in_trigger[i] is Police) {
                (npcs_in_trigger[i] as Police).Dance();
            }
        }
    }
}
