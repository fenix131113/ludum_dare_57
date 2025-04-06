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
        [SerializeField] private PlayerSettingsSO playerSettingsSO;

        protected override void Configure(IContainerBuilder builder)
        {
            #region Core

            builder.Register<DictionaryObjectPool>(Lifetime.Singleton);

            #endregion

            #region Player

            builder.Register<PlayerInput>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterInstance(playerSettingsSO);

            builder.RegisterComponentInHierarchy<ItemHolder>();
            builder.RegisterComponentInHierarchy<PlayerMovement>();

            #endregion
        }
    }
}