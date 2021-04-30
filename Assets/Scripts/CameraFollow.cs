using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetTargetY = 3f;
    public float height = 4;
    public float distance = 10;
    public float smoothSpeed = 40;
    public float rotationSmoothSpeed = 40;

    public PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        StartCoroutine(FindTarget());
    }

    IEnumerator FindTarget()
    {
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");

        //if not all players are found retry after 0.1 seconds
        while (playerObjs.Length != PhotonNetwork.PlayerList.Length)
        {
            yield return new WaitForSeconds(0.1f);
            playerObjs = GameObject.FindGameObjectsWithTag("Player");
        }

        foreach (GameObject playerObj in playerObjs)
        {
            if (PhotonView.Get(playerObj).IsMine)
            {
                target = playerObj.transform;
            }
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(
            transform.position,
            target.TransformPoint(0f, height, -distance),
            smoothSpeed * Time.deltaTime
            );
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(target.position - transform.position + (Vector3.up * offsetTargetY), Vector3.up),
                rotationSmoothSpeed * Time.deltaTime
            );
        }
    }
}
