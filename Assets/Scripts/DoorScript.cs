using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private PlayerItemManager playerItemScript;
    private GameManager gameManagerScript;

    [Header("Door color: Red, Blue or Green")]
    [SerializeField] private string doorColor;
    // Start is called before the first frame update
    void Start()
    {
        playerItemScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItemManager>();
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player"){
            if(playerItemScript.keyColor.Contains(doorColor)){
                gameManagerScript.Win();
            }
            else{
                Debug.Log("Door is locked!");
            }
        }
    }
}
