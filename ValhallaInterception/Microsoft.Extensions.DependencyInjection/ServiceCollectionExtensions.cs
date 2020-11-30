using System;
using Valhalla.Interception.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static InterceptionConfiguration ConfigureValhallaInterception(
			this IServiceCollection services)
		{
			return new InterceptionConfiguration(services);
		}
	}
}
