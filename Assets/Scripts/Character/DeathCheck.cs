using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathCheck : MonoBehaviour
{
    
    public Transform deathPoint;
    public GameObject restartCheckPointPanel;
    public TMP_Text livesText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Should Play : " + GameManager.Instance.shouldPlay);
        if (!GameManager.Instance.shouldPlay)
            return;

        if (transform.position.y < deathPoint.position.y)
        {
            if (GetComponent<Character>().currentLives <= 0 || GetComponent<Character>().checkPointsCrossed <= 0)
            {
                GameManager.Instance.InitLevel();
                Debug.Log("Should Init Level");
            }
            else
            {
                GetComponent<Character>().currentLives--;
                RestartCheckPoint();

                Debug.Log("Should Restart Checkpoint");
            }
        }
    }


    public void RestartCheckPoint()
    {
        
        restartCheckPointPanel.SetActive(true);
        GetComponent<Character>().disableInput = true;
        livesText.text = "Player has Lives : " + GetComponent<Character>().currentLives.ToString();
        // Debug.Log("Should Stop Playing!");
        GameManager.Instance.shouldPlay = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void ContinueCheckpoint()
    {
        restartCheckPointPanel.SetActive(false);
        // Debug.Log("Should Continue Playing!");
        
        GameManager.Instance.LoadGame();
        GameManager.Instance.shouldPlay = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Time.timeScale = 1;
        
    }


}
