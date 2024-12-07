using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace caz;

class Program
{
    static void Main(string[] args)
    {
        TestE();
    }

    private static void TestE()
    {
        var lines = File.ReadAllLines("input.txt");
        var parser = new TestBlue3();
        var result = lines.Select(x => parser.Parse(x))
            .ToArray();
        const int maxR = 12;
        const int maxG = 13;
        const int maxB = 14;
        int sum = 0;
        int power = 0;

        foreach (var game in result)
        {
            Debug.Assert(game.Ok);
            Debug.Assert(game.Rest == string.Empty);
            //var run = new GameRun()
            //{
            //    Data = new List<TestBlue> {
            //        new TestBlue { Color = "red" },
            //        new TestBlue { Color = "blue" },
            //        new TestBlue { Color = "green" },
            //    }
            //};

            var g = game.Value;
            var red = g.Runs.SelectMany(x => x.Data
                            .Where(t => t.Color == "red")
                            .Select(t => t.Number))
                            .Max();
            var green = g.Runs.SelectMany(x => x.Data
                            .Where(t => t.Color == "green")
                            .Select(t => t.Number))
                            .Max();
            var blue = g.Runs.SelectMany(x => x.Data
                            .Where(t => t.Color == "blue")
                            .Select(t => t.Number))
                            .Max();
            if (red <= maxR && green <= maxG && blue <= maxB) sum += g.Index;
            var local = red * green * blue;
            power += local;
            //game.Value.Runs.Select(x => )
        }
        Console.WriteLine(sum);
        Console.WriteLine(power);
    }

    private static void TestD()
    {
        var a = new TestBlue3();
        var b = a.Parse("Game 15: 15 red, 3 blue, 5 red, 10 green; 6 red, 3 blue; 19 green00xj");
    }

    private static void TestC()
    {
        var a = new TestBlue2();
        var b = a.Parse("15 red, 3 blue, 5 red, 10 green00xj");
    }

    private static void TestB()
    {
        var comma = new StartsWith(", ");
        var dp = new TestBlueParser();
        var xp = new Sequence<TestBlue>(new Map<string, TestBlue>(comma, _ => null), dp);
        var mp = new ZeroMore<TestBlue>(xp);
        var rr = mp.Parse(", 3 blue, 5 red, 10 green,--90");
    }

