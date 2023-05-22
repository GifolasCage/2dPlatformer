using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helloscript : MonoBehaviour
{
    private GameManager gameManagerScript;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        gameManagerScript.Die();
    }
}
