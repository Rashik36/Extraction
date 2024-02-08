using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSwap : MonoBehaviour
{
    public AudioSource normal;
    public AudioSource chasing;
    public Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {

        normal.enabled = true;
        chasing.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        enemy = GameObject.FindWithTag("enemy").GetComponent<Enemy>();
        if(enemy.isRunning || enemy.isAttacking){

            normal.enabled = false;
            chasing.enabled = true;
        }else{

            normal.enabled = true;
            chasing.enabled = false;
        }
        
    }
     
}
