using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputState
{
    public Vector2 MovementDirection;
    public bool IsCrouching;
    public bool CanInteract;
    public bool CanJump;
    public bool CanTarget;
}

public interface IInputMiddleware
{
    event Action Interacted;
    event Action OnJump;
    event Action<bool> Targeted;

    int Order { get; }
    InputState Process(InputState previous);
}

public class InputProvider : ScriptableObject
{
    public event Action Interacted;
    public event Action OnJump;
    public event Action<bool> Targeted;

    private readonly List<IInputMiddleware> _middlewares = new List<IInputMiddleware>();

    public InputState GetState()
    {
        var state = new InputState();
        foreach (var middleware in _middlewares)
            state = middleware.Process(state);

        return state;
    }

    public void AddMiddleware(IInputMiddleware middleware)
    {
        middleware.Interacted += HandleInteracted;
        middleware.OnJump += HandleOnJump;
        middleware.Targeted += HandleTargeted;

        for (var i = 0; i < _middlewares.Count; i++)
        {
            if (middleware.Order >= _middlewares[i].Order) continue;
            _middlewares.Insert(i, middleware);
            return;
        }

        _middlewares.Add(middleware);
    }

    public void RemoveMiddleware(IInputMiddleware middleware)
    {
        if (_middlewares.Remove(middleware))
        {
            middleware.Interacted -= HandleInteracted;
            middleware.OnJump -= HandleOnJump;
            middleware.Targeted -= HandleTargeted;
        }
    }

    private void HandleInteracted()
    {
        if (GetState().CanInteract)
            Interacted?.Invoke();
    }

    private void HandleOnJump()
    {
        if(GetState().CanJump)
            OnJump?.Invoke();
    }

    private void HandleTargeted(bool value)
    {
        if (GetState().CanTarget)
            Targeted?.Invoke(value);
    }

    private void OnDisable()
    {
        foreach (var middleware in _middlewares)
        {
            middleware.Interacted -= HandleInteracted;
            middleware.OnJump -= HandleOnJump;
            middleware.Targeted -= HandleTargeted;
        }

        _middlewares.Clear();
    }
}