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

    //list of candles in the level and their original positions
    [SerializeField]
    private List<GameObject> candleList;
    private List<Vector3> candleLocations;

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

        //save original candle positions
        candleLocations = new List<Vector3>();
        foreach(GameObject candle in candleList)
        {
            Vector3 tempTransform = candle.transform.position;
            candleLocations.Add(tempTransform);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                        Candle liftedCandle = candle.GetComponent<Candle>();
                        heldCandle.SetActive(false);

                        //activate player candle
                        Candle playerCandle = GetComponentInChildren<Candle>(true);
                        playerCandle.gameObject.SetActive(true);

                        playerCandle.objects = liftedCandle.objects;
                        playerCandle.savedObjects = liftedCandle.savedObjects;
                        // Candle_diff functionality ----- ------ playerCandle.prevObjects = liftedCandle.prevObjects;

                        //set player state to candle state
                        currentState = PlayerState.Candle;
                        anim.SetInteger("Equipment", (int)currentState);
                        break;
                    }
                }

                break;
            case PlayerState.Candle:
                //drop the candle if the player presses the interact button
                if(Input.GetMouseButtonDown(0))
                {
                    //change player state
                    currentState = PlayerState.None;
                    anim.SetInteger("Equipment", (int)currentState);
                    DropCandle();
                }

                break;
                ;
            case PlayerState.Lantern:

                //swap to lantern sprites if the previous state wasn't lantern
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

        //check for candle reset
        if(Input.GetKeyDown(KeyCode.R))
        {
            //drop a candle if the player is holding one
            if(currentState == PlayerState.Candle)
            {
                currentState = PlayerState.None;
                anim.SetInteger("Equipment", (int)currentState);
                DropCandle();
            }

            for(int i = 0; i < candleList.Count; i++)
            {
                candleList[i].transform.position = candleLocations[i];
                Debug.Log("new candle location: " + candleList[i].transform.position);
                Debug.Log("candlle location listing: " + candleLocations[i]);
            }
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
            currentState = PlayerState.Lantern;
        }
    }

    private void DropCandle()
    {
        //deactivate player candle
        Candle playerCandle = GetComponentInChildren<Candle>();
        playerCandle.gameObject.SetActive(false);

        //activate held candle in front of the player and at it to the candle list
        heldCandle.GetComponent<Candle>().objects = playerCandle.objects;
        heldCandle.GetComponent<Candle>().savedObjects = playerCandle.savedObjects;
        // Candle_diff functionality ----- ------ heldCandle.GetComponent<Candle>().prevObjects = playerCandle.prevObjects;
        heldCandle.transform.position = itemSpawner.position;
        heldCandle.SetActive(true);
    }
}
