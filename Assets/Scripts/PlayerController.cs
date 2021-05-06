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
    private bool isTurning = false;
    private int dir = 1;

    //private Rigidbody rb;


    public int id;
    public PhotonView photonView;
    public Player photonPlayer;


    public Rigidbody sphere;
    private Vector3 oldPos;
    public float forwardAccel = 8f, reverseAccel = 4f, gravityForce = 10f, dragOnGround = 3f;
    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.2f;
    public Transform groundRayPoint;

    GameObject menuPanel;

    [Header("Sound Effects")]
    [Range(0, 1)]
    public float SFXVolume = 0.65f;

    private AudioSource idleSound;
    private AudioSource fastSound;
    private AudioSource screechSound;
    private AudioSource boostSound;
    private AudioSource sfx;


    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;

        if (!photonView.IsMine)
        {
            sphere.isKinematic = true;
            sphere.GetComponent<Checkpoint>().enabled = false;
        }

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
        /*menuPanel = FindObjectOfType<GameMenu>(true).gameObject;*/
        sphere = gameObject.GetComponentInChildren<Rigidbody>();
        PlayerSphere sphereObj = sphere.GetComponent<PlayerSphere>();
        sphereObj.player = this;
        sphere.transform.parent = null;

        idleSound = gameObject.AddComponent<AudioSource>();
        idleSound.clip = Resources.Load<AudioClip>("Sounds/car_idle");
        idleSound.loop = true;
        idleSound.rolloffMode = AudioRolloffMode.Linear;
        idleSound.maxDistance = 250;
        fastSound = gameObject.AddComponent<AudioSource>();
        fastSound.clip = Resources.Load<AudioClip>("Sounds/car_topspeed");
        fastSound.loop = true;
        fastSound.rolloffMode = AudioRolloffMode.Linear;
        fastSound.maxDistance = 250;

        screechSound = gameObject.AddComponent<AudioSource>();
        screechSound.clip = Resources.Load<AudioClip>("Sounds/car_screech");
        screechSound.loop = true;
        screechSound.rolloffMode = AudioRolloffMode.Linear;
        screechSound.maxDistance = 250;

        boostSound = gameObject.AddComponent<AudioSource>();
        boostSound.clip = Resources.Load<AudioClip>("Sounds/boost");
        boostSound.loop = true;
        boostSound.rolloffMode = AudioRolloffMode.Linear;
        boostSound.maxDistance = 250;

        sfx = gameObject.AddComponent<AudioSource>();
        sfx.rolloffMode = AudioRolloffMode.Linear;
        sfx.maxDistance = 250;

        idleSound.Play();
        fastSound.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            grounded = false;
            RaycastHit hit;
            if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
            {
                grounded = true;
                //transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            }

            if (grounded)
            {
                sphere.drag = dragOnGround;

                if (throttleAmount != 0)
                {
                    sphere.AddForce(transform.forward * speed * throttleAmount * -3000);
                }

                if (turnAmount != 0)
                {
                    if (throttleAmount == 0 || throttleAmount > 0)
                        dir = 1;
                    else
                        dir = -1;

                    transform.Rotate(0f, turnSpeed * turnAmount * dir, 0f);
                }
            }
            else
            {
                sphere.drag = 0.1f;
                sphere.AddForce(Vector3.up * -gravityForce * 500);
            }

            transform.position = sphere.transform.position - new Vector3(0, 1f, 0);
        }
        else oldPos = sphere.transform.position;

        UpdateSFX();
    }

    void HandleTurnChange(float turn)
    {
        if (turn > 0)
        {
            if (canTurnRight)
            {
                if (!screechSound.isPlaying) screechSound.Play();
                turnAmount = turn;
                isTurning = true;
            }
            else
            {
                turnAmount = 0f;
                PlaySFX("boost_empty", true);
                // TODO guard/block
            }
        }
        else if (turn < 0)
        {
            if (!canTurnRight)
            {
                if (!screechSound.isPlaying) screechSound.Play();
                turnAmount = turn;
                isTurning = true;
            }
            else
            {
                turnAmount = 0f;
                PlaySFX("boost_empty", true);
                // TODO guard/block
            }
        }
        else
        {
            screechSound.Stop();
            turnAmount = 0f;
            isTurning = false;
        }
    }

    void OnTurn(InputValue input)
    {
        float turn = input.Get<float>();
        HandleTurnChange(turn);
    }

    void OnFlip()
    {
        canTurnRight = !canTurnRight;
        HandleTurnChange(turnAmount);
        Debug.Log("Flipped! Can only turn " + (canTurnRight ? "right." : "left."));
        PlaySFX("car_flip", true);
        Flip();
    }

    void OnThrottle(InputValue input)
    {
        float throttle = input.Get<float>();
        if (throttle > 0)
        {
            throttleAmount = throttle;
        }
        else if (throttle < 0)
        {
            throttleAmount = throttle;
        }
        else
        {
            throttleAmount = 0f;
        }
    }

    void OnOpenMenu()
    {
        Debug.Log("Toggle Menu");
        // if (menuPanel.activeInHierarchy) menuPanel.SetActive(false);
        // else {
        //     menuPanel.SetActive(true);
        //     sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/menu_select"));
        // }
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

    public IEnumerator SpeedBoost(GameObject speedBoostObj)
    {
        if (photonView.IsMine) speed += 7;
        speedBoostObj.SetActive(false);
        boostSound.Play();
        PlaySFX("boost_empty", true);

        yield return new WaitForSeconds(8f);

        if (photonView.IsMine) speed -= 7;
        speedBoostObj.SetActive(true);
        boostSound.Stop();
        PlaySFX("boost_empty", true);
    }

    public void PlaySFX(string filename, bool forEveryone = false)
    {
        if (forEveryone || photonView.IsMine)
            sfx.PlayOneShot(Resources.Load<AudioClip>("Sounds/" + filename), SFXVolume);
    }

    public void GameOver()
    {
        PlaySFX("player_win");
        idleSound.Stop();
        fastSound.Stop();
        boostSound.Stop();
        screechSound.Stop();
        AudioSource bgmSource = GameObject.Find("EventSystem").GetComponent<AudioSource>();
        if (bgmSource != null)
        {
            bgmSource.clip = Resources.Load<AudioClip>("Sounds/bgm_end");
            bgmSource.Play();
        }
    }

    private void UpdateSFX()
    {
        Vector3 sphereVelocity = GetSphereVelocity();

        // set 3D positioning effects
        float blend = Vector3.Distance(GameObject.FindObjectOfType<Camera>().transform.position, gameObject.transform.position) / 15;
        idleSound.spatialBlend = blend;
        fastSound.spatialBlend = blend;
        screechSound.spatialBlend = blend;
        boostSound.spatialBlend = blend;
        sfx.spatialBlend = blend;

        // change car engine pitch based on velocity
        float idlePitch = Mathf.LerpUnclamped(1, 2, sphereVelocity.magnitude / 50f);
        // Debug.Log(sphereVelocity.magnitude);
        idleSound.pitch = idlePitch;
        idleSound.volume = (idlePitch * -1 + 2) * SFXVolume;
        fastSound.pitch = idlePitch - 1;
        fastSound.volume = (-idleSound.volume + 1) * SFXVolume;

        screechSound.volume = 0.7f * SFXVolume;
        boostSound.volume = SFXVolume;
        sfx.volume = SFXVolume;

        // car screeches when sliding
        float turnAngle = Mathf.Abs(Vector3.Angle(transform.forward, new Vector3(sphereVelocity.x, 0f, sphereVelocity.z)) - 180);
        if (sphereVelocity.magnitude > 0.5 && turnAngle > 15 && turnAngle < 165)
        {
            if (!screechSound.isPlaying) screechSound.Play();
        }
        else
        {
            if (!isTurning) screechSound.Stop();
        }
    }

    private Vector3 GetSphereVelocity()
    {
        // if this is my car, we can just get the velocity, otherwise we have to calculate it because photon just moves the transform
        if (photonView.IsMine) return sphere.velocity;
        if (oldPos == null) oldPos = sphere.transform.position;
        return (sphere.transform.position - oldPos) / Time.fixedDeltaTime;
    }
}
