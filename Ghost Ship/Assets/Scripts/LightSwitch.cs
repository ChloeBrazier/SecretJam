using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Eddie Brazier - Secret Game Jam

public class LightSwitch : MonoBehaviour
{
    //modifiable list of objects affected by this light
    [SerializeField]
    private List<GameObject> affectedObjects;

    //light timer
    [SerializeField]
    private float lightTimer;
    private float lightTick = 0;

    //shadow timer
    [SerializeField]
    private float shadowTimer;
    private float shadowTick = 0;

    //light bool
    private bool lightOn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lightOn == true)
        {
            //count down until the light turns off
            if (lightTick <= 1)
            {
                //increment light tick
                lightTick += Time.deltaTime / lightTimer;
            }
            else
            {
                //change light bool and reset light tick
                lightOn = false;
                GetComponent<Light>().enabled = false;

                lightTick = 0f;
            }
        }
        else
        {
            //count down until the light turns on
            if(shadowTick <= 1)
            {
                //increment shadow tick
                shadowTick += Time.deltaTime / shadowTimer;
            }
            else
            {
                //change active objects
                foreach(GameObject platform in affectedObjects)
                {
                    if(platform.activeSelf == true)
                    {
                        platform.SetActive(false);
                    }
                    else
                    {
                        platform.SetActive(true);
                    }
                }

                //reset light bool and shadow tick
                lightOn = true;
                GetComponent<Light>().enabled = true;

                shadowTick = 0f;
            }
        }
    }
}
