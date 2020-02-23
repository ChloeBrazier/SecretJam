using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private List<Collider2D> objectsOverlapping;
    private GameObject[] savedObjects;
    private Light light;
    public float effectRadius;
    private int layerMask;
    private bool prevState;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        layerMask = LayerMask.GetMask("Interactable");

        savedObjects = new GameObject[0];
        objectsOverlapping = new List<Collider2D>();

        prevState = true;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            light.enabled = !light.enabled;
            audioSource.Play();
        }

        // find all overlapping objects
        objectsOverlapping.Clear();
        objectsOverlapping.AddRange(Physics2D.OverlapCircleAll((Vector2)transform.position, effectRadius, layerMask));

        if (light.enabled && !prevState)
        {
            for (int i = 0; i < savedObjects.Length; i++)
            {
                savedObjects[i].GetComponent<SpriteRenderer>().enabled = true;
                savedObjects[i].GetComponent<BoxCollider2D>().isTrigger = false;

                // Making sure saved objects are not immediately deleted below
                for (int j = 0; j < objectsOverlapping.Count; j++)
                {
                    if (savedObjects[i] == objectsOverlapping[j].gameObject)
                    {
                        // remove any overlapping objects that are being reenabled, 
                        // so that they are not immediately disabled below
                        objectsOverlapping.RemoveAt(j);
                        j--;
                    }
                }
            }
            // Saving objects that are newly collided with then disabling them
            savedObjects = new GameObject[objectsOverlapping.Count];
            for (int i = 0; i < objectsOverlapping.Count; i++)
            {
                savedObjects[i] = objectsOverlapping[i].gameObject;

                objectsOverlapping[i].GetComponent<SpriteRenderer>().enabled = false;
                objectsOverlapping[i].GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }

        prevState = light.enabled;
    }
}
