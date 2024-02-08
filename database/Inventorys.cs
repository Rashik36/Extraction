using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using TMPro;

public class Inventorys : MonoBehaviour
{
    Action<string> _createItemsCallback;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ReloadInventory(){
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        _createItemsCallback = (jsonArrayString) => {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };

        CreateItems(); 
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void CreateItems(){
        StartCoroutine(Main.Instance.web.GetItem(_createItemsCallback));
    }

    IEnumerator CreateItemsRoutine(string jsonArrayString){
        bool isDone = false;
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        JSONArray purchaseArray = new JSONArray();

        Action<string> getPurchaseCallback = (userPurchase) => {
            JSONArray tempArray = JSON.Parse(userPurchase) as JSONArray;
            purchaseArray = tempArray;
            isDone = true;

        };

        StartCoroutine(Main.Instance.web.getUserPurchase(GlobalPlayer.userID, getPurchaseCallback));

        yield return new WaitUntil(() => isDone == true);
        for (int i = 0; i < jsonArray.Count; i++){
            JSONObject itemInfoJson = new JSONObject();
            string itemID = jsonArray[i].AsObject["GunID"];
            itemInfoJson = jsonArray[i].AsObject;
            
            // If game item already purchased

            if(purchaseArray == null){
                Debug.Log("no purchase");
            } else {
                for(int j = 0; j <purchaseArray.Count; j++){

                    JSONObject purchaseInfoJson = new JSONObject();
                    purchaseInfoJson = purchaseArray[j].AsObject;
                    
                    if(itemInfoJson["GunID"] == purchaseInfoJson["gunID"]){
                        string equipItemID = jsonArray[i].AsObject["GunID"];
                        GameObject item = Instantiate(Resources.Load("purchasedGun") as GameObject);
                        item.transform.SetParent(this.transform);
                        item.transform.localScale = Vector3.one;
                        item.transform.localPosition = Vector3.zero;

                        item.transform.Find("Equip").GetComponent<Button>().onClick.AddListener(() => {
                            Gun.gunID = int.Parse(itemInfoJson["GunID"]);
                            Gun.damage = int.Parse(itemInfoJson["Damage"]);
                            Gun.fireRate = int.Parse(itemInfoJson["FireRate"]);
                            Gun.magSize = int.Parse(itemInfoJson["MagSize"]);
                            StartCoroutine(Main.Instance.web.SetLoadOut(GlobalPlayer.userID.ToString(),equipItemID));
                            ReloadInventory();
                        });

                        item.transform.Find("Name").GetComponent<TMP_Text>().text = itemInfoJson["Name"];
                        item.transform.Find("Damage").GetComponent<TMP_Text>().text = itemInfoJson["Damage"];
                        item.transform.Find("MagSize").GetComponent<TMP_Text>().text = itemInfoJson["MagSize"];
                        item.transform.Find("FireRate").GetComponent<TMP_Text>().text = itemInfoJson["FireRate"];
                        item.transform.Find("Description").GetComponent<TMP_Text>().text = itemInfoJson["Description"]; 
                        if(Gun.gunID == int.Parse(itemInfoJson["GunID"])){
                            GameObject equip = item.transform.Find("Equiped").gameObject;
                            GameObject equipButton = item.transform.Find("Equip").gameObject;
                            equip.SetActive(true);
                            equipButton.SetActive(false);
                        }  
                    }
                }
            }

         
        }
         
    }

}
