using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WelcomeText : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        TMP_Text welcome = GetComponent<TMP_Text>();
        welcome.text = "Welcome "+GlobalPlayer.userName+" !";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
