using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XPUI : MonoBehaviour
{

    void Start()
    {
        UpdateXP();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateXP(){
        StartCoroutine(Main.Instance.web.GetXP(GlobalPlayer.userID));
    }
}
