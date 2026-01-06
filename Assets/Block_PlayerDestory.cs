using UnityEngine;

public class Block_PlayerDestory : MonoBehaviour
{
    private Gameplay_Manager gameplayManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameplayManager = GameObject.FindWithTag("GameController").GetComponent<Gameplay_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameplayManager.Gameover();
        }
    }
}
