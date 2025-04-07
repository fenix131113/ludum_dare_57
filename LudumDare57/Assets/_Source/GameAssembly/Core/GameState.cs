using System;
using UnityEngine;

namespace Core
{
    public class GameState
    {
        public bool GameCycleBlocked;
        
        public event Action<bool> OnGameCycleBlockedChanged;

        public GameState()
        {
            SetGameCycleBlocked(false);
        }
        
        public void SetGameCycleBlocked(bool value, bool visibleCursor = false)
        {
            GameCycleBlocked = value;
            Cursor.visible = visibleCursor;
            OnGameCycleBlockedChanged?.Invoke(value);
        }
    }
}