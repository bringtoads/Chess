using Logic.Moves;
using Logic.Pieces;
using System.Net.Http.Headers;
using System.Net.WebSockets;

namespace Logic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            { Player.White,null},
            { Player.Black,null}
        };
        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set {  pieces[row, col] = value; }
        }
        
        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPostion(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos;
        }

        public static Board Initial()
        {
            var board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(Player.Black);
                this[6, c] = new Pawn(Player.White);
            }
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column <8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var pos = new Position(i, c);

                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                var piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy()
        {
            var copy = new Board();
            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }

            return copy;
        }

        public Counting CountPieces()
        {
            var counting = new Counting();
            foreach (Position pos in PiecePositions())
            {
                var piece = this[pos];
                counting.Increament(piece.Color, piece.Type);
            }
            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingVsKing(counting) || IsKingBishopVsKing(counting) || IsKingKnightVsKing(counting) || IsKingBishopVsKingBishop(counting);
        }

        private static bool IsKingVsKing(Counting counting)
        {
            return counting.TotalCount == 2;
        }

        private static bool IsKingBishopVsKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVsKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        private bool IsKingBishopVsKingBishop(Counting counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }
            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }
            var wBishopPos = FindPiece(Player.White, PieceType.Bishop);
            var bBishopPos = FindPiece(Player.Black, PieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }

        private bool IsUnmovedKindAndRook(Position kingPos, Position rookPos)
        {
            if (IsEmpty(kingPos) || IsEmpty(rookPos))
            {
                return false;
            }
            var king = this[kingPos];
            var rook = this[rookPos];
            return king.Type == PieceType.King && rook.Type == PieceType.Rook && !king.HasMoved && !rook.HasMoved;
        }

        public bool CastleRightsKingSide(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKindAndRook(new Position(7,4), new Position(7,7)),
                Player.Black => IsUnmovedKindAndRook(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }

        public bool CastleRightsQueenSide(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKindAndRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnmovedKindAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        }

        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)
        {
            foreach (Position pos in pawnPositions.Where(IsInside))
            {
                Piece piece = this[pos];

                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                EnPassant move = new EnPassant(pos, skipPos);

                if (move.IsLegal(this))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPos = GetPawnSkipPosition(player.Opponent());

            if (skipPos == null)
            {
                return false;
            }

            Position[] pawnPositions = player switch
            {
                Player.White => new Position[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast},
                Player.Black => new Position[] { skipPos + Direction.NorthWest, skipPos + Direction.NorthEast},
                _ => []
            };

            return HasPawnInPosition(player, pawnPositions, skipPos);
        }
    }
}
