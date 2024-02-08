using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughCashError : MonoBehaviour
{
    public GameObject notEnoughCashError;

    public void ErrorScreenOff(){
        notEnoughCashError.SetActive(false);
    }
}
