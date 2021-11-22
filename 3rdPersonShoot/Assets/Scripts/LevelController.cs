using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject player;
    public static LevelController Instance;
    bool isEnd = false;
   
    void Awake()
    {
        Instance = this;
    }

    public void LevelEnd(bool isSuccess)
    {
        if (isEnd) return;

        isEnd = true;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        if (isSuccess)
        {
            Debug.Log("Mission complete!");
        }
        
    }
    
}
