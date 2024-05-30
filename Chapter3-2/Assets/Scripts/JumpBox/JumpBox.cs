using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    public float JumpPower = 10;
    private void OnCollisionEnter(Collision collision)
    {
        // layer 6 = Player
        if (collision.gameObject.layer == 6)
        {
            PlayerManager.Instance.Player.controller._rigidbody.velocity = Vector3.zero;
            PlayerManager.Instance.Player.controller._rigidbody.AddForce(Vector2.up * JumpPower, ForceMode.Impulse);
        }
    }
}
