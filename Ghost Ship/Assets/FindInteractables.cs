using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindInteractables : MonoBehaviour
{
    private Object[] all;

    public struct DefaultVals
    {
        public bool sprite;
        public bool trigger;

        public DefaultVals(bool sprite, bool trigger)
        {
            this.sprite = sprite;
            this.trigger = trigger;
        }
    }

    public Dictionary<string, DefaultVals> defaultVals;

    // Start is called before the first frame update
    void Start()
    {
        defaultVals = new Dictionary<string, DefaultVals>();
        all = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        int layer = 8;
        for (int i = 0; i < all.Length; i++)
        {
            GameObject g = (GameObject)all[i];
            if (g.layer == layer)
            {
                if (defaultVals.ContainsKey(g.name))
                {
                    g.name += "i";
                }
                defaultVals.Add(g.name, new DefaultVals(g.GetComponent<SpriteRenderer>().enabled, g.GetComponent<BoxCollider2D>().isTrigger));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
