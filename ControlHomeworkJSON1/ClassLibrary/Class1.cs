// Самсонов Артём Арменович БПИ238-2. Вариант 12.
namespace ClassLibrary;

using System.Text;

/// <summary>
/// Класс, описывающий отдельный объект JSON файла с данными о магазине.
/// </summary>
public class Store
{
    /// <summary>
    /// Id магазина. По умолчанию 0.
    /// </summary>
    private readonly int _storeId = 0;

    /// <summary>
    /// Название магазина. По умолчанию "unknown".
    /// </summary>
    private readonly string _storeName = "unknown";

    /// <summary>
    /// Местоположение магазина. По умолчанию "unknown".
    /// </summary>
    private readonly string _location = "unknown";

    /// <summary>
    /// Работники магазина. По умолчанию null.
    /// </summary>
    private readonly string[] _employees = null;

    /// <summary>
    /// Продукты, которые продаются в магазине. По умолчанию null.
    /// </summary>
    private readonly string[] _products = null;

    /// <summary>
    /// Id магазина доступно только для чтения.
    /// </summary>
    public int StoreId
    {
        get => _storeId;
    }

    /// <summary>
    /// Название магазина. доступно только для чтения.
    /// </summary>
    public string StoreName
    {
        get => _storeName;
    }

    /// <summary>
    /// Локация магазина доступно только для чтения.
    /// </summary>
    public string Location
    {
        get => _location;
    }

    /// <summary>
    /// Работники магазина доступно только для чтения.
    /// </summary>
    public string[] Employees
    {
        get => _employees;
    }

    /// <summary>
    /// Продукты, которые продаются в магазине доступны только для чтения.
    /// </summary>
    public string[] Products
    {
        get => _products;
    }

    /// <summary>
    /// Конструктор класса Store по умолчанию. Пример создания экземпляра класса через него: Store store = new Store().
    /// </summary>
    public Store() { }

    /// <summary>
    /// Полноценный конструктор класса Store. Пример создания экземпляра класса через него: Store store = new Store(storeId: storeId, storeName: storeName, location: location, employees: employees, products: products).
    /// </summary>
    /// <param name="storeId">Id магазина. Переменная типа int.</param>
    /// <param name="storeName">Название магазина. Переменная строкового типа.</param>
    /// <param name="location">Локация магазина. Переменная строкового типа.</param>
    /// <param name="employees">Работники магазина. Переменная типа массива строк.</param>
    /// <param name="products">Продукты, которые продаются в магазине. Переменная типа массива строк.</param>
    public Store(int storeId, string storeName, string location, string[] employees, string[] products)
    {
        _storeId = storeId;
        _storeName = storeName;
        _location = location;
        _employees = employees;
        _products = products;
    }

    /// <summary>
    /// Метод, преобразующий массив строк в строку, в которой элементы массива разделены \n (перенос на следующую строку).
    /// </summary>
    /// <param name="strings">Исходный массив строк, который надо преобразовать в одну строку.</param>
    /// <returns>Строка, состоящая из нескольких строчек, разделенных \n.</returns>
    private string StringConverter(string[] strings)
    {
        StringBuilder result = new StringBuilder();
        int tmp = 0;
        foreach (string @string in strings)
        {
            if (tmp != strings.Length - 1)
            {
                result.Append($"\t{@string},\n");
                ++tmp;
            }
            else
            {
                result.Append($"\t{@string}.\n");
                ++tmp;
            }
        }
        return result.ToString();
    }

    /// <summary>
    /// Метод позволяет получить полную информацию о экземпляре класса Store.  
    /// </summary>
    /// <returns>Строка с информацией о состоянии класса Store(Store Id, Store Name, Store Location, Employees, Products).</returns>
    public override string ToString()
    {
        return $"Store Id: {StoreId}\nStore Name: {StoreName}\nStore Location: {Location}\nEmployees:\n{StringConverter(_employees)}Products:\n{StringConverter(_products)}";
    }
}

/// <summary>
/// Статический класс, позволяющий работать с данными в JSON файле. 
/// </summary>
public static class JsonParser
{
    /// <summary>
    /// Выводит JSON файл.
    /// </summary>
    public static void WriteJson(List<Store> stores)
    {
        Console.WriteLine("[");
        for (int i = 0; i < stores.Count; ++i)
        {
            Console.WriteLine("  {");
            Console.WriteLine($"    \"store_id\": {stores[i].StoreId},");
            Console.WriteLine($"    \"store_name\": \"{stores[i].StoreName}\",");
            Console.WriteLine($"    \"location\": \"{stores[i].Location}\",");
            Console.WriteLine("    \"employees\": [");
            ArreyToStrings(stores[i].Employees);
            Console.WriteLine($"    ],");
            Console.WriteLine("    \"products\": [");
            ArreyToStrings(stores[i].Products);
            Console.WriteLine("    ]");
            if (i != stores.Count - 1)
            {
                Console.WriteLine("  },");
            }
            else
            {
                Console.WriteLine("  }");
                Console.WriteLine("]");
            }
        }
    }


