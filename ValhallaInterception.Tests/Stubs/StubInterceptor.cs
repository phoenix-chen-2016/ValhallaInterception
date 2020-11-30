using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using stakx.DynamicProxy;

namespace ValhallaInterception.Tests.Stubs
{
	class StubInterceptor : AsyncInterceptor
	{
		protected override void Intercept(IInvocation invocation)
		{
			throw new NotImplementedException();
		}

		protected override ValueTask InterceptAsync(IAsyncInvocation invocation)
		{
			throw new NotImplementedException();
		}
	}
}
