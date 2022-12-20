using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Player"){
            col.collider.transform.SetParent(this.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D col) {
        if(col.gameObject.tag == "Player"){
            col.collider.transform.SetParent(null);
        }
    }
}
