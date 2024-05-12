using Logic.Moves;

namespace Logic.Pieces
{
    internal class Bishop : Piece
    {
        private static readonly Direction[] directions = new Direction[]
            {
                Direction.NorthWest,
                Direction.NorthEast,
                Direction.SouthWest,
                Direction.SouthEast,
            };

        public override PieceType Type => PieceType.Bishop;

        public override Player Color { get; }

        public Bishop(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new Bishop(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirections(from, board, directions).Select(to => new NormalMove(from, to));
        }
    }
}
