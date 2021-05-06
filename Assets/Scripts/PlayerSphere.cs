using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphere : MonoBehaviour
{
    // necessary to keep up with player from sphere for speed boost,
    // because car is no longer parent of the sphere
    public PlayerController player;

    private void OnCollisionEnter(Collision other)
    {
        player.PlaySFX("car_crash_classic", true);
    }
}
