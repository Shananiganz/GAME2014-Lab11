using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackAction : MonoBehaviour, Action
{
    [Header("Ranged Attack Properties")]
    [Range(1, 100)]
    public int fireDelay = 30;
    public Transform bulletSpawn;

    public GameObject bulletPrefab;
    public Transform bulletParent;

    private bool hasLOS;
    private PlayerDetection playerDetection;
    private SoundManager soundManager;

    void Awake()
    {
        playerDetection = transform.parent.GetComponentInChildren<PlayerDetection>();
        soundManager = FindObjectOfType<SoundManager>();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        bulletParent = GameObject.Find("[BULLETS]").transform;
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
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity, bulletParent);
        bullet.GetComponent<BulletController>().Activate();
        soundManager.PlaySoundFX(Channel.BULLET, Sound.BULLET);
    }
}
