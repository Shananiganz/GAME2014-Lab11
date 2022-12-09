using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackAction : MonoBehaviour, Action
{
    [Header("Ranged Attack Properties")]
    [Range(1, 100)]
    public int fireDelay = 30;
    public Transform bulletSpawn;

    private bool hasLOS;
    private PlayerDetection playerDetection;
    private SoundManager soundManager;

    void Awake()
    {
        playerDetection = transform.parent.GetComponentInChildren<PlayerDetection>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        hasLOS = playerDetection.LOS;
    }

    void FixedUpdate()
    {
        if (hasLOS && Time.frameCount % fireDelay == 0)
        {
            Execute();
        }
    }

    public void Execute()
    {
        var bullet = BulletManager.Instance().GetBullet(bulletSpawn.position);
        soundManager.PlaySoundFX(Channel.BULLET, Sound.BULLET);
    }
}
