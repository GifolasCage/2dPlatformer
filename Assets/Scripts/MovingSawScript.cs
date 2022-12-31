using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSawScript : MonoBehaviour
{
    [SerializeField] private Transform pos1, pos2, sawBlade;
    [SerializeField] private float moveSpeed, rotationSpeed;
    [SerializeField] private LineRenderer lineRenderer;
    Vector3 nextPos;
    private bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
        sawBlade.position = pos1.position;
        nextPos = pos2.position;

        lineRenderer = GetComponent<LineRenderer>();
        DrawLine();
    }

    // Update is called once per frame
    void Update()
    {
        if(sawBlade.position == pos1.position){
            nextPos = pos2.position;
        }
        if(sawBlade.position == pos2.position){
            nextPos = pos1.position;
        }
        if(isMoving){
        sawBlade.position = Vector3.MoveTowards(sawBlade.position,nextPos,moveSpeed*Time.deltaTime);
        sawBlade.Rotate(new Vector3(0,0,rotationSpeed * Time.deltaTime));
        }
    }

    private void DrawLine(){
        lineRenderer.SetPosition(0, pos1.position);
        lineRenderer.SetPosition(1, pos2.position);
    }

}
