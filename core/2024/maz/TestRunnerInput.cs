using System.Text;

internal class TestRunnerInput
{
    private Dictionary<string, string> Map { get; }

    private void Test2()
    {
        var aa = long.MaxValue;
        long exp = 3541L + 51L * (aa - 98L + 52L);
        checked
        {
            long expr = 3541L + 51L * (36854775807L - 98L + 52L);
            long exp2 = 3541L + 51L * (50000000000L - 98L + 52L);
            Console.WriteLine(expr);
        }

    }

    private void Test()
    {
        var (result, index) = ("##...#......##......#.####.##.#..#..####.#.######.##..#.####...##....#.#.####.####.#..#.######.##...", 0);
        int counter = 0;
        const string cycleStart = "#.#..#.#..#.#..#.#..#.#..#.#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#.#..#..#..#..#..#..#..#.#..#..#";

        for (int i = 0; i < 520; i++)
        {
            (result, index) = Next(result, index);
            counter += index;
        }
        var compare = result == cycleStart;
        var cnt = cycleStart.Where(x => x == '#').Count(); // 51
        var len = cycleStart.Length;

        var sum = cycleStart
              .Select((x, i) => (x, i))
              .Where(B => B.x == '#')
              .Select(B => B.Item2)
              .Sum(); // 3541

        var abc = result
            .Select((x, i) => (x, i + counter))
            .Where(B => B.x == '#')
            .Select(B => B.Item2)
            .Sum();
    }

    public void Start()
    {
        Test2();
        var (result, index) = Next("#.#..#.#..#.#..#.#..#.#..#.#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#.#..#..#..#..#..#..#..#.#..#..#", 0);
        (result, index) = Next("#.#..#.#..#.#..#.#..#.#..#.#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#.#..#..#..#..#..#..#..#.#..#..#", 0);
        (result, index) = Next("#.#..#.#..#.#..#.#..#.#..#.#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#.#..#..#..#..#..#..#..#.#..#..#", 0);
        (result, index) = Next("#.#..#.#..#.#..#.#..#.#..#.#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#..#.#..#..#..#..#..#..#..#.#..#..#", 0);
        Find("##...#......##......#.####.##.#..#..####.#.######.##..#.####...##....#.#.####.####.#..#.######.##...");
    }

    private void Find(string input)
    {
        var slow = input;
        var fast = input;
        int index = 0;
        int count = 0;
        while (true)
        {
            // move fast 2 nodes at a time
            (fast, index) = Next(fast);
            (fast, index) = Next(fast);
            (slow, index) = Next(slow);
            if (slow == fast)
            {
                // cycle detected
                break;
            }
        }

        slow = input; // reset to head
        var start = "";
        while (true)
        {
            (fast, index) = Next(fast);
            (slow, index) = Next(slow);
            count++;
            if (slow == fast)
            {
                start = slow;
                // cycle start
                break;
            }
        }

        int cycleLen = 0;
        while (true)
        {
            (slow, index) = Next(slow, index);
            (slow, index) = Next(slow, index);
            cycleLen++;
            if (slow == start)
            {
                break;
            }
        }
    }

    public TestRunnerInput()
    {
        Map = new Dictionary<string, string>()
        {
            ["#...."] = ".",
            ["#..##"] = "#",
            ["....#"] = ".",
            ["...#."] = ".",
            ["...##"] = "#",
            ["#.#.#"] = ".",
            [".#..."] = "#",
            ["##.#."] = ".",
            ["..#.#"] = ".",
            [".##.#"] = "#",
            ["###.#"] = "#",
            [".#.##"] = ".",
            ["....."] = ".",
            ["#####"] = "#",
            ["###.."] = ".",
            ["##..#"] = "#",
            ["#.###"] = "#",
            ["#.#.."] = ".",
            ["..###"] = ".",
            ["..#.."] = ".",
            [".#..#"] = "#",
            [".##.."] = "#",
            ["##..."] = "#",
            [".#.#."] = "#",
            [".###."] = "#",
            ["#..#."] = ".",
            ["####."] = ".",
            [".####"] = "#",
            ["#.##."] = "#",
            ["##.##"] = ".",
            ["..##."] = ".",
            ["#...#"] = "#",
        };
    }

    private (string, int) Next(string input, int index = 0)
    {
        var sb = new StringBuilder();
        var prep = new string('.', 4);
        sb.Append(prep);
        //var input = "#..#.#..##......###...###";
        sb.Append(input);
        sb.Append(prep);
        var output = sb.ToString();

        var len = output.Length;
        var data = Enumerable.Range(0, len - 5)
            .Select(x => output.Substring(x, 5))
            .Select(key => Map[key]);

        var projection = string.Join(string.Empty, data);
        var start = projection.IndexOf('#');
        var end = projection.LastIndexOf('#');
        var count = end - start + 1;
        return (projection.Substring(start, count), start - 2);
    }
}

