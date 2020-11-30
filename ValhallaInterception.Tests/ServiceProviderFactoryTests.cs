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
			// act
			// assert
		}
	}
}