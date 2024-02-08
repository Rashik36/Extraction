using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterMessage : MonoBehaviour
{
    private Text registerMessage;
    public newDatabase database;

    private void Start() {
        registerMessage = GetComponent<Text>();
    }

    private void Update() {
        registerMessage.text = database.registerMessage;
    }
}
