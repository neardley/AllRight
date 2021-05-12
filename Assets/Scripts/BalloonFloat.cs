using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonFloat : MonoBehaviour
{
    float initTime;
    Vector3 origPos;
    Vector3 origRot;
    [Range(0, 5)]
    public float speed = 0.6f;
    [Range(0, 5)]
    public float amplitude = 5;
    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.position;
        origRot = transform.rotation.eulerAngles;
        initTime = Random.value * Mathf.PI * 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = origPos + (Vector3.up * amplitude * Mathf.Sin((Time.realtimeSinceStartup + initTime) * speed));
        transform.rotation = Quaternion.Euler(origRot + (transform.up * amplitude * Mathf.Sin((Time.realtimeSinceStartup + initTime + (Mathf.PI / 2)) * speed)));
    }
}
