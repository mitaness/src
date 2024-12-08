
using System.Text;

namespace maz;

class Program
{
    static void Main(string[] args)
    {
        Test5();
    }

    private static void Test5()
    {
        new TestRunnerInput().Start();
    }

    private static void Test4()
    {
        var slow = 1;
        var fast = 1;
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

        slow = 1; // reset to head
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

    private static int Next(int v)
    {
        if (v == 10) return 3;
        return v + 1;
    }

    private static void Test3()
    {
        var list = new LinkedList<int>(Enumerable.Range(1, 5));
        list.AddFirst(list.First.Next);
    }

    private static void Test2()
    {
        var list = new LinkedList<int>(Enumerable.Range(1, 5));
        var a = list.First();
        var b = list.Last();
        var c = list.First();
        LinkedListNode<int> aNode = list.First.Next;
        var node = new LinkedListNode<int>(7);
        list.AddAfter(list.First, 20);
    }

    private static void Test()
    {
        var map = new Dictionary<string, string>()
        {
            ["...##"] = "#",
            ["..#.."] = "#"
        };

        var sb = new StringBuilder();
        var prep = new string('.', 4);
        sb.Append(prep);
        var input = "#..#.#..##......###...###";
        sb.Append(input);
        sb.Append(prep);
        var output = sb.ToString();
        var start = output.IndexOf('#');
        var end = output.LastIndexOf('#');
        var len = input.Length;
        var a = input.Substring(0, 5);
        var b = input.Substring(1, 5);
        var c = input.Substring(len - 5, 5);
        var rand = new Random();
        var a1 = rand.Next(10);
        var rrr = Enumerable.Range(0, 10)
            .Select(x => output.Substring(x, 5))
            .Select(x =>
            {
                var xx = rand.Next(10);
                return (xx < 5) ? "." : "#";
            })
            .ToArray();
    }
}
