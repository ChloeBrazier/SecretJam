using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public List<Collider2D> objects;
    public GameObject[] savedObjects;
    private Light light;
    private float effectRadius;
    private int layerMask;
    private bool prevState;

    private FindInteractables interactablesContainer;

    private void Awake()
    {
        savedObjects = new GameObject[0];   
    }

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        effectRadius = light.range / 2;
        layerMask = LayerMask.GetMask("Interactable");
        interactablesContainer = GameObject.Find("Player").GetComponent<FindInteractables>();

        prevState = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        objects = new List<Collider2D>();
        objects.AddRange(Physics2D.OverlapCircleAll((Vector2)transform.position, effectRadius, layerMask));

        for (int i = 0; i < savedObjects.Length; i++)
        {
            bool exists = false;
            for (int j = 0; j < objects.Count; j++)
            {
                if (objects[j] == savedObjects[i])
                {
                    exists = true;
                    break;
                }
            }

            if (!exists && interactablesContainer.defaultVals.ContainsKey(savedObjects[i].name))
            {
                savedObjects[i].GetComponent<SpriteRenderer>().enabled = interactablesContainer.defaultVals[savedObjects[i].name].sprite;
                savedObjects[i].GetComponent<BoxCollider2D>().isTrigger = interactablesContainer.defaultVals[savedObjects[i].name].trigger;
            }
        }

        if (light.enabled)
        {
            savedObjects = new GameObject[objects.Count];
            for (int i = 0; i < objects.Count; i++)
            {
                savedObjects[i] = objects[i].gameObject;
                if (!interactablesContainer.defaultVals.ContainsKey(savedObjects[i].name))
                    continue;
                savedObjects[i].GetComponent<SpriteRenderer>().enabled = !interactablesContainer.defaultVals[savedObjects[i].name].sprite;
                savedObjects[i].GetComponent<BoxCollider2D>().isTrigger = !interactablesContainer.defaultVals[savedObjects[i].name].trigger;
            }
        }
    }
}
