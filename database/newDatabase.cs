using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using SimpleJSON;
using static Models;

public class newDatabase : MonoBehaviour
{
    public string loginMessage = " ";
    public string registerMessage = " ";
    public string test = " ";
    void Start()
    {
        // A correct website page.
        //StartCoroutine(GetUser("http://localhost/ExtractionBackend/getPlayers.php"));
        //StartCoroutine(Login("rashik","rashik"));
        //StartCoroutine(Register("puskal","puskal"));
        // StartCoroutine(GetItem());
    }

    IEnumerator GetUser(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator Login(string name, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginName", name);
        form.AddField("loginPassword", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                loginMessage = www.downloadHandler.text;
                if(loginMessage.Contains("Wrong Password") || loginMessage.Contains("Username doesn't exit")){
                    Debug.Log("incorrect");
                } else{
                    string jsonArrayString = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

                    for (int i = 0; i < jsonArray.Count; i++){
                        JSONObject itemInfoJson = new JSONObject();

                        itemInfoJson = jsonArray[i].AsObject;
                            GlobalPlayer.userID = int.Parse(itemInfoJson["playerID"]);
                            GlobalPlayer.level = int.Parse(itemInfoJson["LevelID"]) + 1;
                            if(itemInfoJson["rankID"] == null){
                                GlobalPlayer.rankID = 0;
                            } else{
                                GlobalPlayer.rankID = int.Parse(itemInfoJson["rankID"]);
                            }
                            
                            GlobalPlayer.cash = int.Parse(itemInfoJson["cash"]);
                            GlobalPlayer.XP = int.Parse(itemInfoJson["XP"]);
                            GlobalPlayer.userName = itemInfoJson["name"];

                    }
                    
                    if(GlobalPlayer.level > 0){
                        SceneManager.LoadScene("Menu");
                    }

                }

                
            }
        }
    }

    public IEnumerator SetLevel(int userID)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/setPlayerLevel.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                test = www.downloadHandler.text;
            }
        }

    }

    public IEnumerator SetLevelBegin(int userID)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/setPlayerLevelBegin.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                test = www.downloadHandler.text;
            }
        }

    }


    
    public IEnumerator Register(string name, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginName", name);
        form.AddField("loginPassword", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                registerMessage = www.downloadHandler.text;
            }
        }
    }

    public IEnumerator FillRegister(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginName", name);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/fillRegister.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetItem(System.Action<string> callback)
    {
       using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/ExtractionBackend/getItems.php"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);

            }
        }
    }

    public IEnumerator getUserPurchase(int userID, System.Action<string> callback)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getUserPurchase.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);
            }
        }

    }

    public IEnumerator GetItemIcon(string itemID, System.Action<Sprite> callback)
    {

        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getItemIcon.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;
                Texture2D texture = new Texture2D(2,2);
                texture.LoadImage(bytes);

                Sprite sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), new Vector2(0.5f,0.5f));

                callback(sprite);
            }
        }
    }

    public IEnumerator BuyItem(string itemID, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);
        form.AddField("userID", userID);

       using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/buyItem.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

            }
        }
    }

    public IEnumerator GetItemPrice(string itemID, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);

       using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getItemPrice.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                callback(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator SetLoadOut(string userID, string itemID)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        form.AddField("itemID", itemID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/setLoadOut.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }

    }

    public IEnumerator GetLoadOutGun(string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

       using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getLoadOutGun.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string jsonArrayString = www.downloadHandler.text;
                JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

                for (int i = 0; i < jsonArray.Count; i++){
                    JSONObject itemInfoJson = new JSONObject();

                    itemInfoJson = jsonArray[i].AsObject;
                    Gun.gunID = int.Parse(itemInfoJson["GunID"]);
                    Gun.damage = int.Parse(itemInfoJson["Damage"]);
                    Gun.fireRate = int.Parse(itemInfoJson["FireRate"]);
                    Gun.magSize = int.Parse(itemInfoJson["MagSize"]);
                    }
            }
        }

    }

    public IEnumerator GetAchievement(System.Action<string> callback)
    {
       using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/ExtractionBackend/getAchievement.php"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);

            }
        }
    }

    public IEnumerator GetUserAchievement(int userID, System.Action<string> callback)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getUserAchievement.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);
            }
        }

    }

    public IEnumerator SetPlayerAchievement(string achievementID, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("achievementID", achievementID);
        form.AddField("userID", userID);

       using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/setPlayerAchievement.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

            }
        }
    }

    public IEnumerator SetPlayerXP(string XP, string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("XP", XP);
        form.AddField("userID", userID);

       using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/setXP.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

            }
        }
    }

    public IEnumerator GetCash(int userID)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getCash.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                GlobalPlayer.cash = int.Parse(www.downloadHandler.text);
            }
        }

    }

    public IEnumerator GetXP(int userID)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getXP.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                GlobalPlayer.XP = int.Parse(www.downloadHandler.text);
            }
        }

    }

    public IEnumerator GetRankDetails(int userID, System.Action<string> callback)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getRankDetails.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;
                callback(jsonArray);
            }
        }

    }

    public IEnumerator GetEnemyHealth(int userID)
    {

        WWWForm form = new WWWForm();
        form.AddField("userID", userID.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/ExtractionBackend/getEnemyHealth.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                GlobalEnemy.enemyHealth = int.Parse(www.downloadHandler.text);
            }
        }

    }
}
