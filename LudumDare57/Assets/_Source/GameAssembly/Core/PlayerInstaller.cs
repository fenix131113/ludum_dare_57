using InputSystem;
using ItemsSystem.Player;
using Player;
using Player.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class PlayerInstaller : LifetimeScope
    {
        [SerializeField] private PlayerSettingsSO playerSettingsSO;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerInput>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterInstance(playerSettingsSO);

            builder.RegisterComponentInHierarchy<ItemHolder>();
            builder.RegisterComponentInHierarchy<PlayerMovement>();
            
            builder.Register<DictionaryObjectPool>(Lifetime.Singleton);
        }
    }
}