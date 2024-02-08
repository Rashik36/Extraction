using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIRedirectRegister : MonoBehaviour
{
    public Button registerRedirect;
    // Start is called before the first frame update
    void Start()
    {
        registerRedirect.onClick.AddListener(() => {
            SceneManager.LoadScene("Register");
        });
        
    }
}
