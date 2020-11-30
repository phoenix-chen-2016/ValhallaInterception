using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
			// act
			// assert
		}
	}
}
