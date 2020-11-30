using System;

namespace Valhalla.Interception
{
	public interface ITypeMatcher
	{
		bool Match(Type serviceType);
	}
}
