using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool canTurnRight = true;
    public float turnSpeed = 1f;
    private float turnAmount = 0f;
    public float speed = 0.5f;
    private float throttleAmount = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (turnAmount != 0) {
            transform.Rotate(0f, turnSpeed * turnAmount, 0f);
        }
        if (throttleAmount != 0) {
            transform.position += transform.forward * speed * throttleAmount;
        }
    }

    void HandleTurnChange(float turn) {
        if (turn > 0)
        {
            if (canTurnRight)
            {
                Debug.Log("Turn Right");
                turnAmount = turn;
                // TODO turn right
            }
            else
            {
                Debug.Log("Guard Right");
                turnAmount = 0f;
                // TODO guard/block
            }
        }
        else if (turn < 0)
        {
            if (!canTurnRight)
            {
                Debug.Log("Turn Left");
                turnAmount = turn;
                // TODO turn left
            }
            else
            {
                Debug.Log("Guard Left");
                turnAmount = 0f;
                // TODO guard/block
            }
        }
        else
        {
            Debug.Log("Stop Turn/Block");
            turnAmount = 0f;
            // TODO don't turn or block
        }
    }

    void OnTurn(InputValue input)
    {
        float turn = input.Get<float>();
        HandleTurnChange(turn);
    }

    void OnBoost(InputValue input)
    {
        if (input.Get<float>() == 1)
        {
            Debug.Log("Boost");
            // TODO player boosts
        }
        else
        {
            Debug.Log("Stop Boost");
            // TODO stop boost
        }
    }

    void OnFlip()
    {
        canTurnRight = !canTurnRight;
        HandleTurnChange(turnAmount);
        Debug.Log("Flipped! Can only turn " + (canTurnRight ? "right." : "left."));
        // TODO flip player
    }

    void OnThrottle(InputValue input)
    {
        float throttle = input.Get<float>();
        if (throttle > 0)
        {
            Debug.Log("Accelerate");
            throttleAmount = throttle;
            // TODO accelerate
        }
        else if (throttle < 0)
        {
            Debug.Log("Brake");
            throttleAmount = throttle;
            // TODO brake
        }
        else
        {
            Debug.Log("Coast");
            throttleAmount = 0f;
            // TODO Coast
        }
    }

    void OnOpenMenu()
    {
        Debug.Log("Toggle Menu");
        // TODO toggle menu
    }
}
