using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    None = 0,
    Candle = 1,
    Lantern = 2
}

public class SetPlayerState : MonoBehaviour
{
    //the player's current and previous states
    private PlayerState currentState;
    private PlayerState previousState;

    //the lantern in the level
    [SerializeField]
    private GameObject worldLantern;

    //list of candles in the level
    [SerializeField]
    private List<GameObject> candleList;

    //held candle
    private GameObject heldCandle;

    //item spawner transform
    [SerializeField]
    private Transform itemSpawner;

    //interaction distance threshold
    public float interactDistance;

    private Animator anim;

    private Light thisFlashlight;

    // Start is called before the first frame update
    void Start()
    {
        //initialize current state as none
        currentState = PlayerState.None;

        itemSpawner = GameObject.Find("ItemSpawner").transform;
        worldLantern = GameObject.Find("WorldLantern");
        anim = GetComponent<Animator>();
        thisFlashlight = GetComponentInChildren<Light>(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO: swap out player sprites and animations
        //check player state and switch active sprites and player
        switch (currentState)
        {
            case PlayerState.None:
                //swap to regular sprites and animations
                if (previousState != PlayerState.None)
                {
                    anim.SetInteger("Equipment", (int)currentState);
                }

                //pick up a candle if it's close enough to the player and they press the interact button
                foreach(GameObject candle in candleList)
                {
                    if(Input.GetMouseButtonDown(0) && Vector3.Distance(transform.position, candle.transform.position) < interactDistance)
                    {
                        //deactivate the candle and set it as the held candle
                        heldCandle = candle;
                        heldCandle.SetActive(false);

                        //activate player candle
                        GetComponentInChildren<Candle>(true).gameObject.SetActive(true);

                        //set player state to candle state
                        currentState = PlayerState.Candle;
                        anim.SetInteger("Equipment", (int)currentState);
                    }
                }

                break;
            case PlayerState.Candle:
                //drop the candle if the player presses the interact button
                if(Input.GetMouseButtonDown(0))
                {
                    //change player state
                    //previousState = currentState;
                    currentState = PlayerState.None;
                    anim.SetInteger("Equipment", (int)currentState);
                    DropCandle();
                }

                break;
                ;
            case PlayerState.Lantern:

                //TODO: swap to lantern sprites if the previous state wasn't lantern
                if(previousState != PlayerState.Lantern)
                {
                    anim.SetInteger("Equipment", (int)currentState);
                }

                if (thisFlashlight.enabled && !anim.GetBool("LanternOn"))
                {
                    anim.SetBool("LanternOn", true);
                }
                else if(!thisFlashlight.enabled && anim.GetBool("LanternOn"))
                {
                    anim.SetBool("LanternOn", false);
                }

                break;
                ;
        }

        //set previous state to current state
        previousState = currentState;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //permanently pick up the lantern if the player collides with it
        if(collision.gameObject == worldLantern)
        {
            //remove lantern object from the scene
            Destroy(worldLantern);

            //activate player lantern
            GetComponentInChildren<Flashlight>(true).gameObject.SetActive(true);

            //deactivate player candle if it's active
            if(currentState == PlayerState.Candle)
            {
                DropCandle();
            }

            //set player state to lantern
            //previousState = currentState;
            currentState = PlayerState.Lantern;
        }
    }

    private void DropCandle()
    {
        //deactivate player candle
        GetComponentInChildren<Candle>().gameObject.SetActive(false);

        //activate held candle in front of the player and at it to the candle list
        heldCandle.transform.position = itemSpawner.position;
        heldCandle.SetActive(true);
    }
}
