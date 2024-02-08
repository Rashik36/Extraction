using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
  private Achievements achievements;
    // Start is called before the first frame update
    void Start()
    {
      achievements = GameObject.Find("Achievements").GetComponent<Achievements>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
 
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
          achievements.LevelEndTrigger();
          StartCoroutine(Main.Instance.web.SetPlayerXP("500",GlobalPlayer.userID.ToString()));
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
          StartCoroutine(Main.Instance.web.SetLevel(GlobalPlayer.userID));
          GlobalPlayer.level += 1; 
        }

    }
}
