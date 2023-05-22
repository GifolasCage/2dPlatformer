using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerStart;
    [SerializeField] private GameObject player;
    [SerializeField] private UnityEvent die;
    [SerializeField] private float restartTime = 2f;

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
            characterControllerScript.StopMoving();
            player.transform.position = playerStart.position;
        }
        else{
            characterControllerScript.StopMoving();
            player.transform.position = playerItemScript.checkpointPosition[playerItemScript.checkpointPosition.Count-1];
        }
    }

    public void Die(){
        die.Invoke();
        StartCoroutine(WaitForRestart());
    }

    IEnumerator WaitForRestart(){
        yield return new WaitForSeconds(restartTime);
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
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }
}
