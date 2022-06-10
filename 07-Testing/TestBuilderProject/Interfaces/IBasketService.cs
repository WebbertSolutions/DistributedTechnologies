namespace TestBuilderProject
{
	public interface IBasketService
    {
        Task<Order> GetOrderDraft(string basketId);
    }

}