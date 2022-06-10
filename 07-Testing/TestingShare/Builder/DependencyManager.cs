namespace TestingShare
{
	public class DependencyManager
	{
		private readonly Dictionary<string, object> dependencies = new();

		public void AddDependency<T>(T dependency)
			where T : class
		{
			AddDependency(typeof(T).Name, dependency);
		}


		public void AddDependency(string name, object value)
		{
			dependencies.Add(name, value);
		}

		public T GetDependency<T>()
			where T : class
		{
			var name = typeof(T).Name;

			if (dependencies.TryGetValue(name, out var value))
				return (T)value;

			return default;
		}

		public T GetDependency<T>(string name)
		{
			if (dependencies.TryGetValue(name, out var value))
				return (T)value!;

			return default!;
		}
	}
}