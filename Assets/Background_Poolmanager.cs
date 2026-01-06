using System.Collections.Generic;
using UnityEngine;

public class Background_Poolmanager : MonoBehaviour
{
    public GameObject startSegement;
    public List<GameObject> segments;

    private int segmentCount;
    private float spawnPosY;
    private int currentSeg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSeg = 0;
        segmentCount = segments.Count;
        spawnPosY = startSegement.transform.position.y + (startSegement.GetComponent<SpriteRenderer>().bounds.size.y + segments[0].GetComponent<SpriteRenderer>().bounds.size.y) / 2;

        for (int i = 0; i < segments.Count; i++)
        {
            GameObject instance = Instantiate(segments[i], new Vector3(startSegement.transform.position.x, spawnPosY, startSegement.transform.position.z), Quaternion.identity);
            instance.GetComponent<Segment_Trigger>().pool = this;

            segments[i] = instance;
            spawnPosY += segments[i].GetComponent<SpriteRenderer>().bounds.size.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePreviousSegment()
    {
        currentSeg++;

        if (currentSeg < segmentCount) return;

        //剰余演算はループ実現するため
        segments[currentSeg % segmentCount].transform.position = new Vector3(segments[currentSeg % segmentCount].transform.position.x, spawnPosY, segments[currentSeg % segmentCount].transform.position.z);
        //
        segments[currentSeg % segmentCount].GetComponent<Segment_Trigger>().SetTriggered();

        spawnPosY += segments[currentSeg % segmentCount].GetComponent<SpriteRenderer>().bounds.size.y;
    }
}

//lossyScale.yかSpriteRenderer.bounds.size.y/Renderer.bounds.size.yか使用すること -> 要検討