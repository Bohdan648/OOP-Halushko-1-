using System;

namespace Lab1
{
    public class City
    {
        private string name;
        private string country;
        private long population;

        public long Population
        {
            get { return population; }
            set { population = value < 0 ? 0 : value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public City(string name, string country, long population)
        {
            this.name = name;
            this.country = country;
            Population = population;
        }

        public string GetInfo()
        {
            return $"Місто: {Name}, Країна: {Country}, Населення: {Population:N0} осіб";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Лабораторна робота №1 (Варіант 5) ===");

            City city1 = new City("Київ", "Україна", 2950000);
            City city2 = new City("Париж", "Франція", 2161000);
            City city3 = new City("Токіо", "Японія", 13960000);

            Console.WriteLine(city1.GetInfo());
            Console.WriteLine(city2.GetInfo());
            Console.WriteLine(city3.GetInfo());
        }
    }
}