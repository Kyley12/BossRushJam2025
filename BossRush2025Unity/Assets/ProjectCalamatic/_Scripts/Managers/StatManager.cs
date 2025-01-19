using UnityEngine;
using UnityEngine.SceneManagement;

public class StatManager : MonoBehaviour
{
    public BossStatSo bossStat;
    public PlayerStatSO playerStat;

    public BattleHandler battleHandler;
    public EndingSO ending;

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

        if(playerStat.cursorHealth <= 0)
        {
            ending.currentEnding = Endings.CursorBreak;
            SceneManager.LoadScene("Ending");
        }
    }
}
