using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelActive : MonoBehaviour
{
    public GameObject[] panels;
    public Button currentButton;
    public Button play;
    public AudioSource navChangeSound;
    // Start is called before the first frame update
    void Start()
    {
        currentButton = play;
        ButtonChange();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void ButtonPanel(Button self){
        navChangeSound.Play();
        currentButton = self;
        ButtonChange();

    }

    private void ButtonChange(){
        foreach(GameObject panel in panels){
            if(currentButton.name == panel.name){
                panel.SetActive(true);
            } else{
                panel.SetActive(false);
            }
        }
    }
}
