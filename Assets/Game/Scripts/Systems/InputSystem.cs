using Input = UnityEngine.InputSystem.InputSystem;
using UnityEngine.InputSystem;
using UnityEngine;
using Leopotam.Ecs;

public class InputSystem : Injects, IEcsInitSystem, IEcsRunSystem
{
    private InputAction _moveInputAction;
    private InputAction _jumpInputAction;
    private InputAction _interacionInputAction;
    private InputAction _mouseInteractInputAction;

    private InputAction _cosoleInputAction;

    public void Init()
    {
        string moveKeyTag = GameConfig.InputConfig.MoveKeyTag;
        _moveInputAction = Input.actions.FindAction(moveKeyTag);
        if (_moveInputAction == null)
            Debug.LogError($"[INPUT SYSTEM] Key tag |{moveKeyTag}| for move is not recognized!" +
                           "Please check Input Config or Input System Settings!");

        string jumpKeytag = GameConfig.InputConfig.JumpKeyTag;
        _jumpInputAction = Input.actions.FindAction(jumpKeytag);
        if (_jumpInputAction != null)
            _jumpInputAction.performed += OnJunpKeyPress;
        else
            Debug.LogError($"[INPUT SYSTEM] Key tag |{jumpKeytag}| for jump is not recognized!" +
                               "Please check Input Config or Input System Settings!");

        string interactionKeytag = GameConfig.InputConfig.InteractionKeyTag;
        _interacionInputAction = Input.actions.FindAction(interactionKeytag);
        if (_interacionInputAction != null)
            _interacionInputAction.performed += OnInteractionKeyPress;
        else
            Debug.LogError($"[INPUT SYSTEM] Key tag |{interactionKeytag}| for interaction is not recognized!" +
                               "Please check Input Config or Input System Settings!");

        string mouseInteractionKeyTag = GameConfig.InputConfig.MouseInteractionKeyTag;
        _mouseInteractInputAction = Input.actions.FindAction(mouseInteractionKeyTag);
        if (_mouseInteractInputAction != null)
        {
            _mouseInteractInputAction.started += OnMouseInteractionStarted;
            _mouseInteractInputAction.canceled += OnMouseInteractionCanceled;
        }
        else
        {
            Debug.LogError($"[INPUT SYSTEM] Key tag |{mouseInteractionKeyTag}| for interaction is not recognized!");
        }

        string consoleTag = GameConfig.InputConfig.ConsoleTag;
        _cosoleInputAction = Input.actions.FindAction(consoleTag);
        if (_cosoleInputAction != null)
            _cosoleInputAction.performed += OnConsoleKeyPress;
        else
            Debug.LogError($"[INPUT SYSTEM] Key tag |{consoleTag}| for last fight style is not recognized!" +
                               "Please check Input Config or Input System Settings!");
    }

    private void OnJunpKeyPress(InputAction.CallbackContext callbackContext)
    {
        EcsWorld.NewEntity().Get<JumpInputEvent>();
    }

    private void OnInteractionKeyPress(InputAction.CallbackContext callbackContext)
    {
        EcsWorld.NewEntity().Get<InteractInputEvent>();
    }

    private void OnMouseInteractionStarted(InputAction.CallbackContext callbackContext)
    {
        EcsWorld.NewEntity().Get<MouseInteractStartEvent>();
    }

    private void OnMouseInteractionCanceled(InputAction.CallbackContext callbackContext)
    {
        EcsWorld.NewEntity().Get<MouseInteractEndEvent>();
    }

    private void OnConsoleKeyPress(InputAction.CallbackContext callbackContext)
    {
        EcsWorld.NewEntity().Get<ConsoleOpenCloseEvent>();
    }

    public void Run()
    {
        var moveInputValue = _moveInputAction.ReadValue<Vector2>();
        EcsWorld.NewEntity().Get<MoveInputEvent>().Vector2 = moveInputValue;

    }
}
