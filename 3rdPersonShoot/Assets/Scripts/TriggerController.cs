using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && gameObject.tag == "FinishLine")
        {
            LevelController.Instance.LevelEnd(true);
        }
    }
}
