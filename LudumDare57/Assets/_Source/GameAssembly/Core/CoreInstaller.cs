using VContainer;
using VContainer.Unity;

namespace Core
{
    public class CoreInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<DictionaryObjectPool>(Lifetime.Singleton);
        }
    }
}