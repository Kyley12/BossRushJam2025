using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool bulletPoolInstance;

    [System.Serializable]
    public class BulletTypePool
    {
        public BulletType bulletType;
        public GameObject bulletPrefab;
        public List<GameObject> bullets = new List<GameObject>();
    }

    [SerializeField]
    private List<BulletTypePool> bulletTypePools;

    private void Awake()
    {
        if (bulletPoolInstance == null)
        {
            bulletPoolInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gets an inactive bullet from the pool for the given bullet type.
    /// </summary>
    public GameObject GetBullet(BulletType bulletType)
    {
        BulletTypePool typePool = bulletTypePools.Find(pool => pool.bulletType == bulletType);

        if (typePool != null)
        {

            Debug.Log($"Instantiating new bullet of type {bulletType}");
            GameObject newBullet = Instantiate(typePool.bulletPrefab);
            newBullet.SetActive(false);
            typePool.bullets.Add(newBullet);
            return newBullet;
        }

        Debug.LogError($"No pool found for bullet type: {bulletType}");
        return null;
    }

    /// <summary>
    /// Clears inactive bullets from the pool for all bullet types.
    /// </summary>
    public void ClearPool()
    {
        foreach (var typePool in bulletTypePools)
        {
            for (int i = typePool.bullets.Count - 1; i >= 0; i--)
            {
                if (typePool.bullets[i] != null && !typePool.bullets[i].activeInHierarchy)
                {
                    Destroy(typePool.bullets[i]); // Destroy inactive bullets
                    typePool.bullets.RemoveAt(i); // Remove from the list
                }
            }
        }

        Debug.Log("Inactive bullets cleared from all pools.");
    }

    /// <summary>
    /// Finds the pool corresponding to the given bullet type.
    /// </summary>
    private BulletTypePool GetPool(BulletType bulletType)
    {
        return bulletTypePools.Find(pool => pool.bulletType == bulletType);
    }
}
