using Logic.Moves;

namespace Logic.Pieces
{
    public class Pawn : Piece
    {
        private readonly Direction forward;
        public override PieceType Type => PieceType.Pawn;

        public override Player Color { get; }

        public Pawn(Player color)
        {
            Color = color;
            if (color == Player.White)
            {
                forward = Direction.North;
            }
            else if (color == Player.Black)
            {
                forward = Direction.South;
            }
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }

        private static bool CanMoveTo(Position pos, Board board)
        {
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }

        private bool CanCaptureAt(Position pos, Board board)
        {
            if (!Board.IsInside(pos) || board.IsEmpty(pos))
            {
                return false;
            }
            return board[pos].Color != Color;
        }

        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            var oneMovePosition = from + forward;
            if(CanMoveTo(oneMovePosition,board))
            {
                yield return new NormalMove(from, oneMovePosition);
                Position toMovePosition = oneMovePosition + forward;
                if (!HasMoved && CanMoveTo(toMovePosition, board)) 
                {
                    yield return new NormalMove(from, toMovePosition);
                }
            }
        }

        private IEnumerable<Move> DiagonalMove(Position from, Board board)
        {
            foreach (Direction dir in new Direction[] { Direction.West, Direction.East})
            {
                Position to = from + forward + dir;
                if (CanCaptureAt(to, board))
                {
                    yield return new NormalMove(from, to);
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMove(from, board));
        }
    }
}
