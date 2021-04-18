using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip_Function : MonoBehaviour
{
    // Start is called before the first frame update
    public bool flip;
    void Start()
    {
        flip = false;
    }


    public void Flip()
    {
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



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            flip = !flip;
            Flip();
        }
    }
}
