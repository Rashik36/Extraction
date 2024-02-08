using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIRedirectLogin : MonoBehaviour
{
    public Button loginRedirect;
    // Start is called before the first frame update
    void Start()
    {
        loginRedirect.onClick.AddListener(() => {
            SceneManager.LoadScene("Login");
        });
        
    }
}
