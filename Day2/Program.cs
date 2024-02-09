using System.Text.RegularExpressions;

// Part 1
//const int RED = 12;
//const int GREEN = 13;
//const int BLUE = 14;

var sum = 0;

foreach (string line in File.ReadLines(@"input.txt"))
{
    var gameNumber = int.Parse(Regex.Match(line, @"\d+").Value);

    var sets = line.Split(":")[1].Split(";");

    // Part 1
    //var possibleGame = true;

    // Part2
    Dictionary<string, int> result = new() { { "red", 0 }, { "green", 0 }, { "blue", 0 } };

    foreach (var set in sets)
    {
        var matches = Regex.Matches(set, "(\\d+)\\s+(green|blue|red)").Cast<Match>();

        // Part1
        //Dictionary<string, int> result = new() { { "red", 0 }, { "green", 0 }, { "blue", 0 } };

        foreach (Match match in matches)
        {
            string color = match.Groups[2].Value;
            int number = int.Parse(match.Groups[1].Value);

            // Part1
            //result[color] = number;
            //possibleGame &= result["red"] <= RED && result["green"] <= GREEN && result["blue"] <= BLUE;

            // Part2
            result[color] = Math.Max(result[color], number);
        }

    }
    // Part1
    //if (possibleGame) sum += gameNumber;

    // Part2
    sum += result["red"] * result["green"] * result["blue"];
}

Console.WriteLine(sum);