using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Collider[] objects;
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
        objects = new Collider[0];

        prevState = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        objects = Physics.OverlapSphere(transform.position, effectRadius, layerMask);
        if (light.enabled && prevState)
        {
            for (int i = 0; i < savedObjects.Length; i++)
            {
                savedObjects[i].SetActive(true);
            }
            savedObjects = new GameObject[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                savedObjects[i] = objects[i].gameObject;
                objects[i].gameObject.SetActive(false);
            }
        }
    }
}
