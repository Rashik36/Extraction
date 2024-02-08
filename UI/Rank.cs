using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using SimpleJSON;

public class Rank : MonoBehaviour
{
    public TMP_Text XP;
    public TMP_Text rankID;
    public TMP_Text currentRankID;
    public Image rankBar;
    public TMP_Text cash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRank(){
        XP.text = GlobalPlayer.XP.ToString();
        StartCoroutine(CreateRankRoutine());
    }

    IEnumerator CreateRankRoutine(){
        bool isDone = false;
        JSONArray RankArray = new JSONArray();

        Action<string> _createRankCallback = (jsonArrayString) => {
            JSONArray tempArray = JSON.Parse(jsonArrayString) as JSONArray;
            RankArray = tempArray;
            isDone = true;
        };

        StartCoroutine(Main.Instance.web.GetRankDetails(GlobalPlayer.userID,_createRankCallback));

        yield return new WaitUntil(() => isDone == true);

        for (int i = 0; i < RankArray.Count; i++){
            JSONObject RankJson = new JSONObject();
            RankJson = RankArray[i].AsObject;

            rankID.text = RankJson["rankID"];
            currentRankID.text = (int.Parse(RankJson["rankID"]) - 1).ToString();
            cash.text = RankJson["cash"];
            rankBar.fillAmount = (float)GlobalPlayer.XP / float.Parse(RankJson["requriedXP"]);

        }
    }
}
