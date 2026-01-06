using UnityEngine;

public class Segment_Trigger : MonoBehaviour
{
    public Background_Poolmanager pool;
    private bool triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTriggered() => triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        triggered = true;
        if (collision.CompareTag("Player"))
        {
            pool.MovePreviousSegment();
        }
    }
}
