using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private PlayerItemManager playerItemScript;
    private bool isTaken = false;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem checkpointParticles;
    [SerializeField] private Sprite checkpointTakenSprite;
   // Start is called before the first frame update
    void Start()
    {
         playerItemScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItemManager>();
         boxCollider = GetComponent<BoxCollider2D>(); 
         spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player"){
            boxCollider.enabled = false;
            spriteRenderer.sprite = checkpointTakenSprite;
            playerItemScript.checkpointPosition.Add(transform.position);
            checkpointParticles.Play();
        }
    }
}
