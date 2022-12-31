using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pos1, pos2, platform;
    [SerializeField] private float speed;
    Vector3 nextPos;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        platform.position = pos1.position;
        nextPos = pos2.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(platform.position == pos1.position){
            nextPos = pos2.position;
        }
        if(platform.position == pos2.position){
            nextPos = pos1.position;
        }
        if(isMoving){
        platform.position = Vector3.MoveTowards(platform.position,nextPos,speed*Time.deltaTime);
        }
    }

    public void StartMoving(){
        isMoving = true;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(pos1.position,pos2.position);
    }
}
