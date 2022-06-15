using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerStay(Collider other){
        this.transform.parent.position = new Vector3( this.transform.parent.position.x, this.transform.parent.position.y+1, this.transform.parent.position.z);
        //Debug.Log("collider girdi");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
