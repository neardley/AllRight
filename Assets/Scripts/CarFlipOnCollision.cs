/* Travis Mewborne
 * 4/20/21
 * Calls flip function on trigger enter
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFlipOnCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Equals("FlipColliderTriggerBox")) {
            this.transform.parent.GetComponent<PlayerController>().Flip();
        }
    }
}
