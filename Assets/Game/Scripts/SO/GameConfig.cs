using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    public InputConfig InputConfig;
    public PlayerConfig PlayerConfig;
    public CatsCharConfig CatsCharConfig;
    public LevelConfig LevelConfig;
    public CommonConfig CommonConfig;
    public DataConfig DataConfig;
}
