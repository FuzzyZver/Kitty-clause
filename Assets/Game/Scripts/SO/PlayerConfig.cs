using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public PlayerActor PlayerActor;
    public float Speed;

    [Header("JumpProperties")]

    public float CoyoteTime = 0.12f;
    public float JumpForce = 15;
    public float JumpVelocity = 0f;
    public float JumpMultiply = 1.5f;
    public float LastGroundTime = -999f;

    public float HangGravityScale = 5f;
    public float NormalGravityScale = 40f;
    public float HangGravityMultiply = 1.5f;

    public float Multiply = 1.5f;
}
