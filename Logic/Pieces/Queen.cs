using Logic.Moves;

namespace Logic.Pieces
{
    public class Queen : Piece
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
        public override PieceType Type => PieceType.Queen;

        public override Player Color { get; }


        public Queen(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new Queen(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirections(from, board, directions).Select(to => new NormalMove(from, to));
        }
    }
}
