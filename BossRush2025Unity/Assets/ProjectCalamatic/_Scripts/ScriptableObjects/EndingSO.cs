using UnityEngine;

public enum Endings
{
    Shutdowned,
    ShutdownedDuringGamePlay,
    Image,
    System,
    CursorBreak,
    DefeatedBossButAtWhatCost,
    DefeatedBoss
}

[CreateAssetMenu(fileName = "EndingSO", menuName = "Scriptable Objects/EndingSO")]
public class EndingSO : ScriptableObject
{
    public Endings currentEnding;
}
