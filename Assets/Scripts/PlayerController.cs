using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;


/// <summary>
/// TODO: add photon view checks
/// </summary>

public class PlayerController : MonoBehaviour
{
    public bool canTurnRight = true;
    public float turnSpeed = 1.5f;
    private float turnAmount = 0f;
    public float speed = 200f;
    private float throttleAmount = 0f;
    public bool flip = false;
    public float maxTurn = 20f;

    private Rigidbody rb;

    public int id;
    public PhotonView photonView;
    public Player photonPlayer;


    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;

        PlayerManager.instance.players[id - 1] = this;

        if (!photonView.IsMine)
            rb.isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {

            if (throttleAmount != 0)
            {
                rb.AddForce(transform.forward * speed * throttleAmount * -1);
            }

            //if (throttleAmount != 0)
            //{
            //    foreach (WheelCollider wheel in throttleWheels)
            //    {
            //        wheel.motorTorque += speed * Time.deltaTime * throttleAmount * -1;
            //    }
            //}

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
        //Quaternion currentRot = this.gameObject.transform.rotation;
        //Vector3 currentPos = this.gameObject.transform.position;
        //if (flip)
        //{
        //    this.gameObject.transform.rotation = new Quaternion(180, currentRot.y, currentRot.z, currentRot.w);
        //    //this.gameObject.transform.position = new Vector3(currentPos.x, 2.6f, currentPos.z);
        //}
        //else
        //{
        //    this.gameObject.transform.rotation = new Quaternion(0, currentRot.y, currentRot.z, currentRot.w);
        //    //this.gameObject.transform.position = new Vector3(currentPos.x, 0f, currentPos.z);
        //}
    }

    public IEnumerator SpeedBoost()
    {
        if (photonView.IsMine)
        {
            speed += 1000;
            yield return new WaitForSeconds(5f);
            speed -= 1000;
        }
    }
}
