using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    public int lap = 1;
    public int j = -1;
    public bool hasWon, gotHalf;
    public GameObject player, finish;
    public GameObject[] checkpoints;
    public List<bool> checkList;
    public InputAction resetLastCP;
    //private Lap lap;

    // Start is called before the first frame update
    void Start()
    {
        //lap = GetComponent<Lap>();
        player = GameObject.FindGameObjectWithTag("Player");
        checkpoints = GameObject.FindGameObjectsWithTag("Check");
        finish = GameObject.FindGameObjectWithTag("FinishLine");
        checkList = new List<bool>();
        foreach (GameObject checkpoint in checkpoints)
        {
            checkList.Add(false);
            Debug.Log(checkList.Count);
        }
    }

    void Awake() {
        resetLastCP.performed += ctx => ResetToLastCheckPoint();
    }

    void ResetToLastCheckPoint() {
        player.transform.position = new Vector3(247f, 0.1083854f, 96f);
    }

    //basic idea: recursive/iterate through checklist, check if the one before it is null or true
    //if it is make it equal false, change current one to true, go to the next one etc.
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Check")) {  
            j++;
            for (int i = 0; i < j; i++)
            {
                if (checkList[i] == false) {
                    checkList[i] = true;
                    for (int k = 0; k < i; k++)
                    {
                        if (other.gameObject.name == "Halfway") {
                            checkList[5] = true;
                            gotHalf = true;
                        }  
                        checkList[k] = false;
                    }
                }
            }
        }
        //lap function
        if ((other.gameObject.tag == "FinishLine") && (gotHalf == true)) {
            lap++;
        }
        if (lap == 3) {
            hasWon = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (hasWon == true) {
            Debug.Log("You have won!");
        }
        
    }
}