    private static void TestA()
    {
        var seq = Enumerable.Range(1, 5);
        var result = seq.Aggregate(new StringBuilder(), (sb, x) => sb.Append(x));
        var final = result.ToString();
        var A = new TestBlueParser();
        var B = A.Parse("2 redblueuuu");
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

//record Occur(int Number, string Color) { }

//class Vysledek
//{
//    public int Number { get; set; }
//    public string Color { get; set; }
//}

[DebuggerDisplay("{Number} {Color}")]
class TestBlue
{
    public int Number { get; set; }
    public string Color { get; set; }
}

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

class Game
{
    public List<GameRun> Runs { get; set; }
    public int Index { get; set; }
}

class GameRun
{
    public List<TestBlue> Data { get; set; }
}

class TestBlue3 : Parser<Game>
{
    public override Result<Game> Parse(string input)
    {
        var obj = new Game { Runs = new List<GameRun>() };

        var bp = new StartsWith("Game ");
        var n = new Number();
        var gdp = new StartsWith(": ");
        var bsq = new Sequence<Game>(new Map<string, Game>(bp, _ => null),
            new Map<int, Game>(n, x =>
            {
                obj.Index = x;
                return obj;
            }), new Map<string, Game>(gdp, _ => null));



        var op = new TestBlue2();
        var op1 = new Map<GameRun, Game>(op, x =>
        {
            obj.Runs.Add(x);
            return obj;
        });

        var comma = new StartsWith("; ");
        var dp = new TestBlue2();
        var xp = new Sequence<GameRun>(new Map<string, GameRun>(comma, _ => null), dp);
        var mp = new ZeroMore<GameRun>(xp);
        var mpf = new Map<GameRun[], Game>(mp, x =>
        {
            obj.Runs.AddRange(x);
            return obj;
        });

        var sq = new Sequence<Game>(bsq, op1, mpf);
        var result = sq.Parse(input);
        if (result.Ok)
        {
            return Result<Game>.ToSuccess(obj, result.Rest);
        }
        else
        {
            return Result<Game>.ToFailure($"Failed to parse {input}");
        }
    }
}

class TestBlue2 : Parser<GameRun>
{
    public override Result<GameRun> Parse(string input)
    {
        var obj = new GameRun { Data = new List<TestBlue>() };
        var op = new TestBlueParser();
        var op1 = new Map<TestBlue, GameRun>(op, x =>
        {
            obj.Data.Add(x);
            return obj;
        });
        var comma = new StartsWith(", ");
        var dp = new TestBlueParser();
        var xp = new Sequence<TestBlue>(new Map<string, TestBlue>(comma, _ => null), dp);
        var mp = new ZeroMore<TestBlue>(xp);
        var mpf = new Map<TestBlue[], GameRun>(mp, x =>
        {
            obj.Data.AddRange(x);
            return obj;
        });

        var sq = new Sequence<GameRun>(op1, mpf);
        var result = sq.Parse(input);
        if (result.Ok)
        {
            return Result<GameRun>.ToSuccess(obj, result.Rest);
        }
        else
        {
            return Result<GameRun>.ToFailure($"Failed to parse {input}");
        }
    }
}

class TestBlueParser : Parser<TestBlue>
{
    public override Result<TestBlue> Parse(string input)
    {
        var obj = new TestBlue();

        var np = new Number();
        var cp = new ColorParser();
        var np1 = new Map<int, TestBlue>(np, (n) =>
        {
            obj.Number = n;
            return obj;
        });

        var sp = new Map<string, TestBlue>(new StartsWith(" "), _ => obj);

        var cp1 = new Map<string, TestBlue>(cp, c =>
        {
            obj.Color = c;
            return obj;
        });

        var agg = new Sequence<TestBlue>(np1, sp, cp1);
        var result = agg.Parse(input);
        if (result.Ok)
            return Result<TestBlue>.ToSuccess(obj, result.Rest);
        return Result<TestBlue>.ToFailure($"Failed to parse {input}");
    }
}

class ThreeBlue : Parser<TestBlue>
{
    public override Result<TestBlue> Parse(string input)
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
                return Result<TestBlue>.ToSuccess(new TestBlue { Number = C.Value, Color = D.Value }, D.Rest);
            }
            return Result<TestBlue>.ToFailure($"{nameof(ThreeBlue)}: Failed to parse {input}");
        }

        return Result<TestBlue>.ToFailure($"{nameof(ThreeBlue)}: Failed to parse {input}");
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

class ZeroMore<T> : Parser<T[]>
{
    private Parser<T> A { get; }

    public override Result<T[]> Parse(string input)
    {
        List<T> list = new List<T>();
        string rest = input;

        while (true)
        {
            var B = A.Parse(rest);
            if (!B.Ok) break;
            list.Add(B.Value);
            rest = B.Rest;
        }
        return Result<T[]>.ToSuccess(list.ToArray(), rest);
    }

    public ZeroMore(Parser<T> A)
    {
        this.A = A;
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

class Sequence<T> : Parser<T>
{
    private Parser<T>[] Args { get; }

    public override Result<T> Parse(string input)
    {
        Result<T> result = null;

        foreach (var item in Args)
        {
            result = item.Parse(input);

            if (!result.Ok)
            {
                return Result<T>.ToFailure($"Sequence: Failed to parse input {input}");
            }

            input = result.Rest;
        }

        return Result<T>.ToSuccess(result.Value, result.Rest);
    }

    public Sequence(params Parser<T>[] args)
    {
        Args = args;
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