using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{

    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
    public newDatabase database;
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(() => { 
            if(usernameInput.text != "" && passwordInput.text != ""){
                StartCoroutine(Main.Instance.web.Login(usernameInput.text,passwordInput.text)); 
            }else{
                database.loginMessage = "Please enter into the fields!";
            }
            
        });
        
    }

    private void Update() {

    }
}
