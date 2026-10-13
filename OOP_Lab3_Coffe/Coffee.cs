using System;

namespace CoffeeApp
{
    public class Coffee
    {
        private string orderCode;
        private string name;
        private CoffeeType type;
        private double price;
        private DateTime roastDate;
        private bool isDecaf;
        private int volumeMl;
        private int temperatureC;

        private static readonly DateTime MinRoastDate = new DateTime(2020, 1, 1);

        public string Currency { get; private set; } = "грн";

        public Coffee()
        {
            TemperatureC = 75;
        }

        public Coffee(string orderCode, string name)
            : this(orderCode, name, CoffeeType.Espresso)
        {
        }

        public Coffee(string orderCode, string name, CoffeeType type)
            : this(orderCode, name, type, 50.0)
        {
        }

        public Coffee(string orderCode, string name, CoffeeType type, double price)
            : this(orderCode, name, type, price, DateTime.Today, false, 200)
        {
        }

        public Coffee(string orderCode, string name, CoffeeType type, double price,
                      DateTime roastDate, bool isDecaf, int volumeMl) : this()
        {
            OrderCode = orderCode;
            Name = name;
            Type = type;
            Price = price;
            RoastDate = roastDate;
            IsDecaf = isDecaf;
            VolumeMl = volumeMl;
        }

        public string OrderCode
        {
            get { return orderCode; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(null, "Error: відсутнє значення!");
                if (value.Length != 6)
                    throw new ArgumentException("Error: код має містити рівно 6 символів!");
                if (!IsLettersOrDigits(value))
                    throw new ArgumentException("Error: дозволені тільки літери та цифри!");
                orderCode = value;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(null, "Error: відсутнє значення!");
                if (value.Length < 2 || value.Length > 30)
                    throw new ArgumentOutOfRangeException(null, "Error: довжина назви має бути від 2 до 30 символів!");
                name = value;
            }
        }

        public CoffeeType Type
        {
            get { return type; }
            set
            {
                if (!Enum.IsDefined(typeof(CoffeeType), value))
                    throw new ArgumentException("Error: невірний тип кави!");
                type = value;
            }
        }

        public double Price
        {
            get { return price; }
            set
            {
                if (value <= 0 || value > 500)
                    throw new ArgumentOutOfRangeException(null, "Error: ціна має бути в діапазоні (0; 500]!");
                price = value;
            }
        }

        public DateTime RoastDate
        {
            get { return roastDate; }
            set
            {
                if (value < MinRoastDate || value > DateTime.Now)
                    throw new ArgumentOutOfRangeException(null,
                        $"Error: дата має бути в діапазоні від 01.01.2020 до {DateTime.Now:dd.MM.yyyy}!");
                roastDate = value;
            }
        }

        public bool IsDecaf
        {
            get { return isDecaf; }
            set { isDecaf = value; }
        }

        public int VolumeMl
        {
            get { return volumeMl; }
            set
            {
                if (value < 30 || value > 1000)
                    throw new ArgumentOutOfRangeException(null, "Error: об'єм має бути в діапазоні [30; 1000]!");
                volumeMl = value;
            }
        }

        public int TemperatureC
        {
            get { return temperatureC; }
            private set
            {
                if (value < 20 || value > 95)
                    throw new ArgumentOutOfRangeException(null, "Error: температура має бути в діапазоні [20; 95]!");
                temperatureC = value;
            }
        }

        public double PricePer100Ml
        {
            get { return Price / VolumeMl * 100; }
        }

        private static bool IsLettersOrDigits(string text)
        {
            foreach (char ch in text)
                if (!char.IsLetterOrDigit(ch)) return false;
            return true;
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public void Reheat()
        {
            Console.WriteLine("  [Виклик Reheat()]");
            Reheat(10);
        }

        public void Reheat(int degrees)
        {
            Console.WriteLine("  [Виклик Reheat(int)]");
            TemperatureC = Clamp(TemperatureC + degrees, 20, 95);
            Console.WriteLine($"  -> {Name} підігріто на {degrees}°C. Поточна температура: {TemperatureC}°C");
        }

        public void Cool(int degrees)
        {
            TemperatureC = Clamp(TemperatureC - degrees, 20, 95);
            Console.WriteLine($"  -> {Name} охолоджено. Поточна температура: {TemperatureC}°C");
        }

        public void ApplyDiscount(double percent)
        {
            Console.WriteLine("  [Виклик ApplyDiscount(double)]");
            double oldPrice = Price;
            Price = Price - Price * percent / 100;
            Console.WriteLine($"  -> Знижка {percent}% застосована. Ціна: {oldPrice:F2} -> {Price:F2} {Currency}");
        }

        public void ApplyDiscount(double percent, double minPrice)
        {
            Console.WriteLine("  [Виклик ApplyDiscount(double, double)]");
            double oldPrice = Price;
            double newPrice = Price - Price * percent / 100;
            double floor = Math.Min(minPrice, oldPrice);
            if (newPrice < floor) newPrice = floor;
            Price = newPrice;
            Console.WriteLine($"  -> Знижка {percent}% (мін. ціна {minPrice:F2}). Ціна: {oldPrice:F2} -> {Price:F2} {Currency}");
        }

        public void AddVolume(int ml)
        {
            Console.WriteLine("  [Виклик AddVolume(int)]");
            VolumeMl = Clamp(VolumeMl + ml, 30, 1000);
            Console.WriteLine($"  -> Додано {ml} мл. Новий об'єм: {VolumeMl} мл");
        }

        public void AddVolume(int ml, bool increasePrice)
        {
            Console.WriteLine("  [Виклик AddVolume(int, bool)]");
            int oldVolume = VolumeMl;
            double oldPrice = Price;
            VolumeMl = Clamp(VolumeMl + ml, 30, 1000);
            if (increasePrice)
                Price = Math.Min(500, Price * VolumeMl / oldVolume);
            Console.WriteLine($"  -> Об'єм: {oldVolume} -> {VolumeMl} мл; ціна: {oldPrice:F2} -> {Price:F2} {Currency}");
        }

        public override string ToString()
        {
            return $"{OrderCode,-8}{Name,-20}{Type,-12}{Price,-10:F2}{RoastDate,-14:dd.MM.yyyy}" +
                   $"{(IsDecaf ? "Так" : "Ні"),-7}{VolumeMl,-8}{TemperatureC}";
        }
    }
}