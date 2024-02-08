using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField confirmPassword;
    public Button registerButton;
    public newDatabase database;
    // Start is called before the first frame update
    void Start()
    {
        registerButton.onClick.AddListener(() => {
            if(usernameInput.text != "" && passwordInput.text != "" && confirmPassword.text != ""){
                if(passwordInput.text == confirmPassword.text){
                    StartCoroutine(Main.Instance.web.Register(usernameInput.text,passwordInput.text));
                    //StartCoroutine(Main.Instance.web.FillRegister(usernameInput.text));
                } else{
                    database.registerMessage = "Incorrect password please enter again.";
                }
            }else{
                database.registerMessage = "Please enter into the fields!";
            }


        });
        
    }

    private void Update() {
        if(database.registerMessage == "Success"){
            SceneManager.LoadScene("Login");
        }
    }
}
