using System;

namespace Core
{
    public class GameState
    {
        public bool GameCycleBlocked;
        
        public event Action<bool> OnGameCycleBlockedChanged;
        
        public void SetGameCycleBlocked(bool value)
        {
            GameCycleBlocked = value;
            OnGameCycleBlockedChanged?.Invoke(value);
        }
    }
}