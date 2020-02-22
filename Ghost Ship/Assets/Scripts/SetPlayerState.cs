using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    None,
    Candle,
    Lantern
}

public class SetPlayerState : MonoBehaviour
{
    //the player's current state
    private PlayerState currentState;

    //the lantern
    [SerializeField]
    private GameObject lantern;

    // Start is called before the first frame update
    void Start()
    {
        //initialize current start as none
        currentState = PlayerState.None;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO: swap out player sprites and animations
        //check player state and switch active sprites and player
        switch (currentState)
        {
            case PlayerState.None:

                //pick up a candle if it's close enough to the player

                break;
            case PlayerState.Candle:

                //swap to candle-holding sprites and animations

                //drop the candle if the player presses the interact button


                break;
                ;
            case PlayerState.Lantern:
                break;
                ;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //permanently pick up the lantern if the player collides with it
        if(collision.gameObject == lantern)
        {
            //remove lantern object from the scene
            Destroy(lantern);

            //set player state to lantern

        }
    }
}
