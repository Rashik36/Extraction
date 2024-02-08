using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flame : MonoBehaviour
{
    public GameObject flameObj;
    private bool isFlameActive;
    public float flameTime;
    public float noFlameTime;
    private float currentFlameTime;
    public float range;
    public float damageRate;
    private float nextTimeToDamage = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentFlameTime = flameTime;

    }

    // Update is called once per frame
    void Update()
    {

        if(currentFlameTime  > 0){
            flameObj.SetActive(true);
            isFlameActive = true;
            currentFlameTime -= Time.deltaTime;
        }else{
            flameObj.SetActive(false);
            isFlameActive = false;
            Invoke("reset", noFlameTime);
        }

        DamagePlayer();
        
    }

    void reset(){
        currentFlameTime = flameTime;
    }

    void DamagePlayer(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range))
        {

            PlayerController player = hit.transform.GetComponent<PlayerController>();
            if(player != null){
                if(Time.time >= nextTimeToDamage && isFlameActive){
                    nextTimeToDamage = Time.time + 1f/damageRate;
                    player.GotHurt(2);
                }
                

            }
        }
    }
}
