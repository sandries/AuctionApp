namespace Auctionata.Runner
{
	public class BidIncrementLogic
	{
		public static decimal IncrementBid(decimal amount)
		{
			if (amount.Between(0, 10)) return amount + 1;
			if (amount.Between(10, 20)) return amount + 2;
			if (amount.Between(20, 40)) return amount + 5;
			if (amount.Between(40, 100)) return amount + 10;
			if (amount.Between(100, 200)) return amount + 20;
			if (amount.Between(200, 400)) return amount + 50;
			if (amount.Between(400, 1000)) return amount + 100;
			if (amount.Between(1000, 2000)) return amount + 200;
			if (amount.Between(2000, 4000)) return amount + 500;
			if (amount.Between(4000, 10000)) return amount + 1000;
			if (amount.Between(10000, 20000)) return amount + 2000;
			if (amount.Between(20000, 40000)) return amount + 4000;
			if (amount.Between(40000, 100000)) return amount + 10000;
			if (amount.Between(100000, 200000)) return amount + 20000;
			if (amount.Between(200000, 400000)) return amount + 50000;
			if (amount.Between(400000, 1000000000)) return amount + 100000;
			if (amount.Between(1000000000, 2000000000)) return amount + 200000;
			if (amount.Between(2000000000, 4000000000)) return amount + 400000;
			return amount + 500000;
		}
	}

	public static class DecimalExtention
	{
		public static bool Between(this decimal value, decimal a, decimal b)
		{
			return value >= a && value < b;
		}
	}
}