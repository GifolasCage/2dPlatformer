using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private GameObject [] cameras;

    public CinemachineVirtualCamera activeCamera = null;
    // Start is called before the first frame update
    void Start()
    {
        cameras = GameObject.FindGameObjectsWithTag("CMCamera");   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(cameras.Length);
        }
    }

    public void ChangeCamera(GameObject camera){
        foreach (GameObject cam in cameras){
            cam.GetComponent<CinemachineVirtualCamera>().Priority = 10;
        }
        camera.GetComponent<CinemachineVirtualCamera>().Priority = 20;
    }
}
