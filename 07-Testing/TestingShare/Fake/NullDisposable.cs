namespace TestingShare
{
	public class NullDisposable : IDisposable
	{
		public static readonly NullDisposable Instance = new NullDisposable();

		private bool disposedValue;


		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// Intentionally does nothing
				}

				// Intentionally does nothing
				disposedValue = true;
			}
		}


		public void Dispose()
		{
			// Intentionally does nothing

			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}