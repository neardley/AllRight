using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerSphere") {

            PlayerSphere carSphere = other.GetComponent<PlayerSphere>();
            PlayerController car = carSphere.player;
            car.StartCoroutine(car.SpeedBoost());
            //gameObject.SetActive(false);
            Destroy(gameObject);

        }
    }
}
