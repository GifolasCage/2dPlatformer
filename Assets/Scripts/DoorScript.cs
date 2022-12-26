using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private PlayerItemManager playerItemScript;
    private GameObject [] keyObject;
    private GameManager gameManagerScript;
    private BoxCollider2D doorCollider;
    private List<KeyScript> keyScript = new List<KeyScript>();

    private SpriteRenderer spriteRenderer;

    [Header("Door color: Red, Blue or Green")]
    [SerializeField] private string doorColor;
    [SerializeField] private Sprite[] doorSprite;


    void Start()
    {
        playerItemScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItemManager>();
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        keyObject = GameObject.FindGameObjectsWithTag("Key");
        doorCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(doorColor == "Red"){
            spriteRenderer.sprite = doorSprite[0];
        }
        else if(doorColor == "Blue"){
            spriteRenderer.sprite = doorSprite[1];
        }
        else if(doorColor == "Green"){
            spriteRenderer.sprite = doorSprite[2];
        }
        else{
            Debug.Log("Cant change sprite!");
        }

        //Find all the keys in the scene and add them to a list.
        for (int i = 0; i < keyObject.Length; i++)
        {
            keyScript.Add(keyObject[i].GetComponent<KeyScript>());
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player"){
            OpenDoor();
        }
    }

    private void OpenDoor(){
            if(playerItemScript.keyColor.Contains(doorColor)){
                gameManagerScript.doorsTaken += 1;
                doorCollider.enabled = false;
                gameManagerScript.CheckDoors();

                //Loop through all the keys and remove the oen that corresponds with the current door;
                foreach (KeyScript key in keyScript)
                {
                    key.DestroyKey(doorColor);
                }

                spriteRenderer.sprite = doorSprite[3];
            }
            else{
                Debug.Log("Door is locked!");
            }
    }
}
