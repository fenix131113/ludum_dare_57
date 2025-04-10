using InputSystem;
using ItemsSystem.Player;
using Player;
using Player.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class GameInstaller : LifetimeScope
    {
        [field: SerializeField] public Transform DefaultSpawnParent { get; private set; }

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
            builder.RegisterComponentInHierarchy<InteractiveChecker>();

            // TODO: ADD PLAYER STATS

            #endregion
        }

        public static GameObject InstantiateInjectedObject(GameObject obj)
        {
            var spawned = _instance?.Container.Instantiate(obj, _instance.DefaultSpawnParent);
            return spawned;
        }

        public static void InjectObject(object obj) => _instance?.Container.Inject(obj);
    }
}