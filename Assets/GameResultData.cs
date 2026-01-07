using UnityEngine;

public class GameResultData : MonoBehaviour
{
    public static GameResultData Instance;

    public int finalScore;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
