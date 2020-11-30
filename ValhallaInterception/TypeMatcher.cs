using System;

namespace Valhalla.Interception
{
	internal class TypeMatcher : ITypeMatcher
	{
		private readonly Type m_ExceptedType;

		public TypeMatcher(Type exceptedType)
		{
			m_ExceptedType = exceptedType ?? throw new ArgumentNullException(nameof(exceptedType));
		}

		public bool Match(Type serviceType)
		{
			return m_ExceptedType == serviceType;
		}
	}
}
