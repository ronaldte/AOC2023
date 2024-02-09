var inputLines = File.ReadAllLines(@"input.txt");
var times = inputLines[0][5..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
var distances = inputLines[1][9..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

long results = 1;

// Part 1 - create time - distance pairs from input
//var races = times.Zip(distances, (time, distance) => (long.Parse(time), long.Parse(distance)));

// Part 2
var races = new List<(long, long)>() { (long.Parse(string.Concat(times)), long.Parse(string.Concat(distances))) };

// Instead of manually testing values, calculate quadratic equation, and it's root are start and end of our hold time range
foreach (var (time, distance) in races)
{
    var D = Math.Sqrt(time * time - 4 * distance);
    var start = (time + D) / 2;
    var end = (time - D) / 2;

    // Create range (smaller number, bigger number), we require whole numbers and also don't count numbers equal, as this would 
    // result in draw and not win.
    var holdTimesRange = ((long)Math.Floor(Math.Min(start, end) + 1), (long)Math.Ceiling(Math.Max(start, end) - 1));

    results *= holdTimesRange.Item2 - (holdTimesRange.Item1 - 1);
}

Console.WriteLine(results);