using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern/Pool")]
public class BulletPoolType : ScriptableObject {
    [SerializeField]
    private GameObject bullet;
//    [SerializeField]
//    private List<GameObject> pool;
//    [SerializeField]
//    private int initialCapacity = 50;

    public GameObject GetBullet()
    {
        return Instantiate(bullet);
    }

    public void ReturnBullet(GameObject bullet)
    {
        Destroy(bullet);
    }
    /*
    private void Awake()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < initialCapacity; i++)
        {
            GameObject obj = (GameObject)Instantiate(bullet);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }
    */
}

