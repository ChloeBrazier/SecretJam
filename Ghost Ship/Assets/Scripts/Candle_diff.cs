using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleDiff : MonoBehaviour
{
    public List<Collider2D> objects;
    public List<Collider2D> prevObjects;
    public GameObject[] savedObjects;
    private Light light;
    private float effectRadius;
    private int layerMask;
    private bool prevState;

    private void Awake()
    {
        savedObjects = new GameObject[0];
        prevObjects = new List<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        effectRadius = light.range / 2;
        layerMask = LayerMask.GetMask("Interactable");

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

            if (exists)
            {
                savedObjects[i].GetComponent<SpriteRenderer>().enabled = !savedObjects[i].GetComponent<SpriteRenderer>().enabled;
                savedObjects[i].GetComponent<BoxCollider2D>().isTrigger = !savedObjects[i].GetComponent<BoxCollider2D>().isTrigger;
            }
        }

        if (light.enabled)
        {
            savedObjects = new GameObject[objects.Count];
            for (int i = 0; i < objects.Count; i++)
            {
                savedObjects[i] = objects[i].gameObject;
                if (prevObjects.Contains(objects[i]))
                    continue;
                savedObjects[i].GetComponent<SpriteRenderer>().enabled = !savedObjects[i].GetComponent<SpriteRenderer>().enabled;
                savedObjects[i].GetComponent<BoxCollider2D>().isTrigger = !savedObjects[i].GetComponent<BoxCollider2D>().isTrigger;
            }
        }

        prevObjects = objects;
    }
}
