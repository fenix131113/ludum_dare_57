using InputSystem;
using ItemsSystem.Player;
using Player;
using Player.Data;
using UnityEngine;
using Upgrades;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class GameInstaller : LifetimeScope
    {
        [SerializeField] private PlayerSettingsSO playerSettingsSO;
        
        private static GameInstaller _instance;

        protected override void Configure(IContainerBuilder builder)
        {
            _instance = this;
            
            #region Core

            builder.Register<DictionaryObjectPool>(Lifetime.Singleton);
            builder.Register<GameState>(Lifetime.Singleton);

            var playerStats = FindAnyObjectByType<PlayerStats>();
            builder.RegisterInstance(playerStats);

            #endregion

            #region Player

            builder.Register<PlayerInput>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterInstance(playerSettingsSO);

            builder.RegisterComponentInHierarchy<ItemHolder>();
            builder.RegisterComponentInHierarchy<PlayerMovement>();
            builder.RegisterComponentInHierarchy<PlayerHealth>();
            
            // TODO: ADD PLAYER STATS

            #endregion
        }

        public static GameObject InstantiateInjectedObject(GameObject go) => _instance.Container.Instantiate(go);
    }
}