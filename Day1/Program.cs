using System.Text.RegularExpressions;

var sum = 0;

foreach (string line in File.ReadLines(@"input.txt"))
{
    var wordToDigit = new Dictionary<string, int>()
    {
        {"zero", 0},
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9},
        {"0", 0},
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
        {"5", 5},
        {"6", 6},
        {"7", 7},
        {"8", 8},
        {"9", 9}
    };

    // Part1
    //var pattern = @"(\d)";

    // Part2
    var pattern = @"(?=(?<number>\d|zero|one|two|three|four|five|six|seven|eight|nine))";

    var matches = new Regex(pattern).Matches(line).Select(m => wordToDigit[m.Groups["number"].Value]);

    sum += matches.First() * 10 + matches.Last();
}

Console.WriteLine(sum);