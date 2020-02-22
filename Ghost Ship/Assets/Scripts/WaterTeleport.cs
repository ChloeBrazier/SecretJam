using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTeleport : MonoBehaviour
{
    //the transform that the player will be sent to when they touch the water
    [SerializeField]
    private Transform checkPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //send the player back to the checkpoint if they touch the water
        if (collision.gameObject.GetComponent<SetPlayerState>() != null)
        {
            collision.gameObject.transform.position = checkPoint.position;
        }
    }
}
