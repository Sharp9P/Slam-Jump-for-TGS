using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Gameplay_Manager : MonoBehaviour
{
    private Camera_Move cam;
    private Player_Move player;
    private int score;
    private TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera_Move>();
        player = GameObject.FindWithTag("Player").GetComponent<Player_Move>();
        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        score = (int)cam.maxReachedY;
        float time = Time.timeSinceLevelLoad;

        text.text = $"Score : {score}\nTime : {time:F1}";
    }

    public void Gameover()
    {
        if (!player.IsInvincible)
        {
            GameResultData.Instance.finalScore = score;
            SceneManager.LoadScene("GameoverScene");
        }
    }
}
