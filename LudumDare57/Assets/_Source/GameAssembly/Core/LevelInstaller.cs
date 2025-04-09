using Levels.Quests;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public sealed class LevelInstaller : GameInstaller
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponentInHierarchy<QuestsManager>();
        }
    }
}