using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool canTurnRight = true;
    public float turnSpeed = 1.5f;
    private float turnAmount = 0f;
    public float speed = 200f;
    private float throttleAmount = 0f;
    public bool flip = false;
    public float maxTurn = 20f;

    public List<WheelCollider> throttleWheels;
    public List<WheelCollider> steeringWheels;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //if (throttleAmount != 0)
        //{
        //    transform.position += transform.forward * speed * throttleAmount;
        //}

        if (throttleAmount != 0)
        {
            foreach (WheelCollider wheel in throttleWheels)
            {
                wheel.motorTorque = speed * Time.deltaTime * throttleAmount;
            }
        }
        if (turnAmount != 0)
        {
            transform.Rotate(0f, turnSpeed * turnAmount, 0f);
        }

        //if (turnAmount != 0)
        //{
        //    foreach (WheelCollider wheel in steeringWheels)
        //    {
        //        wheel.steerAngle = turnSpeed * maxTurn * turnAmount;
        //    }
        //}
    }

    void HandleTurnChange(float turn) {
        if (turn > 0)
        {
            if (canTurnRight)
            {
                Debug.Log("Turn Right");
                turnAmount = turn;
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
        Flip();
    }

    void OnThrottle(InputValue input)
    {
        float throttle = input.Get<float>();
        if (throttle > 0)
        {
            Debug.Log("Accelerate");
            throttleAmount = throttle;
        }
        else if (throttle < 0)
        {
            Debug.Log("Brake");
            throttleAmount = throttle;
        }
        else
        {
            Debug.Log("Coast");
            throttleAmount = 0f;
        }
    }

    void OnOpenMenu()
    {
        Debug.Log("Toggle Menu");
        // TODO toggle menu
    }

    public void Flip()
    {
        flip = !flip;
        Quaternion current = this.gameObject.transform.rotation;
        if (flip)
        {
            this.gameObject.transform.rotation = new Quaternion(180, current.y, current.z, current.w);
        }
        else
        {
            this.gameObject.transform.rotation = new Quaternion(0, current.y, current.z, current.w);
        }
    }
}
