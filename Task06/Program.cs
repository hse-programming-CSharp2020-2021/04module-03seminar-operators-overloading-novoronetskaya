using System;

/*
Источник: https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/operators/operator-overloading

Fraction - упрощенная структура, представляющая рациональное число.
Необходимо перегрузить операции:
+ (бинарный)
- (бинарный)
*
/ (в случае деления на 0, выбрасывать DivideByZeroException)

Тестирование приложения выполняется путем запуска разных наборов тестов, например,
на вход поступает две строки, содержацие числители и знаменатели двух дробей, разделенные /, соответственно.
1/3
1/6
Программа должна вывести на экран сумму, разность, произведение и частное двух дробей, соответственно,
с использованием перегруженных операторов (при необходимости, сокращать дроби):
1/2
1/6
1/18
2

Обратите внимание, если дробь имеет знаменатель 1, то он уничтожается (2/1 => 2). Если дробь в числителе имеет 0, то 
знаменатель также уничтожается (0/3 => 0).
Никаких дополнительных символов выводиться не должно.

Код метода Main можно подвергнуть изменениям, но вывод меняться не должен.
*/

public readonly struct Fraction
{
    readonly int numerator;
    readonly int denomenator;

    public static Fraction Parse(string input)
    {
        int num = 0;
        int den = 0;
        if (input.Split('/').Length > 1)
        {
            int.TryParse(input.Split('/')[0], out num);
            int.TryParse(input.Split('/')[1], out den);
        }
        else
        {
            int.TryParse(input, out num);
            den = 1;
        }
        return new Fraction(num, den);
    }
    public Fraction(int num, int den)
    {
        numerator = num;
        denomenator = den;
    }
    public override string ToString()
    {
        if (denomenator == 1)
            return numerator.ToString();
        return numerator + "/" + denomenator;
    }
    public static Fraction operator +(Fraction a, Fraction b)
    {
        Fraction result = new Fraction(a.numerator * b.denomenator + a.denomenator * b.numerator, a.denomenator * b.denomenator);
        return result.Simplify();
    }
    public static Fraction operator *(Fraction a, Fraction b)
    {
        return new Fraction(a.numerator * b.numerator, a.denomenator * b.denomenator).Simplify();
    }
    public static Fraction operator -(Fraction a, Fraction b)
    {
        Fraction result = new Fraction(a.numerator * b.denomenator - a.denomenator * b.numerator, a.denomenator * b.denomenator);
        return result.Simplify();
    }
    public static Fraction operator /(Fraction a, Fraction b)
    {
        if (b.numerator < 0)
        {
            return new Fraction((-1) * a.numerator * b.denomenator, (-1) * a.denomenator * b.numerator).Simplify();
        }
        if (b.numerator == 0)
        {
            throw new DivideByZeroException();
        }
        return new Fraction(a.numerator * b.denomenator, a.denomenator * b.numerator).Simplify();
    }
    private Fraction Simplify()
    {
        int num = Math.Abs(numerator);
        int den = Math.Abs(denomenator);
        while (num > 0 && den > 0)
        {
            if (num > den)
            {
                int t = num;
                num = den;
                den = t % den;
            }
            else
            {
                int t = den;
                den = num;
                num = t % num;
            }
        }
        int nod = Math.Max(num, den);
        if (numerator < 0 && denomenator < 0)
        {
            nod *= -1;
        }
        return new Fraction(numerator / nod, denomenator / nod);
    }
}

public static class OperatorOverloading
{
    public static void Main()
    {
        try
        {
            Fraction fract1 = Fraction.Parse(Console.ReadLine());
            Fraction fract2 = Fraction.Parse(Console.ReadLine());
            Console.WriteLine(fract1 + fract2);
            Console.WriteLine(fract1 - fract2);
            Console.WriteLine(fract1 * fract2);
            Console.WriteLine(fract1 / fract2);
        }
        catch (ArgumentException)
        {
            Console.WriteLine("error");
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("zero");
        }
    }
}
