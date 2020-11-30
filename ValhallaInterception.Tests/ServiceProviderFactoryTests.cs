using Castle.DynamicProxy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Valhalla.Interception;
using ValhallaInterception.Tests.Stubs;

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
			services.AddSingleton(fakeInterceptor);

			var providerFactory = new DynamicProxyInterceptionServiceProviderFactory();
			var serviceProvider = providerFactory.CreateServiceProvider(services);
			var sut = serviceProvider.GetRequiredService<IStubService>();

			// act
			sut.DoSomething();

			// assert
			fakeInterceptor.Received().Intercept(Arg.Any<IInvocation>());
		}

		[TestMethod]
		public void 註冊的實作是工廠方法也可以正常()
		{
			// arrange
			var services = new ServiceCollection();

			services.AddTransient<IStubService>(sp => new StubService());

			var fakeInterceptor = Substitute.For<IInterceptor>();
			services.AddSingleton(fakeInterceptor);

			var providerFactory = new DynamicProxyInterceptionServiceProviderFactory();
			var serviceProvider = providerFactory.CreateServiceProvider(services);
			var sut = serviceProvider.GetRequiredService<IStubService>();

			// act
			sut.DoSomething();

			// assert
			fakeInterceptor.Received().Intercept(Arg.Any<IInvocation>());
		}

		[TestMethod]
		public void 註冊的實作是實體也可以正常()
		{
			// arrange
			var services = new ServiceCollection();

			services.AddSingleton<IStubService>(new StubService());

			var fakeInterceptor = Substitute.For<IInterceptor>();
			services.AddSingleton(fakeInterceptor);

			var providerFactory = new DynamicProxyInterceptionServiceProviderFactory();
			var serviceProvider = providerFactory.CreateServiceProvider(services);
			var sut = serviceProvider.GetRequiredService<IStubService>();

			// act
			sut.DoSomething();

			// assert
			fakeInterceptor.Received().Intercept(Arg.Any<IInvocation>());
		}

		[TestMethod]
		public void 如果沒有指定Proxy的物件實作型別不會有Proxy()
		{
			// arrange
			var services = new ServiceCollection();

			services.AddTransient<IStubService, StubService>();

			var fakeInterceptor = Substitute.For<IInterceptor>();
			services.AddSingleton(fakeInterceptor);

			var providerFactory = new DynamicProxyInterceptionServiceProviderFactory();
			var sut = providerFactory.CreateServiceProvider(services);

			// act
			var actual = sut.GetRequiredService<IStubService>();

			// assert
			actual.GetType()
				.Should()
				.Be(typeof(StubService));
		}
	}
}
