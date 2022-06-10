using System.Diagnostics;

namespace DistributedShare
{
	static public class ApplicationTraceSource
	{
		static private ActivitySource? instance;
		static public ActivitySource Instance  => instance!;

		static public void Configure(string name, string version)
		{
			instance = new ActivitySource(name, version);
		}
	}
}
