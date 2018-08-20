using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Pool")]
public class BulletPoolType : ScriptableObject
{
    public GameObject bullet;
    public ObjectPooler pool = null;
    public bool disable = false;

    public GameObject GetBullet()
    {
        if (disable)
        {
            return null;
        }
        if (pool)
        {
            GameObject g = pool.GetPooledObject();
            g.SetActive(true);
            return g;
        }
        return Instantiate(bullet);
    }

    public void ReturnBullet(GameObject bullet)
    {
        if (pool)
        {
            bullet.SetActive(false);
            return;
        }
        Destroy(bullet);
    }

    public void Attach(ObjectPooler pool)
    {
        this.pool = pool;
    }

    public void Detach()
    {
        pool = null;
    }
}

