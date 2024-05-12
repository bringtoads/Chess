using Logic.Moves;

namespace Logic.Pieces
{
    public class King : Piece
    {
        private static readonly Direction[] directions = new Direction[]
            {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West,
                Direction.NorthWest,
                Direction.NorthEast,
                Direction.SouthEast,
                Direction.SouthWest
            };

        public override PieceType Type => PieceType.King;

        public override Player Color { get; }

        public King(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (Direction dir in directions)
            {
                Position to = from + dir;
                if (!Board.IsInside(to))
                {
                    continue;
                }
                if (board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }
        }
    }
}
