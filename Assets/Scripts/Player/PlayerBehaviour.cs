using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Properties")]
    public float horizontalForce;
    public float horizontalSpeed;
    public float verticalForce;
    public float airFactor;
    public Transform groundPoint;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isGrounded;

    [Header("Animations")]
    public Animator animator;
    public PlayerAnimationState playerAnimationState;

    [Header("Dust Trail")]
    public ParticleSystem dustTrail;
    public Color dustTrailColor;

    [Header("Screen Shake Properties")]
    public CinemachineVirtualCamera playerCamera;
    public CinemachineBasicMultiChannelPerlin perlin;
    public float shakeIntensity;
    public float shakeDuration;
    public float shakeTimer;
    public bool isCameraShaking;

    [Header("Health System")]
    public HealthBarController health;
    public LifeCounterController life;
    public DeathPlaneController deathPlane;

    [Header("Controls")]
    public Joystick leftStick;
    [Range(0.1f, 1.0f)]
    public float verticalThreshold;

    private Rigidbody2D rigidbody2D;
    private SoundManager soundManager;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = FindObjectOfType<PlayerHealth>().GetComponent<HealthBarController>();
        life = FindObjectOfType<LifeCounterController>();
        health.value = 3;
        deathPlane = FindObjectOfType<DeathPlaneController>();
        soundManager = FindObjectOfType<SoundManager>();
        leftStick = (Application.isMobilePlatform) ? GameObject.Find("LeftStick").GetComponent<Joystick>() : null;
        dustTrail = GetComponentInChildren<ParticleSystem>();
        playerCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        perlin = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        isCameraShaking = false;
        shakeTimer = shakeDuration;
    }

    private void Update()
    {
        if (health.value <= 0)
        {
            life.UpdateLife(-1);

            if (life.value > 0)
            {
                health.ResetHealth();
                deathPlane.ReSpawn(gameObject);
                soundManager.PlaySoundFX(Channel.PLAYER_DEATH_FX, Sound.DEATH);
            }
        }

        if (life.value <= 0)
        {
            SceneManager.LoadScene("End");
        }
    }

    void FixedUpdate()
    {
        var hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;

        Move();
        Jump();
        AirCheck();

        if (isCameraShaking)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0.0f)
            {
                perlin.m_AmplitudeGain = 0.0f;
                shakeTimer = shakeDuration;
                isCameraShaking = false;
            }
        }
    }

    private void Move()
    {
        var x = Input.GetAxisRaw("Horizontal") + ((Application.isMobilePlatform) ? leftStick.Horizontal : 0.0f);

        if(x != 0.0f)
        {
            Flip(x);

            x = (x > 0.0f) ? 1.0f : -1.0f; // Sanitizing X (limiting X to 1 or -1)

            rigidbody2D.AddForce(Vector2.right * x * horizontalForce * ((isGrounded) ? 1.0f : airFactor));

            var clampedXVelocity = Mathf.Clamp(rigidbody2D.velocity.x, -horizontalSpeed, horizontalSpeed);

            rigidbody2D.velocity = new Vector2(clampedXVelocity, rigidbody2D.velocity.y);

            ChangeAnimation(PlayerAnimationState.RUN);

            if (isGrounded)
            {
                CreateDustTrail();
            }
        }

        if((isGrounded) && (x == 0.0f))
        {
            ChangeAnimation(PlayerAnimationState.IDLE);
        }
    }

    private void CreateDustTrail()
    {
        dustTrail.GetComponent<Renderer>().material.SetColor("Color", dustTrailColor);
        dustTrail.Play();
    }

    private void ShakeCamera()
    {
        perlin.m_AmplitudeGain = shakeIntensity;
        isCameraShaking = true;
    }

    private void Jump()
    {
        var y = Input.GetAxis("Jump") + ((Application.isMobilePlatform) ? leftStick.Vertical : 0.0f);

        if((isGrounded) && (y > verticalThreshold))
        {
            rigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
            soundManager.PlaySoundFX(Channel.PLAYER_SOUND_FX, Sound.JUMP);
        }
    }

    private void AirCheck()
    {
        if(!isGrounded)
        {
            ChangeAnimation(PlayerAnimationState.JUMP);
        }
    }

    public void Flip(float x)
    {
        if(x != 0.0f)
        {
            transform.localScale = new Vector3((x > 0.0f) ? 1.0f : -1.0f, 1.0f, 1.0f);
        }
    }

    private void ChangeAnimation(PlayerAnimationState animationState)
    {
        playerAnimationState = animationState;
        animator.SetInteger("AnimationState", (int)playerAnimationState);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            health.UpdateHealth(-20);
            soundManager.PlaySoundFX(Channel.PLAYER_HURT_FX, Sound.HURT);
            ShakeCamera();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            health.UpdateHealth(-30);
            soundManager.PlaySoundFX(Channel.PLAYER_HURT_FX, Sound.HURT);
            ShakeCamera();
        }
    }
}
