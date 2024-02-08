using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLoadOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Main.Instance.web.GetLoadOutGun(GlobalPlayer.userID.ToString()));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
