using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium(15);
            bool isMenuWork = true;

            while (isMenuWork)
            {
                Console.Clear();
                aquarium.ShowInfo();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("1) добавить случайную рыбку \n2) убрать рыбку \n3) убрать всех мёртвых рыб \n4) выход" +
                    "\n\nНажатие любую другую клавишу, чтобы пропустить время");
                ConsoleKeyInfo key = Console.ReadKey(true);
                Console.Clear();

                switch (key.KeyChar)
                {
                    case '1':
                        aquarium.AddRandomFish();
                        break;
                    case '2':
                        aquarium.RemoveFish();
                        break;
                    case '3':
                        aquarium.RemoveDiedFishs();
                        break;
                    case '4':
                        isMenuWork = false;
                        break;
                    default:
                        aquarium.SkipTime();
                        break;
                }
            }
        }
    }   

    class Aquarium
    {
        private static Random _random = new Random();
        private List<Fish> _fishs = new List<Fish>();
        private int _fishsCountMax;

        public Aquarium(int fishsCountMax)
        {
            _fishsCountMax = fishsCountMax;

            for (int i = 0; i < _random.Next(1, _fishsCountMax); i++)
            {
                _fishs.Add(CreateFish());
            }
        }

        public void SkipTime()
        {
            foreach (Fish fish in _fishs)
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

                if (i < _fishs.Count)
                {
                    _fishs[i].ShowInfo();
                }
            }
        }

        public void AddRandomFish()
        {
            SkipTime();

            if (IsFulled() == false)
            {
                _fishs.Add(CreateFish());
            }
        }

        public void RemoveFish()
        {
            _fishs.RemoveAt(GetIndex());
            SkipTime();
        }

        public void RemoveDiedFishs()
        {
            List<Fish> deadFish = new List<Fish>();

            foreach (Fish fish in _fishs)
            {
                if (fish.IsAlive == false)
                {
                    deadFish.Add(fish);
                }
            }

            foreach (Fish fish in deadFish)
            {
                _fishs.Remove(fish);
            }

            SkipTime();
        }

        private bool IsFulled()
        {
            bool isFulled = _fishs.Count >= _fishsCountMax;

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

                if (success && index > 0 && index <= _fishs.Count)
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
            int typesFish = 3;
            int typeFish1 = 0;
            int typeFish2 = 1;
            int coose = _random.Next(typesFish);

            if (coose == typeFish1)
            {
                return new Guppy();
            }
            else if (coose == typeFish2)
            {
                return new GoldFish();
            }
            else
            {
                return new Barbus();
            }
        }
    }

    class Fish
    {
        protected static Random Random = new Random();
        protected ConsoleColor Color;
        protected ConsoleColor PossibleColor1;
        protected ConsoleColor PossibleColor2;
        protected string ClassName;
        protected int Age;
        protected int MaxAge; 
        private int _maxAgeSpread = 50;

        public bool IsAlive { get; private set; }

        public Fish(string className, int medianMaxAge, ConsoleColor possibleColor1, ConsoleColor possibleColor2)
        {
            ClassName = className;
            MaxAge = GetSpread(medianMaxAge);
            Age = 0;
            PossibleColor1 = possibleColor1;
            PossibleColor2 = possibleColor2;
            Color = SetColor();
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

        private ConsoleColor SetColor()
        {
            int colorsNumber = 2;

            if (Random.Next(colorsNumber) == 0)
            {
                return PossibleColor1;
            }
            else
            {
                return PossibleColor2;
            }
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
        public Guppy() : base("Гуппи", 30, ConsoleColor.Blue, ConsoleColor.DarkRed) { }
    }

    class GoldFish : Fish
    {
        public GoldFish() : base("Золотая рыбка", 50, ConsoleColor.DarkYellow, ConsoleColor.Yellow) { }
    }

    class Barbus : Fish
    {
        public Barbus() : base("Барбус", 40, ConsoleColor.DarkGreen, ConsoleColor.Cyan) { }
    }
}
