using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class navigationBG : MonoBehaviour
{
    public RectTransform target;
    public Button play;
    public float speed;
    public Button[] navButtons;
    private static Button currentButton;

    // Start is called before the first frame update
    void Start()
    {
        currentButton = play;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        ResetTextAnimation();
        
    }

    public void ButtonAnimation(Button self){
        currentButton = self;
        TMP_Text nav = self.GetComponentInChildren<TMP_Text>();
        nav.color = Color.black;
        RectTransform button = self.GetComponent<RectTransform>();
        target = button;
    }

    private void ResetTextAnimation(){
        foreach(Button button in navButtons){
            if(button != currentButton){
                TMP_Text nav = button.GetComponentInChildren<TMP_Text>();
                nav.color = Color.white;
            }
        }
    }
}
