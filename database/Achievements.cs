using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    private int totalDeathEnemies = 0;
    private bool isLevelEnd = false;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(totalDeathEnemies >= 4 && AchievementStatus.isAchievement01Completed == false){
            AchievementStatus.isAchievement01Completed = true;
            StartCoroutine(Main.Instance.web.SetPlayerAchievement("1",GlobalPlayer.userID.ToString()));
        }

        if(isLevelEnd == true && AchievementStatus.isAchievement02Completed == false){
            AchievementStatus.isAchievement02Completed = true;
            StartCoroutine(Main.Instance.web.SetPlayerAchievement("2",GlobalPlayer.userID.ToString()));
        }

        if(totalDeathEnemies >= 7 && AchievementStatus.isAchievement03Completed == false){
            AchievementStatus.isAchievement03Completed = true;
            StartCoroutine(Main.Instance.web.SetPlayerAchievement("3",GlobalPlayer.userID.ToString()));
        }

        if(isLevelEnd == true && totalDeathEnemies <= 0 && AchievementStatus.isAchievement04Completed == false){
            AchievementStatus.isAchievement04Completed = true;
            StartCoroutine(Main.Instance.web.SetPlayerAchievement("4",GlobalPlayer.userID.ToString()));
        }

        
    }

    public void DeadEnemiesCount(){
        int deadEnemies = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach(GameObject enemy in enemies){
            if(enemy.GetComponent<Enemy>().isActiveAndEnabled == false){
                deadEnemies += 1;
            }
        }
        totalDeathEnemies = deadEnemies;
    }

    public void LevelEndTrigger(){
        isLevelEnd = true;
    }
}
