using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class EndGame : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.GetComponent<PlatformerCharacter2D>().Move(0f, false, false);
        player.GetComponent<PlatformerCharacter2D>().enabled = false;
        player.GetComponent<Platformer2DUserControl>().enabled = false;
        
        StartCoroutine(EndGameWait());
        //end the game when the player enters the trigger
        

    }

    IEnumerator EndGameWait()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(2);
    }
}
