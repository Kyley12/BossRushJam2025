using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BulletPattern
{
    public BulletType bulletType;
    public float bulletsAmount;
    public float fireRate;
    public float startAngle;
    public float endAngle;
}

[CreateAssetMenu(fileName = "BossPatterns", menuName = "Scriptable Objects/BossPaterns")]
public class BossPatterns : ScriptableObject
{
    public List<BulletPattern> patterns;

    public List<BulletPattern> GetPatterns(BulletType bulletType)
    {
        return patterns.FindAll(pattern => pattern.bulletType == bulletType);
    }

    public BulletPattern GetPattern(BulletType bulletType, int index)
    {
        var matchingPatterns = GetPatterns(bulletType);
        return matchingPatterns[index];
    }
}
