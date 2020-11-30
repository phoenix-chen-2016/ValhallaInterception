using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Valhalla.Interception.Configuration;

namespace Valhalla.Interception
{
	internal class DynamicProxyInterceptionServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
	{
		public IServiceCollection CreateBuilder(IServiceCollection services) => services;

		public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
		{
			var tempServiceProvider = containerBuilder.BuildServiceProvider();

			var matchers = tempServiceProvider.GetServices<ITypeMatcher>();

			IServiceCollection warppedServices = new ServiceCollection();

			var proxy = new ProxyGenerator();

			foreach (var descriptor in containerBuilder)
			{
				if (descriptor.ServiceType == typeof(IInterceptor))
				{
					warppedServices.Add(descriptor);
					continue;
				}

				if (!CheckTypeMatched(matchers, descriptor.ServiceType))
				{
					warppedServices.Add(descriptor);
					continue;
				}

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
									warppedType,
									new object[] {
										sp.GetServices<IInterceptor>()
									});
							},
							descriptor.Lifetime));

					continue;
				}

				if (descriptor.ImplementationFactory != null)
				{
					warppedServices.Add(
						ServiceDescriptor.Describe(
							descriptor.ServiceType,
							sp =>
							{
								var target = proxy.CreateInterfaceProxyWithTarget(
									descriptor.ServiceType,
									descriptor.ImplementationFactory(sp),
									sp.GetServices<IInterceptor>().ToArray());

								return target;
							},
							descriptor.Lifetime));

					continue;
				}

				if (descriptor.ImplementationInstance != null)
				{
					warppedServices.Add(
						ServiceDescriptor.Describe(
							descriptor.ServiceType,
							sp =>
							{
								var target = proxy.CreateInterfaceProxyWithTarget(
									descriptor.ServiceType,
									descriptor.ImplementationInstance,
									sp.GetServices<IInterceptor>().ToArray());

								return target;
							},
							descriptor.Lifetime));

					continue;
				}

				warppedServices.Add(descriptor);
			}

			return warppedServices.BuildServiceProvider();
		}

		private bool CheckTypeMatched(IEnumerable<ITypeMatcher>? matchers, Type serviceType)
		{
			if (matchers == null)
				return false;

			foreach (var matcher in matchers)
			{
				if (matcher.Match(serviceType))
					return true;
			}

			return false;
		}
	}
}
