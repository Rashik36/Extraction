using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    private void Start() {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Quit(){
        Application.Quit();
    }

    public void Restart(){
        
        StartCoroutine(Main.Instance.web.SetLevelBegin(GlobalPlayer.userID));
        GlobalPlayer.level = 1; 
        SceneManager.LoadScene(2);
    }
}
