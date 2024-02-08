using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashUI : MonoBehaviour
{
    public TMP_Text cash;

    void Start()
    {
        UpdateUserCash();
        StartCoroutine(UpdateUICashStart());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUICash(){

        cash.text = GlobalPlayer.cash.ToString();
    }

    IEnumerator UpdateUICashStart(){
        yield return new WaitForSeconds(2f);
        cash.text = GlobalPlayer.cash.ToString();
    }

    private void UpdateUserCash(){
        StartCoroutine(Main.Instance.web.GetCash(GlobalPlayer.userID));
    }
}
