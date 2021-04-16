using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapBehavior : MonoBehaviour
{
    public float minimapScale;
    public Transform player;
    public Transform[] otherPlayers;
    public Image playerMarkerPrefab;
    public Image otherPlayerMarkerPrefab;

    // Update is called once per frame
    void Update()
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
