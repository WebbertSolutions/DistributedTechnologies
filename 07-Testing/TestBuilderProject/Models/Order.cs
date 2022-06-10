namespace TestBuilderProject
{
	public class Order
    {
        public DateTime CardExpiration { get; set; }
        public string CardExpirationShort { get; set; } = string.Empty;

        public void CardExpirationShortFormat()
		{
			CardExpirationShort = CardExpiration.ToString("MM/yy");
		}
	}

}