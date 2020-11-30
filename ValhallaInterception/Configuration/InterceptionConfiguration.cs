using Microsoft.Extensions.DependencyInjection;

namespace Valhalla.Interception.Configuration
{
	public class InterceptionConfiguration
	{
		public IServiceCollection Services { get; }

		public InterceptionConfiguration(IServiceCollection services)
		{
			Services = services ?? throw new System.ArgumentNullException(nameof(services));
		}

		public InterceptionConfiguration AddTypeMatcher<TService>()
		{
			Services.AddSingleton<ITypeMatcher>(new TypeMatcher(typeof(TService)));

			return this;
		}
	}
}
