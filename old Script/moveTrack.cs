using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTrack : MonoBehaviour
{
    [SerializeField] GameObject[] movePoints;
    private int index = 0;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, movePoints[index].transform.position) < 0.1f){
            index++;
            if(index >= movePoints.Length){
                index = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoints[index].transform.position, speed * Time.deltaTime);
        
    }
}
