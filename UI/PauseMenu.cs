using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settings;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                if(settings != null){
                    if(settings.activeSelf){
                        settings.SetActive(false);
                        Pause();
                    }else{
                        Resume();
                    }
                } else{
                    Resume();
                }
                
            } else{
                Pause();
            }
        }
    }

    public void Resume(){
        if(SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "Login" && SceneManager.GetActiveScene().name != "Register"){
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }


    void Pause(){
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void LoadSetting(){
        pauseMenuUI.SetActive(false);
        settings.SetActive(true);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
