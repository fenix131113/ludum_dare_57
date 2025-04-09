using System;
using UnityEngine;

namespace Core
{
    public class GameState
    {
        public bool GamePaused;
        public bool PlayerDamagePaused;
        public bool PlayerMovementPaused;
        
        public event Action<bool> OnGamePausedChanged;
        public event Action<bool> OnPlayerDamagePausedChanged;
        public event Action<bool> OnMovementPausedChanged;

        public GameState() => SetGamePaused(false);

        /// <summary>
        /// For menu. Change game paused state and toggle cursor
        /// </summary>
        public void SetGamePaused(bool value)
        {
            GamePaused = value;
            OnGamePausedChanged?.Invoke(value);
        }

        public void SetPlayerDamagePaused(bool value)
        {
            PlayerDamagePaused = value;
            OnPlayerDamagePausedChanged?.Invoke(value);
        }

        public void SetPlayerMovementPaused(bool value)
        {
            PlayerMovementPaused = value;
            OnMovementPausedChanged?.Invoke(value);
        }
    }
}