using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private float pos1;
    private float pos2;
    private Vector3 nextPosition;
    private bool isTaken;
    private Transform playerTransform;
    private PlayerItemManager playerItemScript;
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] private Vector3 playerFollowOffset;
    [SerializeField] private Vector3 keyScaleAfterTaken;
    [Header("Keycolor: Red, Blue or Green")]
    [SerializeField] private string keyColor;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerItemScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItemManager>();

        pos1 = transform.position.y + 0.1f;
        pos2 = transform.position.y - 0.1f;
        nextPosition = new Vector3(transform.position.x, pos2, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Animate the key if the player has not taken it.
        if(!isTaken){
            if(transform.position.y == pos1){
                nextPosition = new Vector3(transform.position.x, pos2, transform.position.z);
            }
            if(transform.position.y == pos2){
                nextPosition = new Vector3(transform.position.x, pos1, transform.position.z);
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, 0.5f * Time.deltaTime);
        }
        if(isTaken){
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position + playerFollowOffset, moveSpeed * Time.deltaTime);
            transform.localScale = keyScaleAfterTaken;
        }

    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player"){
            isTaken = true;
            playerItemScript.keyColor.Add(keyColor);
        }
    }

    public void DestroyKey(string color){
        if(color == keyColor){
            Destroy(gameObject);
        }
    }
}
