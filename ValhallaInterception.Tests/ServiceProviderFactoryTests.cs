using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Valhalla.Interception;
using ValhallaInterception.Tests.Stubs;
using NSubstitute;
using Castle.DynamicProxy;

namespace ValhallaInterception.Tests
{
	[TestClass]
	public class ServiceProviderFactoryTests
	{
		[TestMethod]
		public void 可以正常從DIContainer中取得物件()
		{
			// arrange
			var services = new ServiceCollection();

			services.AddTransient<IStubService, StubService>();

			var providerFactory = new DynamicProxyInterceptionServiceProviderFactory();
			var sut = providerFactory.CreateServiceProvider(services);

			// assert
			sut.Invoking(sp => sp.GetRequiredService<IStubService>())
				.Should()
				.NotThrow();
		}

		[TestMethod]
		public void 取得物件是Proxy物件()
		{
			// arrange
			var services = new ServiceCollection();

			services.AddTransient<IStubService, StubService>();

			var providerFactory = new DynamicProxyInterceptionServiceProviderFactory();
			var sut = providerFactory.CreateServiceProvider(services);

			// act
			var actual = sut.GetRequiredService<IStubService>();

			// assert
			actual.GetType()
				.Should()
				.NotBe(typeof(StubService));
		}

		[TestMethod]
		public void Interceptor有作用在Proxy物件上()
		{
			// arrange
			var services = new ServiceCollection();

			services.AddTransient<IStubService, StubService>();

			var fakeInterceptor = Substitute.For<IInterceptor>();
			var providerFactory = new DynamicProxyInterceptionServiceProviderFactory();
			var serviceProvider = providerFactory.CreateServiceProvider(services);
			var sut = serviceProvider.GetRequiredService<IStubService>();

			fakeInterceptor.Received().Intercept(Arg.Any<IInvocation>());

			// act
			sut.DoSomething();
		}
	}
}
