namespace Supermarket
{
    class Programm
    {
        static void Main()
        {
            Menu menu = new Menu();
            menu.ShowMenu();
        }
    }

    class Menu
    {
        public void ShowMenu()
        {
            const string MenuServeClient = "1";
            const string MenuCreateClientsQueue = "2";
            const string MenuExit = "0";

            bool isExit = false;
            string userInput;
            Supermarket supermarket = new Supermarket();

            while (isExit == false)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine(MenuServeClient + " - Обслужить клиента");
                Console.WriteLine(MenuCreateClientsQueue + " - Создать очередь клиентов");
                Console.WriteLine(MenuExit + " - Выход");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case MenuServeClient:
                        supermarket.ServeClient();
                        break;

                    case MenuCreateClientsQueue:
                        supermarket.CreateClients();
                        break;

                    case MenuExit:
                        isExit = true;
                        break;
                }
            }
        }
    }

    class Supermarket
    {
        private int _account;
        private List<Product> _storage = new List<Product>();
        private Queue<Client> _clients = new Queue<Client>();
        private Product[] _allProductTypes;
        private Random _random = new Random();

        public Supermarket()
        {
            CreateProducts();
            FillStorage();
        }

        public void CreateClients()
        {
            int clientsCount = 10;
            int clientsEntered = 0;
            int minMoney = 200;
            int maxMoney = 500;

            for (int i = 0; i < clientsCount; i++)
            {
                if (_storage.Count > 0)
                {
                    _clients.Enqueue(new Client(_random.Next(minMoney, maxMoney), _storage));
                    clientsEntered++;
                }
                else
                {
                    Console.WriteLine("На складе кончились товары");
                    i = clientsCount;
                }
            }

            Console.WriteLine($"В очередь встало {clientsEntered} клиентов");
            Console.WriteLine($"Всего клиентов в очереди - {_clients.Count()}");
        }

        public void ServeClient()
        {
            if (_clients.Count > 0)
            {
                bool isPayed = false;
                Client client = _clients.Peek();

                while (isPayed == false)
                {
                    int totalPrice = client.CountCartPrice(client);

                    if (client.Money >= totalPrice)
                    {
                        _account += client.Pay(totalPrice);
                        _clients.Dequeue();

                        if (totalPrice > 0)
                        {
                            Console.WriteLine($"Товары на сумму {totalPrice} оплачены, клиент обслужен.");
                        }
                        else
                        {
                            Console.WriteLine("Клиент ничего не купил");
                        }

                        Console.WriteLine($"Балланс магазина - {_account}");
                        Console.WriteLine($"Клиентов в очереди - {_clients.Count()}");
                        isPayed = true;
                    }
                    else
                    {
                        client.RemoveFromCart();
                    }
                }
            }
            else
            {
                Console.WriteLine("Очередь клиентов пуста");
            }
        }

        private void CreateProducts()
        {
            _allProductTypes = new Product[]
        {
            new Product("Хлеб", 50),
            new Product("Молоко", 70),
            new Product("Яйца", 40),
            new Product("картошка", 45),
            new Product("Сметана", 70),
            new Product("Булка", 40),
            new Product("Жир", 20),
            new Product("Пиво", 140),
            new Product("Куриный пупок", 40),
            new Product("Кислый пупс", 100),
            new Product("Пюрешка", 70),
            new Product("Салат ВсемРад", 110),
            new Product("Салат ТухлыйСмрад", 20),
            new Product("Салат ВесёлыйВлад", 120),
            new Product("Мясо Птицы", 80),
            new Product("Мясо Рыбы", 70),
            new Product("Ни рыба - ни мясо", 60),
            new Product("Колбаса копчёная", 130),
            new Product("Колбаса варёная", 135),
            new Product("Цыплёнок жареный", 110),
            new Product("Цыплёнок пареный", 115),
            new Product("Чай", 80),
            new Product("Кофе", 70),
            new Product("Потанцуем", 145)
        };
        }

        private void FillStorage()
        {
            for (int i = 0; i < _allProductTypes.Length; i++)
            {
                for (int j = 0; j < _random.Next(10, 100); j++)
                {
                    _storage.Add(_allProductTypes[i]);
                }
            }

            Console.WriteLine($"На складе {_storage.Count()} едениц продукции");
        }
    }

    class Client
    {
        public int Money { get; private set; }
        private List<Product> _shoppingCart = new List<Product>();
        private Random _random = new Random();

        public Client(int money, List<Product> productShelf)
        {
            Money = money;
            _shoppingCart = FillShoppingCart(productShelf);
        }

        public List<Product> FillShoppingCart(List<Product> productShelf)
        {
            List<Product> shoppingCart = new List<Product>();
            int minBuy = 0;
            int maxBuy = 10;

            if (productShelf.Count > 0)
            {
                for (int j = 0; j < _random.Next(minBuy, maxBuy); j++)
                {
                    Product product = TakeProductFromSrorage(productShelf, _random.Next(0, productShelf.Count));

                    if (product != null)
                    {
                        shoppingCart.Add(product);
                    }
                    else
                    {
                        return null;
                    }
                }

                return shoppingCart;
            }
            else
            {
                return null;
            }
        }

        public int CountCartPrice(Client client)
        {
            int totalPrice = 0;

            if (client._shoppingCart.Count > 0)
            {
                for (int i = 0; i < client._shoppingCart.Count(); i++) //возможно ничего не будет в корзине добавить проверку
                {
                    totalPrice += client._shoppingCart[i].Price;
                }
                return totalPrice;
            }
            else
            {
                Console.WriteLine("В корзине пусто");
                return totalPrice;
            }
        }

        public void RemoveFromCart()
        {
            int itemIndex = _random.Next(0, _shoppingCart.Count());
            Console.WriteLine($"У клиента не хватило денег, товар - \"{_shoppingCart[itemIndex].Name}\" выложен из корзины");
            _shoppingCart.RemoveAt(itemIndex);
        }

        public int Pay(int price)
        {
            if (Money >= price)
            {
                Money -= price;
                return price;
            }
            else
            {
                Console.WriteLine("Не хватает денег");
                return 0;
            }
        }

        private Product TakeProductFromSrorage(List<Product> productShelf, int index)
        {
            if (productShelf.Count() > 0)
            {
                Product product = productShelf[index];
                productShelf.RemoveAt(index);
                return product;
            }
            else
            {
                return null;
            }
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }
    }
}