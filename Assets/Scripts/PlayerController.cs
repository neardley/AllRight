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
    private int dir = 1;

    //private Rigidbody rb;


    public int id;
    public PhotonView photonView;
    public Player photonPlayer;


    public Rigidbody sphere;
    public float forwardAccel = 8f, reverseAccel = 4f, gravityForce = 10f;
    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;
    GameObject menuPanel;


    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;

        if (!photonView.IsMine) sphere.isKinematic = true;

        PlayerManager.instance.players.Add(this);

        //update minimap
        MinimapBehavior miniMap = FindObjectOfType<MinimapBehavior>();
        if (miniMap != null) miniMap.FindTargets();
        else Debug.Log("Could Not find Minimap Script");

        //set Color
        Debug.Log((string)photonPlayer.CustomProperties["Color"]);
        string[] colors = ((string)photonPlayer.CustomProperties["Color"]).Split(' ');
        MeshRenderer primaryColor = transform.Find("Car_Body_Red").GetComponent<MeshRenderer>();
        MeshRenderer auxColor = transform.Find("Car_Body_Blue").GetComponent<MeshRenderer>();
        primaryColor.material.color = Colors.ToColor(colors[0]);
        auxColor.material.color = Colors.ToColor(colors[1]);

    }

    void Awake()
    {
        menuPanel = FindObjectOfType<GameMenu>(true).gameObject;
        sphere = gameObject.GetComponentInChildren<Rigidbody>();
        sphere.transform.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (throttleAmount != 0)
            {
                sphere.AddForce(transform.forward * speed * throttleAmount * -3000 * Time.deltaTime);

                if (throttleAmount > 0)
                    dir = 1;
                else
                    dir = -1;

                if (turnAmount != 0)
                {
                    transform.Rotate(0f, turnSpeed * turnAmount * dir, 0f);
                }
            }

            transform.position = sphere.transform.position - new Vector3(0, 1f, 0);
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
        if (menuPanel.activeInHierarchy) menuPanel.SetActive(false);
        else menuPanel.SetActive(true);
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