    public static void ArreyToStrings(string[] arr)
    {
        for (int i = 0; i < arr.Length; ++i)
        {
            if (i != arr.Length - 1)
            {
                Console.WriteLine($"      \"{arr[i]}\",");
            }
            else
            {
                Console.WriteLine($"      \"{arr[i]}\"");
            }
        }
    }

    /// <summary>
    /// Считывает JSON файл из потока System.Console.
    /// </summary>
    public static List<Store> ReadJson(ref List<Store> stores)
    {
        // Набор значений, по которым можно определить что хранит в себе строка (или строки после этой строки).
        string storeIdJSON = "store_id";
        string storeNameJSON = "store_name";
        string storeLocationJSON = "location";
        string storeEmployeesJSON = "employees";
        string storeProductsJSON = "products";

        // Набор временных переменных, которые нужны для того, чтобы создать объект типа Store с данными из JSON файла.
        int storeId = 0;
        string storeName = null;
        string storeLocation = null;
        List<string> storeEmployees = new List<string>();
        List<string> storeProducts = new List<string>();

        // Переменная, содержащая строку JSON файла.
        string str = null;

        // Цикл для построчного считывания JSON файла.
        do
        {
            // Считываем новую строку.
            str = Console.ReadLine();

            // Набор ветвлений, которые по ключевому слову позволяют понять, что находится в строке, и извлечь нужные данные из этой строки.
            if (str.Contains(storeIdJSON))
            {
                storeId = Getsomething.GetId(str);
            }
            else if (str.Contains(storeNameJSON))
            {
                storeName = Getsomething.GetString(str);
            }
            else if (str.Contains(storeLocationJSON))
            {
                storeLocation = Getsomething.GetString(str);
            }
            else if (str.Contains(storeEmployeesJSON))
            {
                storeEmployees = Getsomething.GetList();
            }
            else if (str.Contains(storeProductsJSON))
            {
                storeProducts = Getsomething.GetList();

                // "Products" - это последние данные из одного объекта в JSON файле. После этого можно создавать полноценный объект типа Store. Делаем проверку на корректность файла, чтобы все нужные данные присутствовали.
                if (storeId != default && storeName != default && storeLocation != default && storeEmployees.Count > 0 && storeProducts.Count > 0)
                {
                    stores.Add(new Store(storeId, storeName, storeLocation, storeEmployees.ToArray(), storeProducts.ToArray()));
                }
                else
                {
                    throw new ArgumentException("Некорректный файл.");
                }
            }
        }
        // Последняя строка в JSON файле это "]".
        while (str != "]");

        return stores;
    }
}

/// <summary>
/// Статический класс со статическими методами, позволяющими работать с JSON файлами.
/// </summary>
public static class JSONFile
{
    /// <summary>
    /// Статический метод, извлекающий JSON файл через поток System.Console.
    /// </summary>
    /// <param name="filePath">Полный путь JSON файла.</param>
    /// <returns>Лист объектов типа Store. Каждый объект типа Store заполнен полной информацией о себе. Метод извлекает из JSON файла только нужную информацию, и преобразует ее в объект типа Store.</returns>
    public static List<Store> ReadFile(string filePath) 
    {
        List<Store> stores = new List<Store>();

        // Нахождение файла по его полному пути и работа с ним.
        using (StreamReader fileInput = new StreamReader(filePath))
        {
            TextReader originalInput = Console.In;
            // Смена потока на чтение System.Console на чтение файла (а не чтение из консоли как обычно).
            Console.SetIn(fileInput);

            // Извлечение информации из файла.
            JsonParser.ReadJson(ref stores);

            // Возвращение стандартного потока на чтение.
            Console.SetIn(originalInput);
        }

        return stores;
    }

