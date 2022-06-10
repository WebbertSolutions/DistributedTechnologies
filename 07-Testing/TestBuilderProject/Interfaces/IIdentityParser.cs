using System.Security.Principal;

namespace TestBuilderProject
{
	public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }

}