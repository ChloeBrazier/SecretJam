using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    private Collider2D[] objects;
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
        objects = new Collider2D[0];

        prevState = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        objects = Physics2D.OverlapCircleAll((Vector2)transform.position, effectRadius, layerMask);
        if (light.enabled)
        {
            savedObjects = new GameObject[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                savedObjects[i] = objects[i].gameObject;
                savedObjects[i].SetActive(true);
            }
        }
    }
}
