using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMessage : MonoBehaviour
{
    private Text loginMessage;
    public newDatabase database;

    private void Start() {
        loginMessage = GetComponent<Text>();
    }

    private void Update() {
        loginMessage.text = database.loginMessage;
    }
}
