var map = File.ReadLines(@"input.txt");

var transposedMap = Enumerable.Range(0, map.First().Length)
                              .Select(i => string.Concat(map.Select(s => s[i])));

var emptyRowsIdx = GetEmptyLineIndexes(map);
var emptyColumnsIdx = GetEmptyLineIndexes(transposedMap);

var galaxies = FindAllGalaxies(map.ToList());

// Instead of expanding the map itself, just shift the galaxies by the number of empty lines
var shiftAdjustedGalaxies = galaxies.Select(galaxy => PointShift(galaxy, emptyRowsIdx, emptyColumnsIdx)).ToList();

var galaxiesCombinations = GetAllPairs(shiftAdjustedGalaxies);

var distanceSum = galaxiesCombinations.Select(gc => CalculateDistance(gc.Item1, gc.Item2)).Sum();


Console.WriteLine($"Solution: {distanceSum}");


// Check if line consists only from only one type of character
static bool IsStringOfSymbol(string text, char symbol = '.') => text.All(character => character == symbol);

// Returns all index of empty lines on map
static IEnumerable<int> GetEmptyLineIndexes(IEnumerable<string> lines)
{
    return lines.Select((text, index) => new { text, index })
                .Where(line => IsStringOfSymbol(line.text))
                .Select(line => line.index);
}

// Returns all coordinates for galaxies found on map
static IEnumerable<Coordinate> FindAllGalaxies(IList<string> map, char galaxySymbol = '#')
{
    var galaxies = new List<Coordinate>();

    for (int x = 0; x < map.Count; x++)
    {
        for (int y = 0; y < map[x].Length; y++)
        {
            if (map[x][y] == galaxySymbol)
            {
                galaxies.Add(new Coordinate(x, y));
            }
        }
    }

    return galaxies;
}

// Return galaxies Combination
static IEnumerable<ValueTuple<Coordinate, Coordinate>> GetAllPairs(IList<Coordinate> coordinates)
{
    var pairs = new List<ValueTuple<Coordinate, Coordinate>>();

    for (int i = 0; i < coordinates.Count; i++)
    {
        for (int j = i + 1; j < coordinates.Count; j++)
        {
            pairs.Add((coordinates[i], coordinates[j]));
        }
    }

    return pairs;
}

// Shifts the point based on empty lines both horizontal and vertical
static Coordinate PointShift(Coordinate point, IEnumerable<int> rows, IEnumerable<int> cols)
{
    point.X += AdjustCoordinate(point.X, rows);
    point.Y += AdjustCoordinate(point.Y, cols);
    return point;
}

// Calculate how much should the number shift based on the number of empty lines on the map, the multiplier
// represents how much does single line expand
static long AdjustCoordinate(long coordinate, IEnumerable<int> indexes, int multiplier = 1000000) => indexes.Count(s => s < coordinate) * (multiplier - 1);

// Use Manhattan distance formula
static long CalculateDistance(Coordinate pointA, Coordinate pointB) => Math.Abs(pointA.X - pointB.X) + Math.Abs(pointA.Y - pointB.Y);

abstract class Point<T>(T x, T y)
{
    public T X = x;
    public T Y = y;
}

class Coordinate(long x, long y) : Point<long>(x, y) { }
