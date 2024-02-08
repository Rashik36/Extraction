using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;
using TMPro;

public class setInGameAchievements : MonoBehaviour
{
    Action<string> _createAchievementCallback;
    // Start is called before the first frame update
    void Start()
    {

        _createAchievementCallback = (jsonArrayString) => {
            StartCoroutine(CreateAchievementRoutine(jsonArrayString));
        };
        CreateAchievement(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAchievement(){
        StartCoroutine(Main.Instance.web.GetAchievement(_createAchievementCallback));
    }

    IEnumerator CreateAchievementRoutine(string jsonArrayString){
        bool isDone = false;
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        JSONArray AchievementCompletedArray = new JSONArray();

        Action<string> getAchievementCallback = (userAchievement) => {
            JSONArray tempArray = JSON.Parse(userAchievement) as JSONArray;
            AchievementCompletedArray = tempArray;
            isDone = true;

        };

        StartCoroutine(Main.Instance.web.GetUserAchievement(GlobalPlayer.userID, getAchievementCallback));

        yield return new WaitUntil(() => isDone == true);

        for (int i = 0; i < jsonArray.Count; i++){
            JSONObject itemInfoJson = new JSONObject();
            string AchievementID = jsonArray[i].AsObject["achievement_ID"];
            itemInfoJson = jsonArray[i].AsObject;
            

            if(AchievementCompletedArray == null){
                Debug.Log("no Achievement");
            } else {
                for(int j = 0; j <AchievementCompletedArray.Count; j++){

                    JSONObject AchievementInfoJson = new JSONObject();
                    AchievementInfoJson = AchievementCompletedArray[j].AsObject;
                    
                    if(itemInfoJson["achievement_ID"] == AchievementInfoJson["achievement_ID"]){
                        switch (int.Parse(AchievementID)){
                            case 1:
                                AchievementStatus.isAchievement01Completed =true;
                                break;
                            
                            case 2:
                                AchievementStatus.isAchievement02Completed =true;
                                break;

                            case 3:
                                AchievementStatus.isAchievement03Completed =true;
                                break;

                            case 4:
                                AchievementStatus.isAchievement04Completed =true;
                                break;

                        }
 
                    }
                }
            }    
        }
    }
}
