using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Main.Instance.web.GetEnemyHealth(GlobalPlayer.userID));
        
    }


}
