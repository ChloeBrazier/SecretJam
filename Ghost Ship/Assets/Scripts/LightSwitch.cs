using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Eddie Brazier - Secret Game Jam

public class LightSwitch : MonoBehaviour
{
    //modifiable list of objects affected by this light
    [SerializeField,Tooltip("Any object affected by the lights MUST HAVE A BOX COLLIDER")]
    private List<GameObject> affectedObjects;

    //bool for objects being changed
    private bool objectsChanged;

    //light timer
    [SerializeField]
    private float lightTimer;
    private float lightTick = 0;

    //shadow timer
    [SerializeField]
    private float shadowTimer;
    private float shadowTick = 0;

    //fade timer
    [SerializeField]
    private float fadeTimer;
    private float fadeTick = 0;

    //the light attached to this object
    private Light objectlight;

    //light bool
    private bool lightOn = true;

    //max light intensity
    private float maxIntensity;

    private AudioSource audioSource;

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        //get object light
        objectlight = GetComponent<Light>();

        //get max intensity
        maxIntensity = objectlight.intensity;

        audioSource = GetComponent<AudioSource>();

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource != null && player.gameObject.transform.position.y > 4.2f)
            audioSource.volume = 0;
        else if (audioSource != null)
            audioSource.volume = 1;
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
                //Lerp light intensity
                if(fadeTick <= 1)
                {
                    objectlight.intensity = Mathf.Lerp(objectlight.intensity, 0f, fadeTick);

                    //increment fade tick
                    fadeTick += Time.deltaTime / fadeTimer;
                }
                else
                {
                    if(audioSource != null)
                        audioSource.Play();
                    //change light bool and reset light tick
                    lightOn = false;

                    lightTick = 0f;
                    fadeTick = 0f;
                }
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
                if(objectsChanged == false)
                {
                    foreach (GameObject platform in affectedObjects)
                    {
                        if (platform.GetComponent<SpriteRenderer>().enabled == true)
                        {
                            platform.GetComponent<SpriteRenderer>().enabled = false;
                            platform.GetComponent<BoxCollider2D>().isTrigger = true;
                        }
                        else
                        {
                            platform.GetComponent<SpriteRenderer>().enabled = true;
                            platform.GetComponent<BoxCollider2D>().isTrigger = false;
                        }
                    }

                    objectsChanged = true;
                }
                

                //lerp light intensity to max intensity
                if (fadeTick <= 1)
                {
                    objectlight.intensity = Mathf.Lerp(objectlight.intensity, maxIntensity, fadeTick);

                    //increment fade tick
                    fadeTick += Time.deltaTime / fadeTimer;
                }
                else
                {
                    if(audioSource != null)
                        audioSource.Play();
                    //reset light
                    lightOn = true;
                    objectsChanged = false;
                    shadowTick = 0f;
                    fadeTick = 0f;
                }
            }
        }
    }
}
