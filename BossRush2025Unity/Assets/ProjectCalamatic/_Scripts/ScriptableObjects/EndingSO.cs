using UnityEngine;

public enum Endings
{
    Shutdowned,
    ShutdownedDuringGamePlay,
    ImageDeleted,
    SystemDeleted
}

[CreateAssetMenu(fileName = "EndingSO", menuName = "Scriptable Objects/EndingSO")]
public class EndingSO : ScriptableObject
{
    public Endings currentEnding;
}
