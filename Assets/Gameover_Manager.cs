using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Gameover_manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
