using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public class EcsInclude : MonoBehaviour
{

    [SerializeField] private UI _ui;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private SceneData _sceneData;
    private RealtimeData _realtimeData = new RealtimeData()
    {
        Cats = new List<EcsEntity>(),
        StartLevelTime = 0.0f,
        IsGameEnd = false
    };
    private EcsWorld _world;
    private EcsSystems _systems;

    public void Awake()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);

        _systems
            //Add (new ...
            .Add(new InitialSystem())
            .Add(new InputSystem())
            .Add(new MovementSystem())
            .Add(new JumpSystem())
            .Add(new GroundRaycastSystem())
            .Add(new GroundCheckSystem())
            .Add(new BackgroundLoopSystem())
            .Add(new InputSystem())
            .Add(new GiftTakeHandsSystem())
            .Add(new CatsWishesSystem())
            .Add(new GiftsSpawnSystem())
            .Add(new CameraControlSystem())
            .Add(new CollisionSystem())
            .Add(new TimerSystem())
            .Add(new GiveGiftsSystem())
            .Add(new EndGameSystem())
            .OneFrame<EndGameEvent>()
            .Add(new LevelGenerationSystem())
            .Add(new PlayerAniimationSystem())


            //OneFrame<..
            .OneFrame<JumpInputEvent>()
            .OneFrame<InteractInputEvent>()
            .OneFrame<ConsoleOpenCloseEvent>()
            .OneFrame<MoveInputEvent>()
            .OneFrame<GroundEvent>()
            .OneFrame<GroundRaycastEvent>()
            .OneFrame<MouseInteractStartEvent>()
            .OneFrame<MouseInteractEndEvent>()
            .OneFrame<WishlistUpdateEvent>()
            .OneFrame<TakeOfGiftEvent>()
            .OneFrame<SpawnGiftEvent>()
            .OneFrame<CameraToPlayerEvent>()
            .OneFrame<OnCollisionEvent>()
            .OneFrame<GiveGiftEvent>()
            .OneFrame<CatInteractEvent>()
            .OneFrame<SpawnLastChunkEvent>()

            .Add(new ConsoleSystem())
            .OneFrame<CommandEvent>()
            .OneFrame<ConsoleOpenCloseEvent>()


            .Inject(_world)
            .Inject(_gameConfig)
            .Inject(_ui)
            .Inject(_sceneData)
            .Inject(_realtimeData)


            .Init();
    }

    public void Update()
    {
        _systems.Run();
    }

    public void Destroy()
    {
        _systems.Destroy();
        _world.Destroy();
    }
}
