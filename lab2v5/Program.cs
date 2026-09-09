using System;

namespace Lab2
{
    public class City
    {
        private string _name = "Unknown";
        private string _country = "Unknown";
        private long _population;

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
        }

        public string Country
        {
            get => _country;
            set => _country = string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
        }

        public long Population
        {
            get => _population;
            set
            {
                if (value < 0)
                {
                    Console.WriteLine($"[Увага] Населення для міста '{Name}' не може бути від'ємним ({value}). Встановлено 0.");
                    _population = 0;
                }
                else
                {
                    _population = value;
                }
            }
        }

        public City(string name, string country, long population)
        {
            Name = name;
            Country = country;
            Population = population;
        }

        public City() : this("Unknown", "Unknown", 0)
        {
        }

        public string GetCityInfo()
        {
            return $"Місто: {Name}, Країна: {Country}, Населення: {Population:N0} осіб";
        }

        ~City()
        {
            Console.WriteLine($"[GC] Об'єкт City \"{Name}\" знищено з пам'яті.");
        }
    }

    internal class Program
    {
        static void CreateObjects()
        {
            Console.WriteLine("=== Creating objects ===");

            City city1 = new City();
            Console.WriteLine(city1.GetCityInfo());

            City city2 = new City("Рівне", "Україна", 245000);
            Console.WriteLine(city2.GetCityInfo());

            City city3 = new City("Атлантида", "Невідомо", -500);
            Console.WriteLine(city3.GetCityInfo());

            Console.WriteLine("=== Objects created ===");
        }

        static void Main(string[] args)
        {
            CreateObjects();

            Console.WriteLine("=== End of Main, preparing for GC ===");

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}