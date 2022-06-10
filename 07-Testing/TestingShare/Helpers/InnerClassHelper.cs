using System.Reflection;

namespace TestingShare
{
	public class InnerClassHelper
	{
		static public T GetStaticPrivateField<T>(Type type, string fieldName)
		{
			var info = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
			return (T)info?.GetValue(null);
		}
	}
}
