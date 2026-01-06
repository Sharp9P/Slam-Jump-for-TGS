using UnityEngine;

public class SuperJump_Trigger : MonoBehaviour
{
    private Player_Move player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player_Move>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TryChangeStatus(Player_Move.PlayerStatus.SuperJump);
        }
    }
}
