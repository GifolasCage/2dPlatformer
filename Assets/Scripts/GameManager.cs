using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Die(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Win(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            Die();
        }
    }
}
