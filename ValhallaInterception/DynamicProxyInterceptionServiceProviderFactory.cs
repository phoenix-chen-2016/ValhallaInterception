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
				if (descriptor.ImplementationType != null)
				{
					var warppedType = proxy.ProxyBuilder.CreateClassProxyType(
						descriptor.ImplementationType,
						descriptor.ImplementationType.GetInterfaces(),
						ProxyGenerationOptions.Default);

					warppedServices.Add(
						ServiceDescriptor.Describe(
							descriptor.ServiceType,
							sp =>
							{
								return ActivatorUtilities.CreateInstance(
									sp,
									warppedType);
							},
							descriptor.Lifetime));

					continue;
				}

				warppedServices.Add(descriptor);
			}

			return warppedServices.BuildServiceProvider();
		}
	}
}
