using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    private Collider2D[] objects_Arr;
    private GameObject[] savedObjects;
    private Light light;
    private float effectRadius;
    private int layerMask;
    private bool prevState;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        effectRadius = light.range / 2;
        layerMask = LayerMask.GetMask("Interactable");

        savedObjects = new GameObject[0];
        objects_Arr = new Collider2D[0];

        prevState = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        objects_Arr = Physics2D.OverlapCircleAll((Vector2)transform.position, effectRadius, layerMask);
        List<Collider2D> objects = new List<Collider2D>();
        for (int i = 0; i < objects_Arr.Length; i++)
            objects.Add(objects_Arr[i]);

        if (light.enabled)
        {
            savedObjects = new GameObject[objects.Count];
            for (int i = 0; i < objects.Count; i++)
            {
                savedObjects[i] = objects[i].gameObject;
                savedObjects[i].GetComponent<SpriteRenderer>().enabled = true;
                savedObjects[i].GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }
}
