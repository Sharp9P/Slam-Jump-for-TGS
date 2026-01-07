using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Gameover_manager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameResultData.Instance == null)
        {
            Debug.LogError("GameResultData not found!");
            return;
        }

        scoreText.text = "Score : " + GameResultData.Instance.finalScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene("PlayScene");
        }
    }
}
