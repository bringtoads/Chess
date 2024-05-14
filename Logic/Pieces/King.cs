using Logic.Moves;

namespace Logic.Pieces
{
    public class King : Piece
    {
        private static readonly Direction[] directions =
            [
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West,
                Direction.NorthWest,
                Direction.NorthEast,
                Direction.SouthEast,
                Direction.SouthWest
            ];

        public override PieceType Type => PieceType.King;

        public override Player Color { get; }

        public King(Player color)
        {
            Color = color;
        }

        private static bool IsUnmovedRook(Position pos, Board board)
        {
            if (board.IsEmpty(pos))
            {
                return false;
            }
            Piece piece = board[pos];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(pos => board.IsEmpty(pos));
        }

        private bool CanCastleKingSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(from.Row, 7);
            Position[] betweenPositions = [new(from.Row,5), new (from.Row,6)]; 
            
            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQueenSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            Position rookPos = new Position(from.Row, 0);
            Position[] betweenPositions = [new(from.Row, 1), new(from.Row, 2), new(from.Row, 3)];

            return IsUnmovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
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

            if (CanCastleKingSide(from, board))
            {
                yield return new Castle(MoveType.CastleKS, from);
            }
            if (CanCastleQueenSide(from, board))
            {
                yield return new Castle(MoveType.CastleQS, from);
            }
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return MovePositions(from, board).Any(to =>
            {
                var piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