    /// <summary>
    /// Статический метод, извлекающий JSON файл из консольного ввода пользователем.
    /// </summary>
    /// <returns>Лист объектов типа Store. Каждый объект типа Store заполнен полной информацией о себе. Метод извлекает из JSON файла только нужную информацию, и преобразует ее в объект типа Store.</returns>
    public static List<Store> ReadConsole()
    {
        List<Store> stores = new List<Store>();
        Console.WriteLine("Введите JSON файл. Чтобы завершить ввод файла, введите строку, состоящую из одного символа \"]\").\nОбязательно соблюдайте структуру JSON файла!");

        // Извлечение информации из файла.
        try
        {
            JsonParser.ReadJson(ref stores);
        }
        catch (Exception)
        {
            Console.WriteLine("Некорректный файл");
            Menu.Menu1();
        }

        return stores;
    }

    /// <summary>
    /// Статические метод, выводящий информацию JSON объектов в файл.
    /// </summary>
    public static void WriteFile(List<Store> stores)
    {
        string filePath = ConsoleInput.InputString("Путь к файлу, в котором хотите сохранить данные.");

        if (File.Exists(filePath))
        {
            Console.WriteLine("Файл существует и будет перезаписан.");
        }
        else
        {
            Console.WriteLine("Файл создан.");
        }

        // Нахождение файла по его полному пути и работа с ним.
        using (StreamWriter fileInput = new StreamWriter(filePath))
        {
            TextWriter originalInput = Console.Out;
            // Смена потока на чтение System.Console на чтение файла (а не чтение из консоли как обычно).
            Console.SetOut(fileInput);

            // Извлечение информации из файла.
            JsonParser.WriteJson(stores);

            // Возвращение стандартного потока на чтение.
            Console.SetOut(originalInput);
        }
        // Переход к основному меню.
        Menu.MenuMain(stores);
    }

    /// <summary>
    /// Статический метод, позволяющий выводить список объектов вместе с их информацией в консоль.
    /// </summary>
    /// <param name="stores">Список, который необходимо вывести в консоль.</param>
    public static void ConsoleWriter(List<Store> stores)
    {
        // Временная переменна для подсчета количества выведенных строк.
        int tmp = 0;

        Console.WriteLine("\nВывод данных:\n");

        foreach (Store store in stores)
        {
            // Вывод информации об объекте. Одна итерация = один выведенный объект, однако так как store.ToString() выводит данные с разделителями строк ('\n'), одна итерация != одна строка.
            Console.WriteLine(store.ToString());
            tmp++;

            // Консоль вмещает в себя именно 31 объектов типа Store.
            if (tmp % 31 == 0)
            {
                // Переменная для сохранения выбора пользователя.
                ConsoleKey choise;
                do
                {
                    Console.Write("Из-за ограничения на количество строк, консоль не может вместить в себя больше объектов.\n" +
                        "Нажмите:\n" +
                        "1 - Очистить консоль и продолжить вывод.\n" +
                        "2 - Остановить вывод.\n" +
                        "Ваш выбор: ");
                    choise = Console.ReadKey().Key;
                    Console.WriteLine("\nВывод данных:\n");

                    if (choise == ConsoleKey.D1)
                    {
                        Console.Clear();
                        break;
                    }
                    else if (choise == ConsoleKey.D2)
                    {
                        break;
                    }
                }
                while (true);

                // Проверка для окончательного выхода из цикла и остановки ввода.
                if (choise == ConsoleKey.D2)
                {
                    break;
                }
            }
        }
        // Переход к основному меню.
        Menu.MenuMain(stores);
    }
}

