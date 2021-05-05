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
    public List<Transform> otherPlayers;
    public Image playerMarkerPrefab;
    public Image otherPlayerMarkerPrefab;

    public PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        otherPlayers = new List<Transform>();
    }


    public void FindTargets()
    {
        List<PlayerController> players = PlayerManager.instance.players;

        // Try find local player's transform
        PlayerController foundplayer = players.Find(x => x.photonView.IsMine);
        if (foundplayer != null) player = foundplayer.transform;

        // All others are put into otherPlayers
        otherPlayers = players.FindAll(x => !x.photonView.IsMine).ConvertAll<Transform>(x => x.transform);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (player != null)
        {
            Image playerMarker = Instantiate(playerMarkerPrefab, transform);
            playerMarker.transform.SetParent(transform);
            playerMarker.rectTransform.anchoredPosition = new Vector2(
                player.localPosition.x,
                player.localPosition.z
            ) * minimapScale;
        }
        
        foreach (Transform other in otherPlayers)
        {
            Image otherMarker = Instantiate(otherPlayerMarkerPrefab, transform);
            otherMarker.transform.SetParent(transform);
            otherMarker.rectTransform.anchoredPosition = new Vector2(
                other.localPosition.x,
                other.localPosition.z
            ) * minimapScale;
        }
    }

}
