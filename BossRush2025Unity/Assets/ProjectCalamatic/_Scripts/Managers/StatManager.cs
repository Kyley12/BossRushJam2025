using UnityEngine;

public class StatManager : MonoBehaviour
{
    public BossStatSo bossStat;
    public PlayerStatSO playerStat;

    public BattleHandler battleHandler;

    private void Start()
    {
        if(battleHandler == null)
        {
            battleHandler = GetComponent<BattleHandler>();
        }
    }

    private void Update()
    {
        if(bossStat.bossStunbarHealth <= 0)
        {
            battleHandler.StunBoss();
        }
    }
}
