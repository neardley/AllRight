using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lap : MonoBehaviour
{
    public int lap = 0;
    public bool hasWon;
    public GameObject player, finish;
    public Checkpoint chck;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        chck = GetComponent<Checkpoint>();
        yield return new WaitForEndOfFrame();
        player = GameObject.FindGameObjectWithTag("Player");
        finish = GameObject.FindGameObjectWithTag("FnshLine");
    }

    // Update is called once per frame
    void Update()
    {
        if (hasWon == true) {
            Debug.Log("You have won!");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if ((other.gameObject.tag == "Player") && (chck.checkList[1].Item2 == true)) {
            lap++;
            Debug.Log(lap);
        }
        if (lap == 3) {
            hasWon = true;
        }
    }
}
