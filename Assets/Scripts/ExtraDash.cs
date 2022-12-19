using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraDash : MonoBehaviour
{
    private CharacterController playerScript;
    private CircleCollider2D myCollider;
    private SpriteRenderer myRenderer;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] ParticleSystem glow;

    // Start is called before the first frame update
    void Awake() {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<CircleCollider2D>();
    }
 

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        playerScript.dashCount += 1;
        myRenderer.enabled = false;
        myCollider.enabled = false;
        glow.Stop();
        explosion.Play();
        Destroy(gameObject,2f);
    }
}
