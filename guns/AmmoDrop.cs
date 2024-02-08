using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    public GameObject ammoDrop;
    public Enemy enemy;
    public WeaponController weapon;
    public AudioSource ammoAudio;
    // Start is called before the first frame update
    void Start()
    {
        ammoDrop.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.enabled == false){
            ammoDrop.SetActive(true);
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        weapon.currentAmmo = 100;
        weapon.ammoCount.text = weapon.currentAmmo.ToString();
        ammoAudio.Play();
        ammoDrop.SetActive(false);
        GameObject.Destroy(this);
    }
}
