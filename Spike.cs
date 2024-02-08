using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public Transform spikes;
    public Transform startPoint;
    public Transform endPoint;
    public float speed;
    public bool istriggered = false;
    private bool box = false;
    public AudioSource spikeOnAudio;
    public AudioSource spikeOffAudio;

    // Start is called before the first frame update
    void Start()
    {
        spikes.transform.position = endPoint.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(istriggered){
            spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, startPoint.transform.position,speed * Time.deltaTime);
        } else{
            spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, endPoint.transform.position,speed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" ){
            spikeOffAudio.Play();
            istriggered = true;
            
        }
        if(other.tag == "Box"){
            spikeOffAudio.Play();
            istriggered = true;
            box = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            if(!box){
                spikeOnAudio.Play();
                istriggered = false;
            }

        }
        if(other.tag == "Box"){
            spikeOnAudio.Play();
            istriggered = false;
            box = false;
        }
    }


}
