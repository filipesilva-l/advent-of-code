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

void SolvePartOne(string[] lines)
{
    var indexOfSlipt = Array.IndexOf(lines, "");

    var setup = lines[..(indexOfSlipt - 1)];

    var updates = lines[(indexOfSlipt + 1)..];

    var dict = new Dictionary<int, List<int>>();

    foreach (var line in setup)
    {
        var values = line.Split('|').Select(int.Parse).ToArray();

        if (!dict.TryGetValue(values[1], out var list))
            list = [];

        list.Add(values[0]);

        dict[values[1]] = list;
    }

    var total = 0;

    foreach (var line in updates)
    {
        var items = line.Split(',').Select(int.Parse).ToList();

        if (IsValid(items, dict))
            total += items[(int)Math.Floor(items.Count / 2m)];
    }

    WriteLine(total);
}

void SolvePartTwo(string[] lines)
{
    var indexOfSlipt = Array.IndexOf(lines, "");

    var setup = lines[..(indexOfSlipt - 1)];

    var updates = lines[(indexOfSlipt + 1)..];

    var dict = new Dictionary<int, List<int>>();

    foreach (var line in setup)
    {
        var values = line.Split('|').Select(int.Parse).ToArray();

        if (!dict.TryGetValue(values[1], out var list))
            list = [];

        list.Add(values[0]);

        dict[values[1]] = list;
    }

    var total = 0;

    foreach (var line in updates)
    {
        var items = line.Split(',').Select(int.Parse).ToList();

        if (IsValid(items, dict))
            continue;

        var newItems = Correct(dict, items);

        total += newItems[(int)Math.Floor(newItems.Count / 2m)];
    }

    WriteLine(total);
}

bool IsValid(List<int> items, Dictionary<int, List<int>> dict)
{
    for (var i = 0; i < items.Count - 1; i++)
    {
        var item = items[i];

        if (!dict.TryGetValue(item, out var list))
            continue;

        var subset = items[i..];

        if (list.Any(subset.Contains))
            return false;
    }

    return true;
}

List<int> Correct(Dictionary<int, List<int>> dict, List<int> items)
{
    List<int> newItems = [.. items];

    foreach (var (idx, item) in items.Index())
    {
        if (!dict.TryGetValue(item, out var list))
            continue;

        var last = newItems.LastOrDefault(list.Contains);
        var index = newItems.IndexOf(last);

        if (last == default || index < idx)
            continue;

        newItems.Remove(item);
        newItems.Insert(index, item);
    }

    if (!newItems.SequenceEqual(items))
        return Correct(dict, newItems);

    return newItems;
}
