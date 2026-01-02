using UnityEngine;

[CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
public class InputConfig : ScriptableObject
{
    [Header("KeyTags")]
    public string MoveKeyTag;
    public string InteractionKeyTag;
    public string JumpKeyTag;
    public string MouseInteractionKeyTag;

    public string ConsoleTag;

    [Space]
    [Header("OtherProps")]
    public float MoveInputGravity;
    public float GroundDistanceThreshold;
    public float JumpDuration;
    public float MagneticGiftForce;
}
