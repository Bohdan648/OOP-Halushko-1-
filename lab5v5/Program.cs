using System;
using System.Collections.Generic;

namespace lab5v5
{
    public class Animal
    {
        public string Name { get; set; }

        public Animal(string name) => Name = name;

        public virtual void MakeSound() => Console.WriteLine($"{Name} робить невизначений звук.");
    }

    public class Lion : Animal
    {
        public double ManeSize { get; set; }

        public Lion(string name, double maneSize) : base(name) => ManeSize = maneSize;

        public override void MakeSound() => Console.WriteLine($"{Name} (Лев, грива: {ManeSize} см) видає звук: Roar!");
    }

    public class Elephant : Animal
    {
        public double TrunkLength { get; set; }

        public Elephant(string name, double trunkLength) : base(name) => TrunkLength = trunkLength;

        public override void MakeSound() => Console.WriteLine($"{Name} (Слон, хобот: {TrunkLength} м) видає звук: Trumpet!");
    }

    public class Bird : Animal
    {
        public double WingSpan { get; set; }

        public Bird(string name, double wingSpan) : base(name) => WingSpan = wingSpan;

        public override void MakeSound() => Console.WriteLine($"{Name} (Птах, розмах крил: {WingSpan} см) видає звук: Chirp!");
    }

    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Animal> animals = new List<Animal>
            {
                new Lion("Сімба", 25.5),
                new Elephant("Джамбо", 1.8),
                new Bird("Чіжик", 30.0)
            };

            Console.WriteLine("=== Звуки тварин (Поліморфізм) ===");
            foreach (var animal in animals)
            {
                animal.MakeSound();
            }
        }
    }
}