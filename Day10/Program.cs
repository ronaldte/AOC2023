var map = File.ReadAllLines(@"input.txt");

// Find start location
var start = map.Select((line, lineIndex) => new Coordinate(lineIndex, line.IndexOf('S'))).First(c => c.Column != -1);

// Find all possible directions - there will always be only 2
var possibleDirections = new ValueTuple<Coordinate, Direction>[]
{
    (new(start.Row, start.Column - 1), Direction.Left),
    (new(start.Row, start.Column + 1), Direction.Right),
    (new(start.Row - 1, start.Column), Direction.Up),
    (new(start.Row + 1, start.Column), Direction.Down)
}.Where(c => ValidConnection(c.Item2, map[c.Item1.Row][c.Item1.Column]) == true).Select(c => c.Item1);

var steps = 1;
var area = 0;
var previous = start;
var current = possibleDirections.First();

// Traverse the pipes till we get back to the start, to prevent going backwards, i store previous and current
while (!current.Equals(start))
{
    area += previous.Row * current.Column - current.Row * previous.Column;

    var pipe = map[current.Row][current.Column];
    var nextCoordinate = GetNextCoordinate(pipe, current, previous);
    previous = current;
    current = nextCoordinate;
    steps++;
}

// Adding the last point
area += previous.Row * current.Column - current.Row * previous.Column;

//Console.WriteLine($"Part 1 {steps/2}");
Console.WriteLine($"Part 2 {Math.Abs(area) / 2 + 1 - steps / 2}");

// Method to get next pipe coordinate to move to, this takes into account the previous pipe, or direction
// where we came from so we don't get stuck in a loop. There are always just two possible way where to go
// based on type of the pipe - if we came from left - right or vice versa.
static Coordinate GetNextCoordinate(char tile, Coordinate current, Coordinate previous)
{
    return tile switch
    {
        '-' => new Coordinate(current.Row, current.Column - 1).Equals(previous)
                ? new Coordinate(current.Row, current.Column + 1)
                : new Coordinate(current.Row, current.Column - 1),
        '|' => new Coordinate(current.Row - 1, current.Column).Equals(previous)
                ? new Coordinate(current.Row + 1, current.Column)
                : new Coordinate(current.Row - 1, current.Column),
        'L' => new Coordinate(current.Row - 1, current.Column).Equals(previous)
                ? new Coordinate(current.Row, current.Column + 1)
                : new Coordinate(current.Row - 1, current.Column),
        'J' => new Coordinate(current.Row - 1, current.Column).Equals(previous)
                ? new Coordinate(current.Row, current.Column - 1)
                : new Coordinate(current.Row - 1, current.Column),
        '7' => new Coordinate(current.Row, current.Column - 1).Equals(previous)
                ? new Coordinate(current.Row + 1, current.Column)
                : new Coordinate(current.Row, current.Column - 1),
        'F' => new Coordinate(current.Row, current.Column + 1).Equals(previous)
                ? new Coordinate(current.Row + 1, current.Column)
                : new Coordinate(current.Row, current.Column + 1),
        _ => throw new ArgumentException($"Unsupported tile: {tile}", nameof(tile)),
    };
}

// Checks if the pipe is correctly connected and can be used, only used from the start.
static bool ValidConnection(Direction direction, char pipe)
{
    return direction switch
    {
        Direction.Up when pipe == '|' || pipe == '7' || pipe == 'F' => true,
        Direction.Down when pipe == '|' || pipe == 'J' || pipe == 'L' => true,
        Direction.Left when pipe == '-' || pipe == 'F' || pipe == 'L' => true,
        Direction.Right when pipe == '-' || pipe == '7' || pipe == 'J' => true,
        _ => false,
    };
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

// Simplification for use of tuple
class Coordinate(int row, int column)
{
    public int Column { get; set; } = column;

    public int Row { get; set; } = row;

    public override bool Equals(object? obj)
    {
        if (obj is Coordinate coordinate)
        {
            return Row == coordinate.Row && Column == coordinate.Column;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }
}
