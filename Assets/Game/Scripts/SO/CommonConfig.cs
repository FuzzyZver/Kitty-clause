using UnityEngine;

[CreateAssetMenu(fileName = "CommonConfig", menuName = "Configs/CommonConfig")]
public class CommonConfig : ScriptableObject
{
    public float ObstacleDeletedTime;
    [Header("UI Views")] 
    public GiveGiftView GiveGiftView;
}
