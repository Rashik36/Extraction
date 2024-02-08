using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using TMPro;

public class Items : MonoBehaviour
{
    public GameObject notEnoughCashError;
    public CashUI cashUI;
    Action<string> _createItemsCallback;
    // Start is called before the first frame update
    void Start()
    {
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

            GameObject item = Instantiate(Resources.Load("gun") as GameObject);

            
            // If game item already purchased

            if(purchaseArray == null){
                Debug.Log("no purchase");
            } else {
                for(int j = 0; j <purchaseArray.Count; j++){
                    JSONObject purchaseInfoJson = new JSONObject();
                    purchaseInfoJson = purchaseArray[j].AsObject;
                    if(itemInfoJson["GunID"] == purchaseInfoJson["gunID"]){
                        Purchased(item);
                    }
                }
            }

            item.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.transform.Find("Name").GetComponent<TMP_Text>().text = itemInfoJson["Name"];
            item.transform.Find("Damage").GetComponent<TMP_Text>().text = itemInfoJson["Damage"];
            item.transform.Find("MagSize").GetComponent<TMP_Text>().text = itemInfoJson["MagSize"];
            item.transform.Find("FireRate").GetComponent<TMP_Text>().text = itemInfoJson["FireRate"];
            item.transform.Find("Price").GetComponent<TMP_Text>().text = itemInfoJson["Price"];
            item.transform.Find("Description").GetComponent<TMP_Text>().text = itemInfoJson["Description"];

            item.transform.Find("Buy").GetComponent<Button>().onClick.AddListener(() => {
                StartCoroutine(Buy(itemID,item));
            });

            //For image loading

            // Action<Sprite> getItemIconCallback = (downloadedSprite) => {
            //     item.transform.Find("Image").GetComponent<Image>().sprite = downloadedSprite;
            // };

            // StartCoroutine(Main.Instance.web.GetItemIcon(itemInfoJson["GunID"], getItemIconCallback));

            
        }
         
    }

    private void Purchased(GameObject item){
        GameObject purchase = item.transform.Find("Purchased").gameObject;
        GameObject buyButton = item.transform.Find("Buy").gameObject;
        purchase.SetActive(true);
        buyButton.SetActive(false);
    }

    IEnumerator Buy(string itemID, GameObject item){
        string itemPrice = " ";
        bool isDone = false;
        Action<string> getItemPriceCallback = (itemPriceString) => {
            itemPrice = itemPriceString;
            isDone = true;
        };
        StartCoroutine(Main.Instance.web.GetItemPrice(itemID, getItemPriceCallback));

        yield return new WaitUntil(() => isDone == true);
        string iID = itemID;
        string uID = GlobalPlayer.userID.ToString();

        if(GlobalPlayer.cash >= int.Parse(itemPrice)){
            item.transform.Find("BoughtAudio").GetComponent<AudioSource>().Play();
            GlobalPlayer.cash = GlobalPlayer.cash - int.Parse(itemPrice);
            cashUI.UpdateUICash();
            StartCoroutine(Main.Instance.web.BuyItem(iID,uID));
            Purchased(item);
        } else{
            notEnoughCashError.SetActive(true);
        }
    }
}
