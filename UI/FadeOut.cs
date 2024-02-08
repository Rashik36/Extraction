using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Text>() != null && GetComponent<Text>().color.a > 0f){
            Invoke("TextFade", 1f);
        }
        
        if(GetComponent<Image>() != null && GetComponent<Image>().color.a > 0f){
            Invoke("ImageFade", 1f);
        }

        
    }

    private void TextFade(){
        var color = GetComponent<Text>().color;
        color.a -= 0.8f * Time.deltaTime;
        GetComponent<Text>().color = color;
    }

    private void ImageFade(){
        var color = GetComponent<Image>().color;
        color.a -= 0.8f * Time.deltaTime;
        GetComponent<Image>().color = color; 
    }
}
