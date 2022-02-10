using Xunit;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Events.Tests;

public class StockEventsTests
{
    [Fact]
    public void Test1()
    {
        var stock = new Stock("APPL", 250);
        var traidingBot = new TraidingBot(stock);
        var emailNotifier = new EmailNotifier(stock);

        stock.Price = 350;
    }


    public delegate void PriceChangedHandler (decimal oldPrice, decimal newPrice);
    public class Stock
    {
        decimal price;

        public Stock (string symbol, decimal price)
        { 
            this.Symbol = symbol;
            this.price = price;
        }

        public event PriceChangedHandler PriceChanged;
        public decimal Price {
            get { return price; }
            set {
                    if (price == value) 
                    {
                        return;
                    }
                    
                    decimal oldPrice = price;
                    price = value;

                    PriceChanged?.Invoke(oldPrice, price);
                }
        }

        public string Symbol { get; }
    }

    public class TraidingBot
    {
        private readonly Stock stock;

        public TraidingBot(Stock stock)
        {
            this.stock = stock;
            stock.PriceChanged += MakeOrderIfNeeded;
        }

        private void MakeOrderIfNeeded(decimal oldPrice, decimal newPrice)
        {
            if (newPrice / oldPrice > 1.2M)
            {
                /// Create order on exchange
            }
        }
    }

    public class EmailNotifier
    {
        private readonly Stock stock;

        public EmailNotifier(Stock stock)
        {
            this.stock = stock;
            stock.PriceChanged += Notify;
        }

        private void Notify(decimal oldPrice, decimal newPrice)
        {
            var email = $"Price for stock {stock.Symbol} changed {oldPrice} -> {newPrice}";
            //
        }
    }
}