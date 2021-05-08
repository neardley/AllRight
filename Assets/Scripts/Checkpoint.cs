using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int lap = 1;
    public bool hasWon;
    public GameObject player, finish;
    public GameObject[] checkpoints;
    public List<bool> checkList;
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

    //basic idea: recursive/iterate through checklist, check if the one before it is null or true
    //if it is make it equal false, change current one to true, go to the next one etc.
    private void OnTriggerEnter(Collider other) {
        Debug.Log("you've hit the checkpoint!" + other.gameObject.name);
        if (other.gameObject.tag.Equals("Check")) {  
            Debug.Log("In the if statement");
            for (int i = 0; i < checkList.Count; i++)
            {
                if (checkList.Count == 0) {
                    checkList[i] = true;
                    Debug.Log(checkList[i+1]);
                    Debug.Log(checkList[i]);
                    if (i>=1) {
                        checkList[i-1] = false;
                        Debug.Log(checkList[i-1]);
                    }
                }
                else if (checkList[i] == false) {
                    if (i>=1) {
                        checkList[i-1] = false;
                        Debug.Log(checkList[i-1]);
                        Debug.Log("i am in second if");
                    }
                    checkList[i] = true;
                    Debug.Log(checkList[i+1]);
                    Debug.Log(checkList[i]);
                    Debug.Log("i am in first else");
                } else {
                    
                    checkList[i] = true;
                    Debug.Log("i am in second else");
                    Debug.Log(checkList[i+1]);
                    Debug.Log(checkList[i]);
                    if (i>=1) {
                        checkList[i-1] = false;
                        Debug.Log(checkList[i-1] + "i am in last if");
                    }
                }
            }
        }
        //lap function
        if ((other.gameObject.tag == "FinishLine") && (checkList[5] == true)) {
            lap++;
            Debug.Log(lap);
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
