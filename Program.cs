using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium(15);
            bool isMenuWork = true;
            const char CommandAddRandomFish = '1';
            const char CommandRemoveFish = '2';
            const char CommandRemoveDiedFishes = '3';
            const char CommandExit = '4';

            while (isMenuWork)
            {
                Console.Clear();
                aquarium.ShowInfo();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"{CommandAddRandomFish}) добавить случайную рыбку \n{CommandRemoveFish}) убрать рыбку " +
                    $"\n{CommandRemoveDiedFishes}) убрать всех мёртвых рыб \n{CommandExit}) выход" +
                    "\n\nНажатие любую другую клавишу, чтобы пропустить время");
                ConsoleKeyInfo key = Console.ReadKey(true);
                aquarium.SkipTime();

                switch (key.KeyChar)
                {
                    case CommandAddRandomFish:
                        aquarium.AddRandomFish();
                        break;
                    case CommandRemoveFish:
                        aquarium.RemoveFish();
                        break;
                    case CommandRemoveDiedFishes:
                        aquarium.RemoveDiedFishes();
                        break;
                    case CommandExit:
                        isMenuWork = false;
                        break;
                }
            }
        }
    }

    class Aquarium
    {
        private static Random _random = new Random();
        private List<Fish> _fishes = new List<Fish>();
        private int _fishsCountMax;

        public Aquarium(int fishsCountMax)
        {
            _fishsCountMax = fishsCountMax;

            for (int i = 0; i < _random.Next(1, _fishsCountMax); i++)
            {
                _fishes.Add(CreateFish());
            }
        }

        public void SkipTime()
        {
            foreach (Fish fish in _fishes)
            {
                fish.SkipTime();
            }
        }

        public void ShowInfo()
        {
            Console.SetCursorPosition(0, 10);

            for (int i = 0; i < _fishsCountMax; i++)
            {
                Console.Write("\n" + (i + 1) + ") ");

                if (i < _fishes.Count)
                {
                    _fishes[i].ShowInfo();
                }
            }
        }

        public void AddRandomFish()
        {
            if (IsFulled() == false)
            {
                _fishes.Add(CreateFish());
            }
        }

        public void RemoveFish()
        {
            Console.Clear();

            if (_fishes.Count == 0)
            {
                Console.WriteLine("В аквариуме нет рыб");
            }
            else
            {
                _fishes.RemoveAt(GetIndex());
            }
        }

        public void RemoveDiedFishes()
        {
            List<Fish> deadFishes = new List<Fish>();

            foreach (Fish fish in _fishes)
            {
                if (fish.IsAlive == false)
                {
                    deadFishes.Add(fish);
                }
            }

            foreach (Fish fish in deadFishes)
            {
                _fishes.Remove(fish);
            }
        }

        private bool IsFulled()
        {
            bool isFulled = _fishes.Count >= _fishsCountMax;

            if (isFulled)
            {
                Console.WriteLine("Аквариум максимально заполнен");
                Console.ReadKey(true);
            }

            return isFulled;
        }

        private int GetIndex()
        {
            int index = 0;
            bool success = false;
            bool isCorrect = false;

            while (isCorrect == false)
            {
                Console.WriteLine("Рыбку под каким номером Вы хотите убрать?");
                string userInput = Console.ReadLine();
                success = int.TryParse(userInput, out index);

                if (success && index > 0 && index <= _fishes.Count)
                {
                    isCorrect = true;
                }
                else
                {
                    Console.WriteLine("Такой рыбки нет");
                }
            }

            return index - 1;
        }

        private Fish CreateFish()
        {
            Fish[] fishesTypes = {new Guppy(ConsoleColor.Blue), new Guppy(ConsoleColor.DarkRed),
                new GoldFish(ConsoleColor.Yellow), new GoldFish (ConsoleColor.DarkYellow),
            new Barbus (ConsoleColor.Cyan), new Barbus (ConsoleColor.Green)};

            return fishesTypes[_random.Next(fishesTypes.Length)];
        }
    }

    class Fish
    {
        protected static Random Random = new Random();
        protected ConsoleColor Color;
        protected string ClassName;
        protected int Age;
        protected int MaxAge;
        private int _maxAgeSpread = 50;

        public bool IsAlive { get; private set; }

        public Fish(string className, int medianMaxAge, ConsoleColor color)
        {
            ClassName = className;
            MaxAge = GetSpread(medianMaxAge);
            Age = 0;
            Color = color;
            IsAlive = true;
        }

        public void SkipTime()
        {
            Age++;
            IsDied();
        }

        public void ShowInfo()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;

            if (IsAlive)
            {
                Console.Write($"{ClassName}. Возраст: {Age} / {MaxAge}. Жива");
            }
            else
            {
                Console.Write($"{ClassName}. Умерла");
            }

            Console.ForegroundColor = defaultColor;
        }

        private int GetSpread(int medianMaxAge)
        {
            int hundredPercent = 100;
            medianMaxAge += medianMaxAge * Random.Next(-_maxAgeSpread, _maxAgeSpread + 1) / hundredPercent;
            return medianMaxAge;
        }

        private void IsDied()
        {
            if (IsAlive)
            {
                IsAlive = Age <= MaxAge;
            }
        }
    }

    class Guppy : Fish
    {
        public Guppy(ConsoleColor color) : base("Гуппи", 30, color) { }
    }

    class GoldFish : Fish
    {
        public GoldFish(ConsoleColor color) : base("Золотая рыбка", 50, color) { }
    }

    class Barbus : Fish
    {
        public Barbus(ConsoleColor color) : base("Барбус", 40, color) { }
    }
}
