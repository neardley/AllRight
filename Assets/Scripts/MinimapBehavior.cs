using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;


[RequireComponent(typeof(PhotonView))]
public class MinimapBehavior : MonoBehaviour
{
    public float minimapScale;
    public Transform player;
    public Transform[] otherPlayers;
    public Image playerMarkerPrefab;
    public Image otherPlayerMarkerPrefab;

    public PhotonView photonView;
    List<PlayerController> players;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        otherPlayers = new Transform[PhotonNetwork.PlayerList.Length - 1];
    }

    public IEnumerator FindTargets()
    {
        players = PlayerManager.instance.players;
        List<Transform> newPlayers = new List<Transform>();

        //if not all players are found retry after 0.1 seconds
        while (players.Count != PhotonNetwork.PlayerList.Length)
        {
            yield return new WaitForSeconds(0.1f);
            players = PlayerManager.instance.players;
        }

        foreach (PlayerController p in players)
        {
            while (players == null)
            {
                Debug.Log("player null retrying in 0.1 seconds");
                yield return new WaitForSeconds(0.1f);
            }
            //Debug.Log(p);
            if (p.photonView.IsMine)
            {
                player = p.transform;
            }
            else
                newPlayers.Add(p.transform);
        }
        otherPlayers = newPlayers.ToArray();
    }


    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Image marker = Instantiate(playerMarkerPrefab, transform);
            marker.transform.SetParent(transform);
            marker.rectTransform.anchoredPosition = new Vector2(
                player.localPosition.x,
                player.localPosition.z
            ) * minimapScale;

            foreach (Transform other in otherPlayers)
            {
                marker = Instantiate(otherPlayerMarkerPrefab, transform);
                marker.transform.SetParent(transform);
                marker.rectTransform.anchoredPosition = new Vector2(
                    other.localPosition.x,
                    other.localPosition.z
                ) * minimapScale;
            }
        }
        
    }

}
