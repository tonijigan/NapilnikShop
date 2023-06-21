using System;
using System.Collections.Generic;

namespace NapilnikShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            warehouse.ShowInfo(); //Вывод всех товаров на складе с их остатком

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            cart.ShowInfo(); //Вывод всех товаров в корзине

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    class Shop
    {
        private Warehouse _warehouse;

        public string Paylink { get; private set; } = "Случайная строка";

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            Cart cart = new Cart();
            cart.Init(_warehouse);
            return cart;
        }
    }

    class Warehouse
    {
        private Dictionary<Good, int> _cells = new Dictionary<Good, int>();

        private const int MinCountProduct = 0;

        public Dictionary<Good, int> Cells => _cells;

        public void Delive(Good product, int count)
        {
            if (count <= MinCountProduct)
                return;

            _cells.Add(product, count);
        }

        public void ShowInfo()
        {
            Console.WriteLine("На складе:\n");

            foreach (var item in _cells)
                Console.WriteLine($"Продукт - {item.Key.ProductName} Колличество {item.Value}");
        }

        public bool TryGetCell(Good product, int countProducts)
        {
            if (_cells.ContainsKey(product) && _cells[product] > countProducts)
                return true;

            return false;
        }
    }

    class Cart
    {
        private Dictionary<Good, int> _cells = new Dictionary<Good, int>();
        private Warehouse _warehouse;

        public void Init(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Shop Order()
        {
            Shop shop = new Shop(_warehouse);
            return shop;
        }

        public void ShowInfo()
        {
            Console.WriteLine("\nВ корзине:\n");

            foreach (var item in _cells)
                Console.WriteLine($"Продукт - {item.Key.ProductName} Колличество {item.Value}");
        }

        public void Add(Good product, int count)
        {
            if (_warehouse.TryGetCell(product, count) == true)
            {
                RemoveDuplicate(product);
                _cells.Add(product, count);
                Console.WriteLine($"\nТовар {product.ProductName} " +
                 $"успешно добавлен в колличестве {count} штук\n");
            }
            else
            {
                Console.WriteLine("Ошибка - введено не верное количество или наименование товара");
            }
        }

        private void RemoveDuplicate(Good product)
        {
            foreach (var item in _cells)
            {
                if (item.Key == product)
                    _cells.Remove(product);
                return;
            }
        }
    }

    class Good
    {
        public string ProductName { get; private set; }

        public Good(string productName)
        {
            ProductName = productName;
        }
    }
}