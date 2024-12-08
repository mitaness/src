using System.Text;

namespace maz;

internal class RunnerInput
{
    private Dictionary<string, string> Map { get; }

    public void Start()
    {
        var result = Next("#..#.#..##......###...###");
        Find("##...#......##......#.####.##.#..#..####.#.######.##..#.####...##....#.#.####.####.#..#.######.##...");
    }

    private void Find(string input)
    {
        var slow = input;
        var fast = input;
        while (true)
        {
            // move fast 2 nodes at a time
            fast = Next(fast);
            fast = Next(fast);
            slow = Next(slow);
            if (slow == fast)
            {
                // cycle detected
                break;
            }
        }

        slow = input; // reset to head
        while (true)
        {
            fast = Next(fast);
            slow = Next(slow);
            if (slow == fast)
            {
                // cycle start
                break;
            }
        }
    }

    public RunnerInput()
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

    private string Next(string input)
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
        return projection.Substring(start, count);
    }
}
