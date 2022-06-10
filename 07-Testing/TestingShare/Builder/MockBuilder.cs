namespace TestingShare
{
	public abstract class MockBuilder       // TODO: Testing - 01
	{
		protected readonly DependencyManager dependencies = new();

		public MockBuilder AddDependency<T>(T dependency)
			where T : class
		{
			dependencies.AddDependency(dependency);
			return this;
		}

		public MockBuilder AddDependency(string name, object value)
		{
			dependencies.AddDependency(name, value);
			return this;
		}

		public T GetDependency<T>()
			where T : class
		{
			return dependencies.GetDependency<T>();
		}

		public T GetDependency<T>(string name)
		{
			return dependencies.GetDependency<T>(name);
		}
	}
}