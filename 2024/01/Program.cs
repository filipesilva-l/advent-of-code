using System.Text.Json;
using static System.Console;

var filename = args[0];
var part = args[1];

if (!File.Exists(filename))
    throw new InvalidOperationException("File not found.");

var input = await File.ReadAllLinesAsync(filename);

if (part == "1")
    SolvePartOne(input);
else if (part == "2")
    SolvePartTwo(input);
else
    throw new InvalidOperationException("Part not found.");

void SolvePartOne(string[] input)
{
    var (left, right) = SplitLists(input);

    left = [.. left.OrderBy(i => i)];
    right = [.. right.OrderBy(i => i)];

    var total = 0;

    for (var i = 0; i < left.Count; i++)
    {
        var leftItem = left[i];
        var rightItem = right[i];

        total += leftItem > rightItem ? leftItem - rightItem : rightItem - leftItem;
    }

    WriteLine(total.ToString());
}

void SolvePartTwo(string[] input)
{
    var (left, right) = SplitLists(input);

    var dictionaryRight = right.GroupBy(r => r).ToDictionary(r => r.Key, r => r.Count());

    var total = 0;

    foreach (var leftItem in left)
    {
        if (!dictionaryRight.TryGetValue(leftItem, out var rightItemCount))
            continue;

        total += leftItem * rightItemCount;
    }

    WriteLine(total);
}

(List<int>, List<int>) SplitLists(string[] input)
{
    List<int> left = [];
    List<int> right = [];

    foreach (var line in input)
    {
        var splitted = line.Split(' ');

        left.Add(int.Parse(splitted[0]));
        right.Add(int.Parse(splitted[1]));
    }

    return (left, right);
}
