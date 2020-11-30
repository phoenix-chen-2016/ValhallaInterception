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
		public void �i�H���`�qDIContainer�����o����()
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
		public void ���o����OProxy����()
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
		public void Interceptor���@�ΦbProxy����W()
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
