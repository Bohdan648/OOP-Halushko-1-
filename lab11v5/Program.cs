
using System;
using System.Collections.Generic;
using System.Linq;

class Landmark
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

class City
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Population { get; set; }
    public List<Landmark> Landmarks { get; set; } = new();
}

class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<City> Cities { get; set; } = new();
}

static class Extensions
{
    public static int TotalPopulation(this IEnumerable<City> cities)
    {
        return cities.Sum(c => c.Population);
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        List<Country> countries = new()
        {
            new Country
            {
                Id = 1,
                Name = "Україна",
                Cities = new()
                {
                    new City
                    {
                        Id = 1, Name = "Київ", Population = 2900000,
                        Landmarks = new()
                        {
                            new Landmark { Id = 1, Name = "Софійський собор" },
                            new Landmark { Id = 2, Name = "Золоті ворота" }
                        }
                    },
                    new City
                    {
                        Id = 2, Name = "Львів", Population = 720000,
                        Landmarks = new()
                        {
                            new Landmark { Id = 3, Name = "Оперний театр" },
                            new Landmark { Id = 4, Name = "Високий замок" }
                        }
                    },
                    new City
                    {
                        Id = 3, Name = "Рівне", Population = 240000,
                        Landmarks = new()
                        {
                            new Landmark { Id = 5, Name = "Парк Шевченка" },
                            new Landmark { Id = 6, Name = "Краєзнавчий музей" }
                        }
                    }
                }
            },
            new Country
            {
                Id = 2,
                Name = "Польща",
                Cities = new()
                {
                    new City
                    {
                        Id = 4, Name = "Варшава", Population = 1800000,
                        Landmarks = new()
                        {
                            new Landmark { Id = 7, Name = "Королівський замок" },
                            new Landmark { Id = 8, Name = "Палац культури" }
                        }
                    },
                    new City
                    {
                        Id = 5, Name = "Краків", Population = 800000,
                        Landmarks = new()
                        {
                            new Landmark { Id = 9, Name = "Вавельський замок" },
                            new Landmark { Id = 10, Name = "Ринкова площа" }
                        }
                    }
                }
            }
        };

       
        Console.WriteLine("Усі пам'ятки:");

        var landmarks = countries
            .SelectMany(c => c.Cities)
            .SelectMany(c => c.Landmarks);

        foreach (var landmark in landmarks)
        {
            Console.WriteLine(landmark.Name);
        }

        
        var cities = countries.SelectMany(c => c.Cities).ToList();

        Console.WriteLine("\nМіста по 4 на сторінку:");

        int pageSize = 4;
        int pages = (cities.Count + pageSize - 1) / pageSize;

        for (int page = 1; page <= pages; page++)
        {
            Console.WriteLine("\nСторінка " + page);

            var result = cities
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            foreach (var city in result)
            {
                Console.WriteLine(city.Name);
            }
        }

       
        Console.WriteLine("\nЗагальне населення:");
        Console.WriteLine(cities.TotalPopulation());
    }
}
