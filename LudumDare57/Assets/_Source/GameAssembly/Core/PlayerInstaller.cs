using InputSystem;
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
        }
    }
}