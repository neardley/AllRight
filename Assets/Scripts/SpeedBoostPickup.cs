using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {



            Debug.Log("Picking up Speed Boost");

            PlayerController car = other.GetComponent<PlayerController>();
            car.StartCoroutine(car.SpeedBoost());
            //gameObject.SetActive(false);
            Destroy(gameObject);

            //speed booth effect code
        }
    }
}