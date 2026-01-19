using UnityEngine;
using Unity.Cinemachine;

public class SceneData : MonoBehaviour
{
    public PlayerActor Player;
    public GiftsBagActor GiftsBag;
    public ChunkActor FirstChunk;
    public SpawnGiftsView SpawnGiftsView;
    public Transform GiftSpawnPoint;
    public CinemachineCamera PlayerCamera;
    public CinemachineCamera StartCamera;
}
