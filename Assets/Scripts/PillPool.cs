using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillPool : MonoBehaviour
{
    private static PillPool _i;
    public static PillPool GetInstance => _i;
    [SerializeField] private int amount = 5;
    public static int GetAmount => _i.amount;
    [SerializeField] private GameObject pill_prefab, gas_prefab;
    private List<GameObject> pill_pool, gas_pool;
    [SerializeField] private float spawn_delay = 4f;

    [SerializeField] private Transform[] pill_locations, gas_locations;
    private bool[] is_pill_there, is_gas_there;

    void Awake()
    {
        if(_i == null) {
            _i = this;
        }else {
            Debug.LogError("There is another PillPool...");
            Debug.Break();
            Destroy(this);
        }

        pill_pool = new List<GameObject>();
        gas_pool = new List<GameObject>();
        for(int i = 0; i < amount; i++) {
            CreatePill();
            CreateGas();
        }

        is_pill_there = new bool[pill_locations.Length];
        is_gas_there = new bool[gas_locations.Length];
        // розставимо пігулки (рандомно)
        for(int i = 0; i < amount; i++) {
            GameObject pill = GetPill();
            pill.transform.position = pill_locations[i].position;
            is_pill_there[i] = true;   

            GameObject gas = GetGas();
            gas.transform.position = gas_locations[i].position;
            is_gas_there[i] = true;         
        }
    }
    private GameObject CreatePill(bool active = false) {
        GameObject new_pill = Instantiate(pill_prefab);
        new_pill.SetActive(active);
        pill_pool.Add(new_pill);
        return new_pill;
    }
    private GameObject CreateGas(bool active = false) {
        GameObject new_gas = Instantiate(gas_prefab);
        new_gas.SetActive(active);
        gas_pool.Add(new_gas);
        return new_gas;
    }

    public GameObject GetPill() {
        foreach (GameObject pill in pill_pool) {
            if (!pill.activeInHierarchy) { // Якщо є невикористана пігулка - дати її     
                pill.SetActive(true);           
                return pill;
            }
        }
        // Якщо всі пігулки використані, створити нову та дати її
        return CreatePill(true);
    }

    public GameObject GetGas() {
        foreach (GameObject gas in gas_pool) {
            if (!gas.activeInHierarchy) { // Якщо є невикористаний газ - дати її     
                gas.SetActive(true);           
                return gas;
            }
        }
        // Якщо всі гази використані, створити новий та дати його
        return CreateGas(true);
    }
    public void ReturnPill(GameObject pill) {
        pill.SetActive(false);
        SetIsPillThere(pill.transform.position, false);
        // створити нову
        // пізніше
        StartCoroutine(GameManager.GetInstance.DelayedAction(spawn_delay, ()=> {
            PlacePill();
        }));
    }
    public void ReturnGas(GameObject gas) {
        gas.SetActive(false);
        SetIsGasThere(gas.transform.position, false);
        // створити нову
        // пізніше
        StartCoroutine(GameManager.GetInstance.DelayedAction(spawn_delay, ()=> {
            PlaceGas();
        }));
    }

    private void SetIsPillThere(Vector3 pill_pos, bool is_there) {
        for(int i = 0; i < pill_locations.Length; i++) {
            if(pill_pos == pill_locations[i].position) {
                is_pill_there[i] = is_there;
            }
        }
    }

    private void SetIsGasThere(Vector3 gas_pos, bool is_there) {
        for(int i = 0; i < gas_locations.Length; i++) {
            if(gas_pos == gas_locations[i].position) {
                is_gas_there[i] = is_there;
            }
        }
    }

    private void PlacePill() {
        GameObject pill = GetPill();
        int amount = pill_locations.Length;
        if(Random.Range(0, 2)==0) {
            for(int i = amount-1; i >= 0; i--) {
            if(is_pill_there[i] == false) {
                pill.transform.position = pill_locations[i].position;
                is_pill_there[i] = true;  
                return;       
                }
            }
        }else {
            for(int i = 0; i < amount; i++) {
            if(is_pill_there[i] == false) {
                pill.transform.position = pill_locations[i].position;
                is_pill_there[i] = true;  
                return;       
                }
            }
        }
        
    }
     private void PlaceGas() {
        GameObject gas = GetGas();
        int amount = gas_locations.Length;
        if(Random.Range(0, 2)==0) {
            for(int i = amount-1; i >= 0; i--) {
            if(is_gas_there[i] == false) {
                gas.transform.position = gas_locations[i].position;
                is_gas_there[i] = true;  
                return;       
                }
            }
        }else {
            for(int i = 0; i < amount; i++) {
            if(is_gas_there[i] == false) {
                gas.transform.position = gas_locations[i].position;
                is_gas_there[i] = true;  
                return;       
                }
            }
        }
        
    }
}
