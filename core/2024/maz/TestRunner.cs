using System.Text;

namespace maz;
// Dec 8'24
// add start index

internal class TestRunner
{
    private Dictionary<string, string> Map { get; }

    public void Start()
    {
        var (result, index) = ("#..#.#..##......###...###", 0);
        int counter = 0;

        for (int i = 0; i < 20; i++)
        {
            (result, index) = Next(result, index);
            counter += index;
        }
        var abc = result
            .Select((x, i) => (x, i + counter))
            .Where(B => B.x == '#')
            .Select(B => B.Item2)
            .Sum();
        //var (result, index) = Next("#..#.#..##......###...###", 0);
        Find("#..#.#..##......###...###");
    }

    private void Find(string input)
    {
        var slow = input;
        var fast = input;
        int index = 0;
        while (true)
        {
            // move fast 2 nodes at a time
            (fast, index) = Next(fast, index);
            (fast, index) = Next(fast, index);
            (slow, index) = Next(slow, index);
            if (slow == fast)
            {
                // cycle detected
                break;
            }
        }

        slow = input; // reset to head
        while (true)
        {
            (fast, index) = Next(fast, index);
            (slow, index) = Next(slow, index);
            if (slow == fast)
            {
                // cycle start
                break;
            }
        }
    }

    public TestRunner()
    {
        Map = new Dictionary<string, string>()
        {
            ["...##"] = "#",
            ["..#.."] = "#",
            [".#..."] = "#",
            [".#.#."] = "#",
            [".#.##"] = "#",
            [".##.."] = "#",
            [".####"] = "#",
            ["#.#.#"] = "#",
            ["#.###"] = "#",
            ["##.#."] = "#",
            ["##.##"] = "#",
            ["###.."] = "#",
            ["###.#"] = "#",
            ["####."] = "#",
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
            .Select(key =>
            {
                var found = Map.TryGetValue(key, out var value);
                return !found ? "." : "#";
            });
        var projection = string.Join(string.Empty, data);
        var start = projection.IndexOf('#');
        var end = projection.LastIndexOf('#');
        var count = end - start + 1;
        return (projection.Substring(start, count), start - 2);
    }
}

// 0 1 2 3 #
// 0 1 . . .