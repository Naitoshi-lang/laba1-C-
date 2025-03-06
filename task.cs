using System;

// 6*. Собственный класс исключения
public class NotCorrectlyDenominatorException : ArgumentException
{
    public NotCorrectlyDenominatorException(string message) : base(message) { }
}

public class Fraction
{
    // 1. Переменные (поля) класса
    private int numerator;     // Числитель
    private int denominator;   // Знаменатель

    // 2. Свойства для доступа к числителю и знаменателю
    public int Numerator
    {
        get { return numerator; }
        set { numerator = value; Simplify(); } // При изменении числителя - упрощаем дробь
    }

    public int Denominator
    {
        get { return denominator; }
        set
        {
            // 4. Проверка знаменателя на 0.
            if (value == 0)
            {
                throw new NotCorrectlyDenominatorException("Знаменатель не может быть равен 0"); // 6*.  Выбрасываем собственное исключение
            }
            denominator = value;
            Simplify(); // При изменении знаменателя - упрощаем дробь
        }
    }

    // 3. Свойство для получения десятичной дроби (только для чтения)
    public double DecimalValue
    {
        get { return (double)numerator / denominator; }
    }


    // Конструктор класса
    public Fraction(int numerator, int denominator)
    {
        this.numerator = numerator;
        this.Denominator = denominator; // Используем свойство Denominator, чтобы сработала проверка на 0 и упрощение
    }

    // 5. Упрощение дробей (находим НОД и делим на него)
    private void Simplify()
    {
        int gcd = GreatestCommonDivisor(Math.Abs(numerator), Math.Abs(denominator));
        numerator /= gcd;
        denominator /= gcd;

        // Приводим знаменатель к положительному значению
        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }
    }

    // Метод для нахождения наибольшего общего делителя (НОД)
    private static int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }


    // 1. Методы сложения, вычитания, умножения и деления дробей

    public Fraction Add(Fraction other)
    {
        int newNumerator = this.numerator * other.denominator + other.numerator * this.denominator;
        int newDenominator = this.denominator * other.denominator;
        return new Fraction(newNumerator, newDenominator);
    }

    public Fraction Subtract(Fraction other)
    {
        int newNumerator = this.numerator * other.denominator - other.numerator * this.denominator;
        int newDenominator = this.denominator * other.denominator;
        return new Fraction(newNumerator, newDenominator);
    }

    public Fraction Multiply(Fraction other)
    {
        int newNumerator = this.numerator * other.numerator;
        int newDenominator = this.denominator * other.denominator;
        return new Fraction(newNumerator, newDenominator);
    }

    public Fraction Divide(Fraction other)
    {
        // Проверка деления на ноль (для дробей - это когда числитель второй дроби равен 0)
        if (other.numerator == 0)
        {
            throw new DivideByZeroException("Деление на ноль невозможно.");
        }

        int newNumerator = this.numerator * other.denominator;
        int newDenominator = this.denominator * other.numerator;
        return new Fraction(newNumerator, newDenominator);
    }

    // Метод для отображения дроби в формате "числитель/знаменатель"
    public override string ToString()
    {
        return $"{numerator}/{denominator}";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Создаем объекты класса Fraction
            Fraction fraction1 = new Fraction(1, 2);
            Fraction fraction2 = new Fraction(3, 4);

            // Демонстрируем работу методов
            Console.WriteLine($"Дробь 1: {fraction1}, десятичное значение: {fraction1.DecimalValue}"); // Вывод: 1/2, десятичное значение: 0.5
            Console.WriteLine($"Дробь 2: {fraction2}, десятичное значение: {fraction2.DecimalValue}"); // Вывод: 3/4, десятичное значение: 0.75

            Fraction sum = fraction1.Add(fraction2);
            Console.WriteLine($"Сумма: {sum}, десятичное значение: {sum.DecimalValue}"); // Вывод: 10/8, десятичное значение: 1.25 (упрощено до 5/4)

            Fraction difference = fraction1.Subtract(fraction2);
            Console.WriteLine($"Разность: {difference}, десятичное значение: {difference.DecimalValue}"); // Вывод: -2/8, десятичное значение: -0.25 (упрощено до -1/4)

            Fraction product = fraction1.Multiply(fraction2);
            Console.WriteLine($"Произведение: {product}, десятичное значение: {product.DecimalValue}"); // Вывод: 3/8, десятичное значение: 0.375

            Fraction quotient = fraction1.Divide(fraction2);
            Console.WriteLine($"Частное: {quotient}, десятичное значение: {quotient.DecimalValue}"); // Вывод: 4/6, десятичное значение: 0.6666666666666666 (упрощено до 2/3)

            // Изменение числителя и знаменателя с использованием свойств
            fraction1.Numerator = 5;
            fraction1.Denominator = 6; // Произойдет упрощение.
            Console.WriteLine($"Измененная дробь 1: {fraction1}, десятичное значение: {fraction1.DecimalValue}"); // Вывод: 5/6, десятичное значение: 0.8333333333333334

            //  Попытка создать дробь с нулевым знаменателем (вызовет исключение)
            // Fraction invalidFraction = new Fraction(5, 0);

        }
        catch (NotCorrectlyDenominatorException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}"); // Обработка исключения, когда знаменатель равен 0.
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");  // Обработка исключения при делении на 0
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}"); // Обработка других исключений.
        }
    }
}
