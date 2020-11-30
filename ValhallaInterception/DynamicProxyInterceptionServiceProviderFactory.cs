using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Valhalla.Interception
{
	internal class DynamicProxyInterceptionServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
	{
		public IServiceCollection CreateBuilder(IServiceCollection services) => services;

		public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
		{
			IServiceCollection warppedServices = new ServiceCollection();

			var proxy = new ProxyGenerator();

			foreach (var descriptor in containerBuilder)
			{
				warppedServices.Add(descriptor);
			}

			return warppedServices.BuildServiceProvider();
		}
	}
}
