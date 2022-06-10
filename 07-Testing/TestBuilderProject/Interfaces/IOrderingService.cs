namespace TestBuilderProject
{
	public interface IOrderingService
    {
        Order MapUserInfoIntoOrder(ApplicationUser user, Order order);
    }

}