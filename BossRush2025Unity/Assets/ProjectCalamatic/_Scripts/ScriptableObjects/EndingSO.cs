using UnityEngine;

public enum Endings
{
    Shutdowned,
    ShutdownedDuringGamePlay,
    Image,
    System
}

[CreateAssetMenu(fileName = "EndingSO", menuName = "Scriptable Objects/EndingSO")]
public class EndingSO : ScriptableObject
{
    public Endings currentEnding;
}
