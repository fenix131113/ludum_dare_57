using System;
using System.Collections.Generic;
using System.Linq;
using InputSystem.Data;
using UnityEngine;
using VContainer.Unity;

namespace InputSystem
{
    public class PlayerInput : ITickable
    {
        private readonly Dictionary<InputType, Dictionary<Type, List<object>>> _valueInputCallbacks = new();
        private readonly Dictionary<InputType, List<IInputCallback>> _nonValueCallbacks = new();

        public void RegisterCallback<T>(InputType inputType, IInputValueCallback<T> valueCallback)
        {
            _valueInputCallbacks.TryAdd(inputType, new Dictionary<Type, List<object>>());
            _valueInputCallbacks[inputType].TryAdd(typeof(T), new List<object>());

            _valueInputCallbacks[inputType][typeof(T)].Add(valueCallback);
        }

        public void RegisterNonValueCallback(InputType inputType, IInputCallback callback)
        {
            _nonValueCallbacks.TryAdd(inputType, new List<IInputCallback>());
            _nonValueCallbacks[inputType].Add(callback);
        }

        public void Tick()
        {
            ReadMoveInput();
            ReadJumpInput();
        }

        private void ReadMoveInput()
        {
            if(!_valueInputCallbacks.ContainsKey(InputType.MOVE))
                return;
            
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            input.Normalize();

            foreach (var c in from callbackPair in _valueInputCallbacks[InputType.MOVE]
                     from callback in callbackPair.Value
                     select callback as IInputValueCallback<Vector2>)
            {
                c?.InputCallback(input);
            }
        }

        private void ReadJumpInput()
        {
            if (!Input.GetKeyDown(KeyCode.Space) || !_nonValueCallbacks.TryGetValue(InputType.JUMP, out var callback))
                return;
            
            callback.ForEach(x => x.InputCallback());
        }
    }
}