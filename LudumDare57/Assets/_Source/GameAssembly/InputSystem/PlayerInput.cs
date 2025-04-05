using System;
using UnityEngine;
using VContainer.Unity;

namespace InputSystem
{
    public class PlayerInput : ITickable
    {
        public event Action<Vector2> OnMove;
        public event Action OnJump;
        
        public void Tick()
        {
            ReadMoveInput();
            ReadJumpInput();
        }

        private void ReadMoveInput()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            input.Normalize();
            
            OnMove?.Invoke(input);
        }

        private void ReadJumpInput()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
                OnJump?.Invoke();
        }
    }
}