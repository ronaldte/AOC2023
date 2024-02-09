using System.Text.RegularExpressions;

var lines = File.ReadAllLines(@"input.txt");

var containsSymbol = (string str) => Regex.IsMatch(str, @"[^.\d]");
var gears = new Dictionary<(int, int), List<int>>();
var sum = 0;

for (int i = 0; i < lines.Length; i++)
{
    var matches = Regex.Matches(lines[i], @"\d+").Cast<Match>();
    foreach (var match in matches)
    {
        var previousLine = Math.Max(i - 1, 0);
        var nextLine = Math.Min(i + 1, lines.Length - 1);

        var idx = match.Index;
        var idxBefore = Math.Max(idx - 1, 0);
        var idxAfter = Math.Min(idx + match.Length + 1, lines[i].Length - 1);

        for (int line = previousLine; line <= nextLine; line++)
        {
            // Part 1 - number has a symbol nearby then it's valid number
            //if (containsSymbol(lines[line][idxBefore..idxAfter]))
            //{
            //    sum += int.Parse(match.Value);
            //    break;
            //}

            // Part 2 - find all * around number, save each stars' location with the number
            var stars = Regex.Matches(lines[line][idxBefore..idxAfter], @"\*").Cast<Match>();
            foreach (var starsMatch in stars)
            {
                var key = (line, starsMatch.Index + idxBefore);
                var value = int.Parse(match.Value);
                
                if (!gears.TryGetValue(key, out var list))
                {
                    list = [];
                    gears[key] = list;
                }
                list.Add(value);
            }
        }
    }
}

Console.WriteLine($"Part 1: {sum}");
Console.WriteLine("Part 2: " + gears.Where(gears => gears.Value.Count == 2).Select(gears => gears.Value.First() * gears.Value.Last()).Sum());