using UnityEngine;
using Leopotam.Ecs;

public class EcsInclude : MonoBehaviour
{

    [SerializeField] private UI _ui;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private SceneData _sceneData;
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
            .Add(new LevelGenerationSystem())
            .Add(new BackgroundLoopSystem())


            //OneFrame<..
            .OneFrame<JumpInputEvent>()
            .OneFrame<InteractInputEvent>()
            .OneFrame<ConsoleOpenCloseEvent>()
            .OneFrame<MoveInputEvent>()
            .OneFrame<GroundEvent>()
            .OneFrame<GroundRaycastEvent>()

            .Add(new ConsoleSystem())
            .OneFrame<CommandEvent>()
            .OneFrame<ConsoleOpenCloseEvent>()


            .Inject(_world)
            .Inject(_gameConfig)
            .Inject(_ui)
            .Inject(_sceneData)

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
