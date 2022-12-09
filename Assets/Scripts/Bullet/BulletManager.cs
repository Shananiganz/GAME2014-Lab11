using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    private static BulletManager instance;

    private BulletManager()
    {
        Initialize();
    }

    public static BulletManager Instance()
    {
        return instance ??= new BulletManager();
    }

    private Queue<GameObject> bulletPool;
    private int bulletNumber;
    private GameObject bulletPrefab;
    private Transform bulletParent;

    private void Initialize()
    {
        bulletNumber = 30;
        bulletPool = new Queue<GameObject>();
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
    }

    public void BuildBulletPool()
    {
        bulletParent = GameObject.Find("[BULLETS]").transform;
        for (int i = 0; i < bulletNumber; i++)
        {
            bulletPool.Enqueue(CreateBullet());
        }
    }

    private GameObject CreateBullet()
    {
        var tempBullet = MonoBehaviour.Instantiate(bulletPrefab, bulletParent);
        tempBullet.SetActive(false);
        return tempBullet;
    }

    public GameObject GetBullet(Vector2 position)
    {
        GameObject bullet = null;
        if (bulletPool.Count < 0)
        {
            bulletPool.Enqueue(CreateBullet());
        }

        bullet = bulletPool.Dequeue();
        bullet.SetActive(true);
        bullet.transform.position = position;
        bullet.GetComponent<BulletController>().Activate();
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    public void DestroyPool()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            var tempBullet = bulletPool.Dequeue();
            MonoBehaviour.Destroy(tempBullet);
        }
        bulletPool.Clear();
    }
}
