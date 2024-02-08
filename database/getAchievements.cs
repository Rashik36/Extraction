using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;

public class getAchievements : MonoBehaviour
{
    Action<string> _createAchievementCallback;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ReloadAchievement(){
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

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

            GameObject item = Instantiate(Resources.Load("Achievement") as GameObject);
            

            if(AchievementCompletedArray == null){
                Debug.Log("no Achievement");
            } else {
                for(int j = 0; j <AchievementCompletedArray.Count; j++){

                    JSONObject AchievementInfoJson = new JSONObject();
                    AchievementInfoJson = AchievementCompletedArray[j].AsObject;
                    
                    if(itemInfoJson["achievement_ID"] == AchievementInfoJson["achievement_ID"]){
                        GameObject achievementCompleted = item.transform.Find("Completed").gameObject;
                        achievementCompleted.SetActive(true);
 
                    }
                }
            }
        
            item.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.transform.Find("Description").GetComponent<TMP_Text>().text = itemInfoJson["description"];
            item.transform.Find("Price").GetComponent<TMP_Text>().text = itemInfoJson["cash"];

         
        }
    }

}
