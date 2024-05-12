using Logic.Moves;

namespace Logic.Pieces
{
    public class Rook : Piece
    {
        private static readonly Direction[] directions = new Direction[]
            {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
        public override PieceType Type => PieceType.Rook;

        public override Player Color { get; }
        public Rook(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new Rook(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionsInDirections(from, board, directions).Select(to => new NormalMove(from, to));
        }
    }
}
