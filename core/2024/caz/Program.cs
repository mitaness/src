namespace caz;

class Program
{
    static void Main(string[] args)
    {
        Test8();
    }

    private static void Test8()
    {
        var A = new ThreeBlue();
        var B = A.Parse("59      red blue0000738sdjfl");
    }

    private static void Test7()
    {
        var A = new Number();
        var B = A.Parse("0000738sdjfl");
    }

    private static void Test6()
    {
        var A = new Digit();
        var B = new Map<int, string>(A, x => x.ToString());
        var result = B.Parse("-3kd");
    }

    private static void Test5()
    {
        var A = new Digit();
        var AA = new OneMore<int>(A);
        var result = AA.Parse("1x23kd");
    }

    private static void Test4()
    {
        var A = new Digit();
        var B = A.Parse("6xyz");
    }

    private static void Test3()
    {
        var input = "z7jd";
        var range = Enumerable.Range(0, 10)
            .Select(x => x.ToString())
            //.Select(x => input.StartsWith(x))
            .SkipWhile(x => !input.StartsWith(x))
            .Take(1)
            .ToArray();
        if (range.Length == 0)
        {

        }
    }

    private static void Test2()
    {
        var R = new StartsWith("red");
        var G = new StartsWith("green");
        var B = new StartsWith("blue");
        var A = new OneOf<string>(R, G, B);
        var result = A.Parse("u22dk");
    }

    private static void Test()
    {
        var A = new StartsWith("AAA");
        var B = A.Parse("AAA123 d");
    }
}

// 3 blue,

record Occur(int Number, string Color) { }

//class Vysledek
//{
//    public int Number { get; set; }
//    public string Color { get; set; }
//}

class ColorParser : Parser<string>
{
    public override Result<string> Parse(string input)
    {
        var A = new StartsWith("red");
        var B = new StartsWith("green");
        var C = new StartsWith("blue");
        var D = new OneOf<string>(A, B, C);
        return D.Parse(input);
    }
}

class GameSet : Parser<int>
{
    public override Result<int> Parse(string input)
    {
        var A = new ThreeBlue();
        var result = A.Parse(input);
        if (result.Ok)
        {
        }
        throw new NotImplementedException();
    }
}

class ThreeBlue : Parser<Occur>
{
    public override Result<Occur> Parse(string input)
    {
        var A = new Number();
        var B = new ColorParser();
        var C = A.Parse(input);
        if (C.Ok)
        {
            input = C.Rest.TrimStart();
            var D = B.Parse(input);
            if (D.Ok)
            {
                return Result<Occur>.ToSuccess(new Occur(C.Value, D.Value), D.Rest);
            }
            return Result<Occur>.ToFailure($"{nameof(ThreeBlue)}: Failed to parse {input}");
        }

        return Result<Occur>.ToFailure($"{nameof(ThreeBlue)}: Failed to parse {input}");
    }
}

//??? is it good?
class Optional : Parser<string>
{
    private string X { get; }

    public override Result<string> Parse(string input)
    {
        if (input.StartsWith(X))
        {
            return Result<string>.ToSuccess(X, input.Substring(X.Length));
        }

        return Result<string>.ToSuccess(X, input);
    }

    public Optional(string x)
    {
        X = x;
    }
}

class Number : Parser<int>
{
    public override Result<int> Parse(string input)
    {
        var A = new Digit();
        var AA = new OneMore<int>(A);
        var B = AA.Parse(input);
        if (B.Ok)
        {
            var digits = B.Value;
            var xyz = string.Join(string.Empty, digits);
            var num = int.Parse(xyz);
            return Result<int>.ToSuccess(num, B.Rest);
        }
        return Result<int>.ToFailure($"No number found {input}");
    }
}

class Map<T, U> : Parser<U>
{
    private Parser<T> A { get; }
    private Func<T, U> Transform { get; }

    public override Result<U> Parse(string input)
    {
        var B = A.Parse(input);
        if (B.Ok)
        {
            return Result<U>.ToSuccess(Transform(B.Value), B.Rest);
        }
        return Result<U>.ToFailure($"Map: Failed to parse {input}");
    }

    public Map(Parser<T> A, Func<T, U> transform)
    {
        this.A = A;
        Transform = transform;
    }
}

class OneMore<T> : Parser<T[]>
{
    private Parser<T> A { get; }

    public override Result<T[]> Parse(string input)
    {
        var B = A.Parse(input);
        if (!B.Ok)
        {
            return Result<T[]>.ToFailure($"Failed to parse {input}");
        }

        List<T> list = new List<T>();
        string rest = input;

        while (B.Ok)
        {
            list.Add(B.Value);
            rest = B.Rest;
            B = A.Parse(rest);
        }
        return Result<T[]>.ToSuccess(list.ToArray(), rest);
    }

    public OneMore(Parser<T> A)
    {
        this.A = A;
    }
}

class Digit : Parser<int>
{
    public override Result<int> Parse(string input)
    {
        var result = Enumerable.Range(0, 10)
            .Select(x => x.ToString())
            .SkipWhile(x => !input.StartsWith(x))
            .Take(1)
            .ToArray();
        if (result.Length == 0)
        {
            return Result<int>.ToFailure($"No digit found {input}");
        }

        var digit = int.Parse(result.Single());
        return Result<int>.ToSuccess(digit, input.Substring(1));

    }
}

class OneOf<T> : Parser<T>
{
    private Parser<T>[] Parsers { get; }

    public override Result<T> Parse(string input)
    {
        foreach (var item in Parsers)
        {
            var result = item.Parse(input);
            if (result.Ok)
            {
                return result;
            }
        }

        return Result<T>.ToFailure($"OneOf: Failed to parser input {input}");
    }

    public OneOf(params Parser<T>[] parsers)
    {
        Parsers = parsers;
    }
}

class OrElse<T> : Parser<T>
{
    private Parser<T> A { get; }
    private Parser<T> B { get; }

    public override Result<T> Parse(string input)
    {
        var RA = A.Parse(input);
        if (RA.Ok)
        {
            return RA;
        }
        return B.Parse(input);
    }

    public OrElse(Parser<T> A, Parser<T> B)
    {
        this.A = A;
        this.B = B;
    }
}

class StartsWith : Parser<string>
{
    private string Start { get; }

    public override Result<string> Parse(string input)
    {
        if (input.StartsWith(Start))
        {
            return Result<string>.ToSuccess(Start, input.Substring(Start.Length));
        }
        return Result<string>.ToFailure("Fail parsing");
    }

    public StartsWith(string start)
    {
        Start = start;
    }
}

abstract class Parser<T>
{
    public abstract Result<T> Parse(string input);
}

abstract class Result<T>
{
    public T Value { get; private set; }
    public bool Ok { get; private set; }
    public string ErrorMessage { get; private set; }
    public string Rest { get; private set; }

    public static Success<T> ToSuccess(T value, string rest)
    {
        return new Success<T> { Value = value, Rest = rest, Ok = true };
    }

    public static Failure<T> ToFailure(string error)
    {
        return new Failure<T> { ErrorMessage = error };
    }
}

class Success<T> : Result<T>
{

}

class Failure<T> : Result<T>
{

}