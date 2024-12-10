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
    var total = 0;

    Span<char> word = stackalloc char[4];

    for (var _line = 0; _line < lines.Length; _line++)
    {
        var currentLine = lines[_line];

        for (var _column = 0; _column < currentLine.Length; _column++)
        {
            if (currentLine[_column] != 'X')
                continue;

            var words = GetWords(ref _line, ref _column, currentLine, lines, length: 4);

            total += words.Count(w => w == "XMAS");
        }
    }

    WriteLine(total);
}

void SolvePartTwo(string[] lines)
{
    var total = 0;

    Span<char> word = stackalloc char[4];

    for (var _line = 0; _line < lines.Length; _line++)
    {
        var currentLine = lines[_line];

        for (var _column = 0; _column < currentLine.Length; _column++)
        {
            if (currentLine[_column] != 'A')
                continue;

            var words = GetWords(ref _line, ref _column, currentLine, lines, length: 2);

            if (words[4] is null || words[7] is null || words[5] is null || words[6] is null)
                continue;

            var diagonalLeftToRight = words[5]![1..] + "A" + words[6]![1..];
            var diagonalRightToLeft = words[4]![1..] + "A" + words[7]![1..];

            if (diagonalLeftToRight != "MAS" && diagonalLeftToRight != "SAM")
                continue;

            if (diagonalRightToLeft != "MAS" && diagonalRightToLeft != "SAM")
                continue;

            total += 1;
        }
    }

    WriteLine(total);
}

string?[] GetWords(ref int _line, ref int _column, string currentLine, string[] lines, int length)
{
    if (length <= 0)
        throw new ArgumentException("Word length must be greater than 0.", nameof(length));

    Span<char> word = stackalloc char[length];
    var words = new string?[8];

    var canGoRight = _column + length <= currentLine.Length;
    var canGoLeft = _column >= length - 1;
    var canGoUp = _line - (length - 1) >= 0;
    var canGoDown = _line + (length - 1) < lines.Length;

    // in line - right
    if (canGoRight)
        words[0] = currentLine[_column..(_column + length)];

    // in line - left
    if (canGoLeft)
    {
        for (var i = 0; i < length; i++)
            word[i] = currentLine[_column - i];

        words[1] = new string(word);
    }

    // column - up
    if (canGoUp)
    {
        for (var i = 0; i < length; i++)
            word[i] = lines[_line - i][_column];

        words[2] = new string(word);
    }

    // column - down
    if (canGoDown)
    {
        for (var i = 0; i < length; i++)
            word[i] = lines[_line + i][_column];

        words[3] = new string(word);
    }

    // diagonal - up right
    if (canGoUp && canGoRight)
    {
        for (var i = 0; i < length; i++)
            word[i] = lines[_line - i][_column + i];

        words[4] = new string(word);
    }

    // diagonal - up left
    if (canGoUp && canGoLeft)
    {
        for (var i = 0; i < length; i++)
            word[i] = lines[_line - i][_column - i];

        words[5] = new string(word);
    }

    // diagonal - down right
    if (canGoDown && canGoRight)
    {
        for (var i = 0; i < length; i++)
            word[i] = lines[_line + i][_column + i];

        words[6] = new string(word);
    }

    // diagonal - down left
    if (canGoDown && canGoLeft)
    {
        for (var i = 0; i < length; i++)
            word[i] = lines[_line + i][_column - i];

        words[7] = new string(word);
    }

    return words;
}
