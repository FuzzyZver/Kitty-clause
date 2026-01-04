using UnityEngine;
using Leopotam.Ecs;

public class GiftsEcs : MonoBehaviour
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
            .Add(new InputSystem())
            .Add(new GiftTakeHandsSystem())
            .Add(new CatsWishesSystem())

            //OneFrame<..
            .OneFrame<MouseInteractStartEvent>()
            .OneFrame<MouseInteractEndEvent>()
            .OneFrame<WishlistUpdateEvent>()

            .Add(new ConsoleSystem())
            .OneFrame<CommandEvent>()
            .OneFrame<ConsoleOpenCloseEvent>()


            .Inject(_world)
            .Inject(_gameConfig)
            .Inject(_ui)
            .Inject(_sceneData)

            .Init();
    }

    public void Start()
    {
        foreach (GiftActor gift in _sceneData.Gifts)
        {
            gift.Init(_world);
        }
        _sceneData.GiftsBag.Init(_world);
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
