using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private List<Cell> _cells = new List<Cell>();

        private const int MinCountProduct = 0;

        public List<Cell> Cells => _cells;

        public void Delive(Good product, int count)
        {
            if (count <= MinCountProduct)
                return;

            _cells.Add(new Cell(product, count));
        }

        public void ShowInfo()
        {
            Console.WriteLine("На складе:\n");

            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].ShowInfo();
                Console.WriteLine();
            }
        }
    }

    class Cart
    {
        private List<Cell> _cells = new List<Cell>();
        private Warehouse _warehouse = new Warehouse();

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

            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].ShowInfo();
            }
        }

        public void Add(Good product, int count)
        {
            if (TryGetCell(out Cell cell, product, count) == true)
            {
                _cells.Add(cell);
                Console.WriteLine($"\nТовар {cell.Good.ProductName} " +
                $"успешно добавлен в колличестве {cell.CountProduct} штук\n");
            }
            else
            {
                Console.WriteLine("Ошибка - нет необходимого колличества или нет товара");
            }
        }

        private bool TryGetCell(out Cell cell, Good productName, int countProduct)
        {
            cell = null;

            for (int i = 0; i < _warehouse.Cells.Count; i++)
            {
                if (_warehouse.Cells[i].Good == productName && _warehouse.Cells[i].CountProduct > countProduct)
                {
                    cell = new Cell(productName, countProduct);
                }
            }
            return cell != null;
        }
    }

    class Cell
    {
        public Good Good { get; private set; }
        public int CountProduct { get; private set; }

        public Cell(Good good, int countProduct)
        {
            Good = good;
            CountProduct = countProduct;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Товар - {Good.ProductName};\nКолличество - {CountProduct}");
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