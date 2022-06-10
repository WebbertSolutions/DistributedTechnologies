namespace DistributedShare
{
	public class Cart
	{
		public int CartID { get; init; }
		public List<CartItem> Items { get; set; } = new List<CartItem>();

		public Cart(int cartID)
		{
			CartID = cartID;
		}
	}

	public class CartItem
	{
		public int ItemID { get; set; }
		public string Item { get; set; }
		public int Count { get; set; }
		public decimal PriceEach { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		/// <summary>
		/// Public constructor for Deserialization of Messages and is not intended for general use
		/// </summary>
		public CartItem()

		{

		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


		public CartItem(int itemID, string item, int count, decimal price)
		{
			Item = item;
			Count = count;
			PriceEach = price;
			ItemID = itemID;
		}
	}
}
