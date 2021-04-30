using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject player;
    public GameObject[] checkpoints;
    public List<(GameObject, bool)> checkList;
    //private Lap lap;

    // Start is called before the first frame update
    void Start()
    {
        //lap = GetComponent<Lap>();
        player = GameObject.FindGameObjectWithTag("Player");
        checkpoints = GameObject.FindGameObjectsWithTag("Check");
        checkList = new List<(GameObject, bool)>();
        foreach (GameObject checkpoint in checkpoints)
        {
            checkList.Add((checkpoint, false));
            Debug.Log(checkList[0].Item2);
        }
    }

    //basic idea: recursive/iterate through checklist, check if the one before it is null or true
    //if it is make it equal false, change current one to true, go to the next one etc.

    // Update is called once per frame
    void FixedUpdate()
    {

        
    }

    

}
