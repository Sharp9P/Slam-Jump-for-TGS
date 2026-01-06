using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera_Move : MonoBehaviour
{
    private Camera cam;
    private GameObject player;
    private Gameplay_Manager gameplayManager;
    public float maxReachedY { get; private set; }

    public float smoothTime;
    public float followTriggerY;// (0 ~ 1)
    public float offsetY;
    public float deadlineY;

    private Vector3 velocity = Vector3.zero;

    //public float blendRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        player = GameObject.FindWithTag("Player");
        gameplayManager = GameObject.FindWithTag("GameController").GetComponent<Gameplay_Manager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (player == null) return;

        if (player.transform.position.y > maxReachedY) maxReachedY = player.transform.position.y;

        float playerViewportPosY = cam.WorldToViewportPoint(player.transform.position).y;

        if (playerViewportPosY > followTriggerY)
        {
            /*チャッピーによるオプティマイズ -> ダメそう
            float t = Mathf.InverseLerp(followTriggerY, followTriggerY + blendRange, playerViewportPosY);
            float targetY = Mathf.Lerp(transform.position.y, maxReachedY + offsetY, t);
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), ref velocity, smoothTime);
            */

            Vector3 targetPos = new Vector3(transform.position.x, maxReachedY + offsetY, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }

        if (playerViewportPosY < deadlineY) gameplayManager.Gameover();
    }
}

//カメラとプレイヤーの最大距離 maxDistance = playerMaxSpeed * smoothTime - offsetY (近似値)