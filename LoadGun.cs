using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGun : MonoBehaviour
{
    public GameObject[] guns;
    public ParticleSystem muzzleFlash;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetGun(){
        foreach(GameObject gun in guns){
            if(Gun.gunID == int.Parse(gun.name)){
                gun.SetActive(true);
                muzzleFlash = gun.transform.Find("gun").Find("muzzleFlash").GetComponent<ParticleSystem>();
            } else {
                gun.SetActive(false);
            }
        }
    }
}
