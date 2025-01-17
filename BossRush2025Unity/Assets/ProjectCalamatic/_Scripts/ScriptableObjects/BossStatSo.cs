using UnityEngine;

[CreateAssetMenu(fileName = "BossStatSo", menuName = "Scriptable Objects/BossStatSo")]
public class BossStatSo : ScriptableObject
{
    public float bossStunbarHealth;
    public float bossStunbarHealthMax;
    public int bossRagebarMax;
    public int bossCurrRage;

}
