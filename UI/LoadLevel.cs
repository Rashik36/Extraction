using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadLevel : MonoBehaviour
{
    public GameObject error;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void LoadScene(){
        
        if(Gun.gunID == 0){
            error.SetActive(true);
        }else{
        SceneManager.LoadScene(GlobalPlayer.level);
        StartCoroutine(Main.Instance.web.SetPlayerXP("20",GlobalPlayer.userID.ToString()));
        } 
    }

    public void UnloadError(){
        error.SetActive(false);
    }
}