/// <summary>
/// Статический класс с набором статических методов, которые позволяют извлекать из JSON файла какие либо данные.
/// </summary>
public static class Getsomething
{
    /// <summary>
    /// Статический метод, позволяющий получить из JSON файла ID конкретного объекта.
    /// </summary>
    /// <param name="str">Строка, из которой надо извлечь ID объекта.</param>
    /// <returns>ID объекта</returns>
    public static int GetId(string str)
    {
        // Ищем индексы начала и конца ID в строке.
        int startIndex = str.IndexOf(":") + 2;
        int endIndex = str.LastIndexOf(",");

        // "Извлекаем" ID объекта из строки.
        string ID = str.Substring(startIndex, endIndex - startIndex);

        // Преобразуем ID из типа string в тип int.
        if (int.TryParse(ID, out int result))
        {
            return result;
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Статический метод, позволяющий получить из JSON файла название какого либо объекта, которое записано в одной строке (конкретно для файла в варианте 12: название магазина и локация магазина. 
    /// </summary>
    /// <param name="str">Строка, из которой надо извлечь данные.</param>
    /// <returns>Строка с нужным названием.</returns>
    public static string GetString(string str)
    {
        // Ищем индексы начала и конца названия объекта в строке.
        int startIndex = str.IndexOf(":") + 3;
        int endIndex = str.LastIndexOf("\"");

        // "Извлекаем" название объекта из строки.
        string result = str.Substring(startIndex, endIndex - startIndex);

        return result;
    }

    /// <summary>
    /// Статический метод, позволяющий получить из JSON файла список каких либо объектов (конкретно для файла в варианте 12: список названий продуктов в магазине и список работников магазина.
    /// </summary>
    /// <returns>Список типа string каких либо объектов из JSON файла. </returns>
    public static List<string> GetList()
    {
        List<string> result = new List<string>();
        string str = Console.ReadLine();

        do
        {
            // Ищем индексы начала и конца названия объекта в строке.
            int startIndex = str.IndexOf("\"");
            int endIndex = str.LastIndexOf("\"");

            // "Извлекаем" название объекта из строки.
            string product = str.Substring(startIndex + 1, endIndex - startIndex - 1);

            // Объектов много, поэтому складируем их названия в списке.
            result.Add(product);

            // Переход на новую строку.
            str = Console.ReadLine();
        }
        // Список объектов в JSON файле заканчивается строкой "    ]\n".
        while (!str.Contains("]"));

        return result;
    }
}

public delegate bool Comp(Store store1, Store store2);

/// <summary>
/// Статический класс со статическими методами, представляющий меню взаимодействия пользователя с программой.
/// </summary>
public static class Menu
{
    /// <summary>
    /// Статический метод, представляющий первый блок меню - ввод JSON файла в программу.
    /// </summary>
    public static void Menu1()
    {
        // Основной список, с которым будем работать в течение всей программы.
        List<Store> stores = null;

        // Цикл для проверки корректности ввода.
        while (true)
        {
            // Очистка консоли от прошлый неудачных итераций (попыток ввода).

            Console.Write("Начало программы. Меню.\n" +
                "Нажмите:\n" +
                "1 - вручную ввести данные в консоль.\n" +
                "2 - считать JSON файл автоматически.\n" +
                "Ваш выбор: ");
            var choise = Console.ReadKey().Key;
            Console.WriteLine();

            // Ручной ввод данных.
            if (choise == ConsoleKey.D1)
            {
                stores = JSONFile.ReadConsole();
                Console.WriteLine("\nГотово, данные успешно считаны с консоли!\n");
                break;
            }
            // Автоматическое считывание файла.
            else if (choise == ConsoleKey.D2)
            {
                try
                {
                    stores = JSONFile.ReadFile(ConsoleInput.GetFullPath());
                }
                catch (Exception)
                {
                    Console.WriteLine("Пиздец");
                    return;
                }
                Console.WriteLine("\nГотово, файл успешно считан!\n");
                break;
            }
        }

        // Переход к новому блоку меню.
        MenuMain(stores);
    }

    /// <summary>
    /// Основное меню, которое предоставляет возможности фильтрации, сортировки, вывода данных, сохранения их в файл, перезапуска и завершения программы.
    /// </summary>
    /// <param name="stores">Список магазинов, с которым надо произвести какие либо действия.</param>
    public static void MenuMain(List<Store> stores)
    {
        // Цикл для корректного ввода данных.
        do
        {
            // Очистка консоли от прошлых итераций.
            Console.Clear();

            Console.Write("Нажмите:\n" +
                "1 - отфильтровать данные по определенному значению.\n" +
                "2 - отсортировать данные по определенному значению.\n" +
                "3 - вывести данные в консоль.\n" +
                "4 - сохранить данные в файл.\n" +
                "5 - перезапустить программу.\n" +
                "Escape - завершить программу.\n" +
                "Ваш выбор: ");
            var choise = Console.ReadKey().Key;
            Console.WriteLine();

            if (choise == ConsoleKey.D1)
            {
                Filtering.FilteringMenu(ref stores);
                break;
            }
            else if (choise == ConsoleKey.D2)
            {
                Sorting.SortingMenu(ref stores);
                break;
            }
            else if (choise == ConsoleKey.D3)
            {
                ConsoleOutput.ConsoleWriter(stores);
                break;
            }
            else if (choise == ConsoleKey.D4)
            {
                JSONFile.WriteFile(stores);
                break;
            }
            else if (choise == ConsoleKey.D5)
            {
                Menu1();
                break;
            }

            // Реализация завершения программы с помощью выбрасывания исключения.
            try
            {
                if (choise == ConsoleKey.Escape)
                {
                    throw new ArgumentException("Завершение программы");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                break;
            }
        }
        while (true);
    }
}

public static class ConsoleOutput
{
    /// <summary>
    /// Статический метод, позволяющий выводить список объектов вместе с их информацией в консоль.
    /// </summary>
    /// <param name="stores">Список, который необходимо вывести в консоль.</param>
    public static void ConsoleWriter(List<Store> stores)
    {
        // Временная переменна для подсчета количества выведенных строк.
        int tmp = 0;

        Console.WriteLine("\nВывод данных:\n");

        foreach (Store store in stores)
        {
            // Вывод информации об объекте. Одна итерация = один выведенный объект, однако так как store.ToString() выводит данные с разделителями строк ('\n'), одна итерация != одна строка.
            Console.WriteLine(store.ToString());
            tmp++;

            // Консоль вмещает в себя именно 31 объектов типа Store.
            if (tmp % 31 == 0)
            {
                // Переменная для сохранения выбора пользователя.
                ConsoleKey choise;
                do
                {
                    Console.Write("Из-за ограничения на количество строк, консоль не может вместить в себя больше объектов.\n" +
                        "Нажмите:\n" +
                        "1 - Очистить консоль и продолжить вывод.\n" +
                        "2 - Остановить вывод.\n" +
                        "Ваш выбор: ");
                    choise = Console.ReadKey().Key;
                    Console.WriteLine("\nВывод данных:\n");

                    if (choise == ConsoleKey.D1)
                    {
                        Console.Clear();
                        break;
                    }
                    else if (choise == ConsoleKey.D2)
                    {
                        break;
                    }
                }
                while (true);

                // Проверка для окончательного выхода из цикла и остановки ввода.
                if (choise == ConsoleKey.D2)
                {
                    break;
                }
            }
        }
        // Переход к основному меню.
        Menu.MenuMain(stores);
    }
}

/// <summary>
/// Статический класс со статическими методами для чтения информации с консоли.
/// </summary>
public static class ConsoleInput
{
    /// <summary>
    /// Статический метод для считывания данных типа string с консоли.
    /// </summary>
    /// <param name="name">Имя переменной, которую будет видеть пользователь при считывании.</param>
    /// <returns>Введенное пользователем значение переменной.</returns>
    public static string InputString(string name)
    {
        string result;

        // Цикл для проверки ввода на корректность.
        while (true)
        {
            Console.Write($"{name} = ");
            result = Console.ReadLine();

            // result = null только если пользователь ничего не ввел в консоль и нажал Enter.
            if (!string.IsNullOrEmpty(result.Trim()))
            {
                break;
            }
            else
            {
                Draw.ChangeBadAllColor();
                Console.WriteLine("Не оставляйте поле без ответа!");
                Draw.ChangeGoodForegroundColor();
            }
        }

        return result;
    }

    /// <summary>
    /// Статический метод для считывания данных типа int с консоли.
    /// </summary>
    /// <param name="name">Имя переменной, которую будет видеть пользователь при считывании.</param>
    /// <returns>Введенное пользователем значение переменной.</returns>
    public static int InputInt(string name)
    {
        int result;

        // Цикл для проверки ввода на корректность.
        while (true)
        {
            Console.Write($"{name} = ");

            // Непосредственно проверка корректности ввода.
            if (int.TryParse(Console.ReadLine(), out result))
            {
                break;
            }
            else
            {
                Draw.ChangeBadAllColor();
                Console.WriteLine("Некорректный ввод!");
                Draw.ChangeGoodForegroundColor();
            }
        }
        return result;
    }

    /// <summary>
    /// Статический метод для считывания полного пути к файлу (В нашем случае - к JSON файлу).
    /// </summary>
    /// <returns>Возвращает полный путь к файлу в виде строки.</returns>
    public static string GetFullPath()
    {
        string filePath;
        while (true)
        {
            filePath = InputString("Полный путь к JSON файлу");
            if (File.Exists(filePath))
            {
                break;
            }
            else
            {
                Draw.ChangeBadAllColor();
                Console.WriteLine("По заданному пути файл не найден. Повторите попытку.");
                Draw.ChangeGoodForegroundColor();
            }
        }
        return filePath;
    }
}

/// <summary>
/// Статический класс со статическими методами для реализации различных способок фильтрации.
/// </summary>
public static class Filtering
{
    /// <summary>
    /// Статический метод, представляющий меню для общения с пользователем, которое вызывается перед фильтрованием. Оно определяет, какое именно фильтрование нужно пользователю.
    /// </summary>
    /// <param name="stores">Список, который надо отфильтровать. Передается по ссылке с модификатором ref.</param>
    public static void FilteringMenu(ref List<Store> stores)
    {
        // Цикл для обработки некорректных ответов пользователя.
        while (true)
        {
            Comp comp = null;

            // Очистка консоли от предыдущих некорректных ответов пользователя.
            Console.Clear();

            Console.Write("Нажмите:\n" +
                "I - отфильтровать данные по значениям \"store_id\".\n" +
                "N - отфильтровать данные по значениям \"store_name\".\n" +
                "L - отфильтровать данные по значениям \"location\".\n" +
                "E - отфильтровать данные по значениям \"employees\".\n" +
                "P - отфильтровать данные по значениям \"products\".\n" +
                "Ваш выбор: ");
            var choise = Console.ReadKey().Key;
            Console.WriteLine("\n");
            // Ветвление, определяющее сортировку по убыванию или возрастанию.
            if (choise == ConsoleKey.I)
            {
                int id = ConsoleInput.InputInt("Значение \"store_id\", по которому вы хотите отфильтровать");
                FilterStores(ref stores, store => store.StoreId == id);
                break;
            }
            else if (choise == ConsoleKey.N)
            {
                string name = ConsoleInput.InputString("Значение \"store_name\", по которому вы хотите отфильтровать");
                FilterStores(ref stores, store => store.StoreName == name);
                break;
            }
            else if (choise == ConsoleKey.L)
            {
                string location = ConsoleInput.InputString("Значение \"store_id\", по которому вы хотите отфильтровать");
                FilterStores(ref stores, store => store.Location == location);
                break;
            }
            else if (choise == ConsoleKey.E)
            {
                string employee = ConsoleInput.InputString("Значение \"store_id\", по которому вы хотите отфильтровать");
                FilterStores(ref stores, store => store.Employees.Contains(employee));
                break;
            }
            else if (choise == ConsoleKey.P)
            {
                string product = ConsoleInput.InputString("Значение \"store_id\", по которому вы хотите отфильтровать");
                FilterStores(ref stores, store => store.Products.Contains(product));
                break;
            }
        }

        // Возврат в основное меню.
        Menu.MenuMain(stores);
    }

    /// <summary>
    /// Статический метод для фильтрации списка по заданному условию.
    /// </summary>
    /// <param name="stores">Список, который нужно отфильтровать.</param>
    /// <param name="condition">Условие фильтрации в виде лямбда-выражения.</param>
    public static void FilterStores(ref List<Store> stores, Func<Store, bool> condition)
    {
        List<Store> result = new List<Store>();

        for (int i = 0; i < stores.Count; ++i)
        {
            // Проверяем, выполняется ли условие фильтрации для текущего магазина
            if (condition(stores[i]))
            {
                // Если условие выполняется, добавляем магазин в результат
                result.Add(stores[i]);
            }
        }

        stores = result;
    }
}

/// <summary>
/// Статический класс со статическими методами для реализации различных способок сортировки.
/// </summary>
public static class Sorting
{
    /// <summary>
    /// Статический метод, представляющий меню для общения с пользователем, которое вызывается перед конкретной сортировкой. Оно определяет, какая именно сортировка нужна пользователю.
    /// </summary>
    /// <param name="stores">Список, который надо отсортировать. Передается по ссылке с модификатором ref.</param>
    public static void SortingMenu(ref List<Store> stores)
    {
        // Цикл для обработки некорректных ответов пользователя.
        while (true)
        {
            Comp comp = null;

            // Очистка консоли от предыдущих некорректных ответов пользователя.
            Console.Clear();

            Console.Write("Нажмите:\n" +
            "1 - отсортировать данные по возрастанию какого то значения.\n" +
            "2 - отсортировать данные по убыванию какого то значения.\n" +
            "Ваш выбор: ");
            var choise = Console.ReadKey().Key;
            Console.WriteLine("\n");
            // Ветвление, определяющее сортировку по убыванию или возрастанию.
            if (choise == ConsoleKey.D1)
            {
                // Выбор конкретного значения сортировки.
                SortingComp(ref comp);

                // Сортировка списка по возрастанию по переданному правилу.
                SortingSome(ref stores, comp);

                break;
            }
            else if (choise == ConsoleKey.D2)
            {
                // Выбор конкретного значения сортировки.
                SortingComp(ref comp);

                // Сортировка списка по убыванию по переданному правилу.
                SortingSomeReverse(ref stores, comp);

                break;
            }
        }

        // Возврат в основное меню.
        Menu.MenuMain(stores);
    }

    /// <summary>
    /// Статический метод, определяющий конкретное выбор конретного значения, по которому будет проводиться сортировка.
    /// </summary>
    /// <param name="comp">Компаратор, определяющий правила сортировки.</param>
    public static void SortingComp(ref Comp comp)
    {
        // Цикл для обработки некорректных ответов пользователя.
        while (true)
        {
            Console.Write("Нажмите:\n" +
        "I - отсортировать данные по \"store_id\".\n" +
        "N - отсортировать данные по \"store_name\".\n" +
        "L - отсортировать данные по \"location\".\n" +
        "E - отсортировать данные по \"employees\".\n" +
        "P - отсортировать данные по \"products\".\n" +
        "Ваш выбор: ");

            var tmp = Console.ReadKey().Key;

            // Ветвление, которое определяет, какое правило сортировки достанется делегату.
            if (tmp == ConsoleKey.I)
            {
                comp = ComparatorId;
                break;
            }
            else if (tmp == ConsoleKey.N)
            {
                comp = ComparatorName;
                break;
            }
            else if (tmp == ConsoleKey.L)
            {
                comp = ComparatorLocation;
                break;
            }
            else if (tmp == ConsoleKey.E)
            {
                comp = ComparatorEmployees;
                break;
            }
            else if (tmp == ConsoleKey.P)
            {
                comp = ComparatorProducts;
                break;
            }
        }
    }

    /// <summary>
    /// Статический метод сортировки списка по возрастанию объектов. Реализуется по определенным правилам, передающимся в компараторе.
    /// </summary>
    /// <param name="stores">Изначальный список, который надо отсортировать.</param>
    /// <param name="comp">Компаратор (в виде делегета), который задает правила сортировки объекта. Методы внутри делегата возращают значение bool.</param>
    public static void SortingSome(ref List<Store> stores, Comp comp)
    {
        // Временная переменная для оптимизации сортировки.
        bool swapped;

        for (int i = 0; i < stores.Count - 1; i++)
        {

            swapped = false;

            for (int j = 0; j < stores.Count - i - 1; j++)
            {
                if (comp(stores[j], stores[j + 1]))
                {
                    // Перестановка объектов.
                    Store temp = stores[j];
                    stores[j] = stores[j + 1];
                    stores[j + 1] = temp;

                    swapped = true;
                }
            }

            // Оптимизация: если во внутреннем цикле не было перестановок, значит, сортировка окончена.
            if (!swapped)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Статический метод сортировки списка по убыванию объектов. Реализуется по определенным правилам, передающимся в компараторе.
    /// </summary>
    /// <param name="stores">Изначальный список, который надо отсортировать.</param>
    /// <param name="comp">Компаратор (в виде делегета), который задает правила сортировки объекта. Методы внутри делегата возращают значение bool.</param>
    public static void SortingSomeReverse(ref List<Store> stores, Comp comp)
    {
        // Временная переменная для оптимизации сортировки.
        bool swapped;

        for (int i = 0; i < stores.Count - 1; i++)
        {
            swapped = false;

            for (int j = 0; j < stores.Count - i - 1; j++)
            {
                // Изначально компаратор возвращает bool значение для сортировки по возрастнанию, чтобы реализовать сортировку по убыванию - делаем отрицание от этого значения.
                if (!comp(stores[j], stores[j + 1]))
                {
                    // Перестановка объектов.
                    Store temp = stores[j];
                    stores[j] = stores[j + 1];
                    stores[j + 1] = temp;

                    swapped = true;
                }
            }

            // Оптимизация: если во внутреннем цикле не было перестановок, значит, сортировка окончена.
            if (!swapped)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Статический метод, представляющий компаратор для сравнения двух объектов по полю "Id". Сравнение происходит на основе числового значения переменных.
    /// </summary>
    /// <param name="store1">Первый объект </param>
    /// <param name="store2">Второй объект</param>
    /// <returns>true, если первый элемент больше второго, и false в противном случае.</returns>
    public static bool ComparatorId(Store store1, Store store2)
    {
        return store1.StoreId > store2.StoreId;
    }

    /// <summary>
    /// Статический метод, представляющий компаратор для сравнения двух объектов по полю "Name". Сравнение происходит на лексикографической основе.
    /// </summary>
    /// <param name="store1">Первый объект </param>
    /// <param name="store2">Второй объект</param>
    /// <returns>true, если первый элемент больше второго, и false в противном случае.</returns>
    public static bool ComparatorName(Store store1, Store store2)
    {
        return string.Compare(store1.StoreName, store2.StoreName) > 0;
    }

    /// <summary>
    /// Статический метод, представляющий компаратор для сравнения двух объектов по полю "Location". Сравнение происходит на лексикографической основе.
    /// </summary>
    /// <param name="store1">Первый объект </param>
    /// <param name="store2">Второй объект</param>
    /// <returns>true, если первый элемент больше второго, и false в противном случае.</returns>
    public static bool ComparatorLocation(Store store1, Store store2)
    {
        return string.Compare(store1.Location, store2.Location) > 0;
    }

    /// <summary>
    /// Статический метод, представляющий компаратор для сравнения двух объектов по полю "Employees". Сравнение происходит на основе количества объектов внутри массива "Employees".
    /// </summary>
    /// <param name="store1">Первый объект </param>
    /// <param name="store2">Второй объект</param>
    /// <returns>true, если первый элемент больше второго, и false в противном случае.</returns>
    public static bool ComparatorEmployees(Store store1, Store store2)
    {
        return store1.Employees.Length > store2.Employees.Length;
    }

    /// <summary>
    /// Статический метод, представляющий компаратор для сравнения двух объектов по полю "Products". Сравнение происходит на основе количества объектов внутри массива "Products".
    /// </summary>
    /// <param name="store1">Первый объект </param>
    /// <param name="store2">Второй объект</param>
    /// <returns>true, если первый элемент больше второго, и false в противном случае.</returns>
    public static bool ComparatorProducts(Store store1, Store store2)
    {
        return store1.Products.Length > store2.Products.Length;
    }
}

/// <summary>
/// Статический класс со статическими методами для рисования красивых фигур в консоли. К техническому заданию никак не относится.
/// </summary>
public static class Draw
{
    /// <summary>
    /// Статический метод для рисования четырех сердечек.
    /// </summary>
    public static void DrawingHeart()
    {
        // Смена цветов консоли. В итоге получается тёмно-красный шрифт на белом фоне.
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.BackgroundColor = ConsoleColor.White;

        // Сам рисунок. \u2764 - это символ сердечка в кодировке ASCII.
        Console.WriteLine("*************************************************************************************************************************************".Replace('*', '\u2764'));
        Console.WriteLine("                                                                                                                                     ");
        Console.WriteLine("     ******         ******             ******         ******             ******         ******             ******         ******     ".Replace('*', '\u2764'));
        Console.WriteLine("   **********     **********         **********     **********         **********     **********         **********     **********   ".Replace('*', '\u2764'));
        Console.WriteLine(" ************** **************     ************** **************     ************** **************     ************** ************** ".Replace('*', '\u2764'));
        Console.WriteLine("*******************************   *******************************   *******************************   *******************************".Replace('*', '\u2764'));
        Console.WriteLine(" *****************************     *****************************     *****************************     ***************************** ".Replace('*', '\u2764'));
        Console.WriteLine("  ***************************       ***************************       ***************************       ***************************  ".Replace('*', '\u2764'));
        Console.WriteLine("   *************************         *************************         *************************         *************************   ".Replace('*', '\u2764'));
        Console.WriteLine("    ***********************           ***********************           ***********************           ***********************    ".Replace('*', '\u2764'));
        Console.WriteLine("     *********************             *********************             *********************             *********************     ".Replace('*', '\u2764'));
        Console.WriteLine("       *****************                 *****************                 *****************                 *****************       ".Replace('*', '\u2764'));
        Console.WriteLine("         *************                     *************                     *************                     *************         ".Replace('*', '\u2764'));
        Console.WriteLine("           *********                         *********                         *********                         *********           ".Replace('*', '\u2764'));
        Console.WriteLine("             *****                             *****                             *****                             *****             ".Replace('*', '\u2764'));
        Console.WriteLine("               *                                 *                                 *                                 *               ".Replace('*', '\u2764'));
        Console.WriteLine("*************************************************************************************************************************************".Replace('*', '\u2764'));

        Console.ResetColor();
    }

    /// <summary>
    /// Статический метод, меняющий цвет шрифта на более красивый.
    /// </summary>
    public static void ChangeGoodForegroundColor()
    {
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Cyan;
    }

    /// <summary>
    /// Статический метод, меняющий цвет шрифта и фона на более красивый.
    /// </summary>
    public static void ChangeGoodAllColor()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Cyan;
    }

    /// <summary>
    /// Статический метод, меняющий цвет шрифта и фона на предупреждающий красный.
    /// </summary>
    public static void ChangeBadAllColor()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Red;
    }
}