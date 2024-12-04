using System.Collections;
using static System.Console;

var filename = args[0];
var part = args[1];

if (!File.Exists(filename))
    throw new InvalidOperationException("File not found.");

var input = await File.ReadAllTextAsync(filename);

if (part == "1")
    SolvePartOne(input);
else if (part == "2")
    SolvePartTwo(input);
else
    throw new InvalidOperationException("Part not found.");

void SolvePartOne(string line)
{
    var total = 0;

    var iter = new Iter(line);

    while (iter.MoveNext())
    {
        if (iter.Current != 'm' || !Matches(iter, "mul("))
            continue;

        var n1 = ReadNumber(iter);

        if (iter.Current != ',' || n1 is null)
            continue;

        var n2 = ReadNumber(iter);

        if (iter.Current != ')' || n2 is null)
            continue;

        total += n1.Value * n2.Value;
    }

    WriteLine(total);
}

void SolvePartTwo(string line)
{
    var total = 0;

    var iter = new Iter(line);

    var dont = false;

    while (iter.MoveNext())
    {
        switch (iter.Current)
        {
            case 'd' when Matches(iter, "do"):
                if (Matches(iter, "o()"))
                    dont = false;
                else if (Matches(iter, "on't()"))
                    dont = true;

                break;

            case 'm' when Matches(iter, "mul("):
                var n1 = ReadNumber(iter);

                if (iter.Current != ',' || n1 is null)
                    continue;

                var n2 = ReadNumber(iter);

                if (iter.Current != ')' || n2 is null)
                    continue;

                if (!dont)
                    total += n1.Value * n2.Value;

                break;
        }
    }

    WriteLine(total);
}

int? ReadNumber(Iter iter)
{
    int? total = null;

    while (iter.MoveNext() && char.IsDigit(iter.Current))
    {
        var number = iter.Current - '0';

        total = ((total ?? 0) * 10) + number;
    }

    return total;
}

bool Matches(Iter iter, string value)
{
    foreach (var c in value.Skip(1))
    {
        if (iter.Peek() == c)
            iter.MoveNext();
        else
            return false;
    }

    return true;
}

internal class Iter(string Line) : IEnumerator<char>
{
    private int _index = -1;

    public char Current => Line[_index];

    object IEnumerator.Current => Current;

    public void Dispose() { }

    public bool MoveNext() => ++_index < Line.Length;

    public void Reset() => _index = 0;

    public char? Peek() => _index + 1 < Line.Length ? Line[_index + 1] : null;
}
