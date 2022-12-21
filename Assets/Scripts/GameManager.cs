using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerStart;
    [SerializeField] private GameObject player;

    private PlayerItemManager playerItemScript;
    private CharacterController characterControllerScript;
    private GameObject[] doors;
    public int doorsTaken;

    private void Start() {
        playerItemScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItemManager>();
        characterControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        doors = GameObject.FindGameObjectsWithTag("Door");

        SetUpGame();
    }

    private void SetUpGame()
    {
        if(playerItemScript.checkpointPosition.Count == 0){
            player.transform.position = playerStart.position;
            characterControllerScript.StopMoving();
        }
        else{
            player.transform.position = playerItemScript.checkpointPosition[playerItemScript.checkpointPosition.Count-1];
            characterControllerScript.StopMoving();
        }
    }

    public void Die(){
        SetUpGame();
    }
    public void Win(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CheckDoors(){
        if(doorsTaken == doors.Length){
            Win();
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            Die();
        }
    }
}
