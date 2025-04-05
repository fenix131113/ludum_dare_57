using System;
using UnityEngine;
using VContainer.Unity;

namespace InputSystem
{
    public class PlayerInput : ITickable
    {
        public event Action<Vector2> OnMove;
        public event Action<float> OnScroll;
        public event Action OnJump;
        public event Action OnFall;
        public event Action OnInteract;

        public void Tick()
        {
            ReadMoveInput();
            ReadJumpInput();
            ReadFallInput();
            ReadInteractInput();
            ReadScrollInput();
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

        private void ReadInteractInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
                OnInteract?.Invoke();
        }

        private void ReadFallInput()
        {
            if (Input.GetKey(KeyCode.S))
                OnFall?.Invoke();
        }

        private void ReadScrollInput()
        {
            if(Input.mouseScrollDelta.y != 0)
                OnScroll?.Invoke(Input.mouseScrollDelta.y);
        }
    }
}