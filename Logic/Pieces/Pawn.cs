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

        private static IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PieceType.Knight);
            yield return new PawnPromotion(from, to, PieceType.Bishop);
            yield return new PawnPromotion(from, to, PieceType.Rook);
            yield return new PawnPromotion(from, to, PieceType.Queen);
        }

        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            var oneMovePosition = from + forward;
            if(CanMoveTo(oneMovePosition,board))
            {
                if (oneMovePosition.Row == 0 || oneMovePosition.Row == 7)
                {
                    foreach (Move promMove in PromotionMoves(from, oneMovePosition))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, oneMovePosition);
                }
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
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promMove in PromotionMoves(from, to))
                        {
                            yield return promMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(from, to);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMove(from, board));
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return DiagonalMove(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
