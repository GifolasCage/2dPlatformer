using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformCollision : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlatform;
    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Player"){
            col.collider.transform.SetParent(this.transform);
            onPlatform.Invoke();
            characterController.ChangeInterp();
        }
    }
    private void OnCollisionExit2D(Collision2D col) {
        if(col.gameObject.tag == "Player"){
            col.collider.transform.SetParent(null);
            characterController.ChangeInterp();
        }
    }
}
