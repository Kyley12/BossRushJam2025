using UnityEngine;
using UnityEngine.UI;

public class BossStunbarDisplay : MonoBehaviour
{
    public Slider stunbar;
    public BossStatSo bossStat;

    private void Start()
    {
        bossStat.bossStunbarHealth = bossStat.bossStunbarHealthMax;
    }
    private void Update()
    {
        stunbar.value = bossStat.bossStunbarHealth / bossStat.bossStunbarHealthMax;
    }
}
