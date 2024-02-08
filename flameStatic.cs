using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameStatic : MonoBehaviour
{
    public GameObject flameObj;
    public float range;
    public float damageRate;
    private float nextTimeToDamage = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DamagePlayer();
        
    }

    void DamagePlayer(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, range))
        {

            PlayerController player = hit.transform.GetComponent<PlayerController>();
            if(player != null){
                if(Time.time >= nextTimeToDamage){
                    nextTimeToDamage = Time.time + 1f/damageRate;
                    player.GotHurt(2);
                }
                

            }
        }
    }
}
