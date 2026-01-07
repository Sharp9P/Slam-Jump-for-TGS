using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using Unity.VisualScripting;

public class Player_Move : MonoBehaviour
{
    public float bulletTimeScale;// (0 ~ 1)
    private float defaultFixedDeltaTime;
    public float minSpeed;
    public float maxSpeed;
    public float maxDragPercentage;// (0 ~ 1)
    public float minLength;
    public float maxLength;
    public GameObject aimHead;

    private bool pressStartedInPlay = false;

    private Rigidbody2D rb;
    private LineRenderer lr;

    private bool isPressed;
    private Vector3 unholdMousePos;
    private Vector3 onholdMousePos;
    private Vector3 releaseMousePos;
    private Vector3 pendingVelocity;
    private bool launched;

    public float timeSuperJump;
    public float speedSuperJump;
    public float lineSuperJump;// (0 ~ 1)

    public enum PlayerStatus
    {
        None,
        SuperJump
    }

    private PlayerStatus status;
    //public event Action <PlayerStatus> onStatusChange; イベントシステムはまだ使わなくていい

    public bool IsInvincible
    {
        get
        {
            return status == PlayerStatus.SuperJump;
        }
    }

    private Animator animator;

    private Coroutine superJumpCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        status = PlayerStatus.None;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        aimHead.SetActive(false);
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (status)
        {
            case PlayerStatus.None:
                NoneRender();
                break;
            case PlayerStatus.SuperJump:
                SuperJumpRender();//今は何もしてない
                return;
                //break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (status)
        {
            case PlayerStatus.None:
                NonePhysics();
                break;
            case PlayerStatus.SuperJump:
                SuperJumpPhysics();
                break;
            default:
                break;
        }
    }

    public void GoSuperJump(PlayerStatus statusChanged, Vector3 dir)
    {
        if (status == statusChanged) return;//同じ状態の重複切り替え禁止
        if (status == PlayerStatus.SuperJump && statusChanged != PlayerStatus.None) return;//起死回生ジャンプからデフォルト状態のみ切り替えられる

        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        status = statusChanged;
        //onStatusChange?.Invoke(statusChanged);

        superJumpCoroutine = StartCoroutine(SuperJump(dir, timeSuperJump));
    }

    private void NoneRender()
    {
        animator.SetFloat("VerticalSpeed", rb.linearVelocityY);

        if (Mouse.current.leftButton.wasPressedThisFrame && IsClickOnControllable())
        {
            pressStartedInPlay = true;
            isPressed = true;

            animator.SetTrigger("wasPressed");
            unholdMousePos = Mouse.current.position.ReadValue();
        }

        if (!pressStartedInPlay) return;//他シーンからのファントム入力防止

        if (isPressed && Mouse.current.leftButton.isPressed)
        {
            onholdMousePos = Mouse.current.position.ReadValue();

            //極小操作は無視する
            if ((unholdMousePos - onholdMousePos).sqrMagnitude < 1f) return;
            //

            lr.enabled = true;
            Vector3 dir = (unholdMousePos - onholdMousePos).normalized;
            float force = (unholdMousePos - onholdMousePos).magnitude;

            //解像度違っても同じ操作感保つための機能
            float charge01 = Mathf.Clamp01(force / (Screen.height * maxDragPercentage));
            float length = Mathf.Lerp(minLength, maxLength, charge01);
            //

            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position + dir * length);

            aimHead.SetActive(true);
            aimHead.transform.position = transform.position + dir * length;
            aimHead.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        }

        if (isPressed && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            lr.enabled = false;
            aimHead.SetActive(false);
            animator.SetTrigger("wasReleased");

            releaseMousePos = Mouse.current.position.ReadValue();

            //極小操作は無視する
            if ((unholdMousePos - releaseMousePos).sqrMagnitude < 1f)
            {
                isPressed = false;
                pressStartedInPlay = false;
                return;
            }
            //

            Vector3 dir = (unholdMousePos - releaseMousePos).normalized;
            float force = (unholdMousePos - releaseMousePos).magnitude;

            if (unholdMousePos.y < Screen.height * lineSuperJump)
            {
                GoSuperJump(PlayerStatus.SuperJump, dir);
            }


            //解像度違っても同じ操作感保つための機能
            float charge01 = Mathf.Clamp01(force / (Screen.height * maxDragPercentage));
            float speed = Mathf.Lerp(minSpeed, maxSpeed, charge01);
            //

            pendingVelocity = dir * speed;
            launched = true;
            isPressed = false;
            pressStartedInPlay = false;
        }
    }

    private void NonePhysics()
    {
        if (isPressed)
        {
            Time.timeScale = bulletTimeScale;
            Time.fixedDeltaTime = defaultFixedDeltaTime * bulletTimeScale;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = defaultFixedDeltaTime;
        }

        if (launched)
        {
            lr.enabled = false;
            aimHead.SetActive(false);

            rb.linearVelocity = Vector2.zero;
            rb.linearVelocity = pendingVelocity;
            launched = false;
        }
    }

    private void SuperJumpRender()
    {
        //何もない
    }

    private void SuperJumpPhysics()
    {
        //何もない
    }

    private IEnumerator SuperJump(Vector3 dir, float duration)
    {
        float timer = 0f;

        animator.SetTrigger("isSuperJumping");

        while (timer < duration)
        {
            rb.linearVelocity = dir * speedSuperJump;

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        EndSuperJump();
    }
    private bool IsClickOnControllable()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return Physics2D.OverlapPoint(mouseWorldPos, LayerMask.GetMask("Clickable")) != null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (status == PlayerStatus.SuperJump)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                rb.linearVelocity = Vector2.zero;
                EndSuperJump();
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void EndSuperJump()
    {
        if (status != PlayerStatus.SuperJump) return;

        animator.SetTrigger("finishSuperJumping");

        status = PlayerStatus.None;

        if (superJumpCoroutine != null)
        {
            StopCoroutine(superJumpCoroutine);
            superJumpCoroutine = null;
        }
    }
}
