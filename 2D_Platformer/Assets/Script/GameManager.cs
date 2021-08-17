using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;

    public PlayerMove player;
    public GameObject[] Stages;

    public RawImage[] UIHealth;
    public Text UIPoint;
    public Text UIStage;
    public Button RestartButton;

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        // Change stage
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else  // Game clear
        {
            // Player control lock
            Time.timeScale = 0;

            // Result UI
            Debug.Log("게임 클리어!");

            // Restart button UI
            Text restartText = RestartButton.GetComponentInChildren<Text>();
            restartText.text = "Clear!";
            RestartButton.gameObject.SetActive(true);
        }

        // Calculate point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIHealth[health].color = new Color(1, 1, 1, 0.2f);
        }
        else
        {
            // All health UI off
            UIHealth[0].color = new Color(1, 1, 1, 0.2f);

            // Player die effect
            player.OnDie();

            // Result UI
            Debug.Log("죽음");

            // Retry button UI
            RestartButton.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player reposition
            if (health > 1)
                PlayerReposition();

            // Health down
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-2f, 1.5f, 0);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
