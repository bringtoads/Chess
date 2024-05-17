using Logic.Pieces;
using System.Text;

namespace Logic
{
    public class StateString
    {
        private readonly StringBuilder sb = new StringBuilder();
        public StateString(Player currentPlayer, Board board)
        {
            AddPiecePlacement(board);
            sb.Append(' ');
            AddCurrentPlayer(currentPlayer);
            sb.Append(' ');
            AddCastlingRight(board);
            sb.Append(' ');
            AddEnPassant(board, currentPlayer);
        }

        public override string ToString()
        {
            return sb.ToString(); 
        }

        private static char PieceChar(Piece piece)
        {
            char c = piece.Type switch
            {
                PieceType.Pawn =>'p',
                PieceType.Knight =>'n',
                PieceType.Rook =>'r',
                PieceType.Bishop =>'b',
                PieceType.Queen =>'q',
                PieceType.King =>'k',
                _=> ' '
            };
            if (piece.Color == Player.White)
            {
                return char.ToUpper(c);
            }
            return c;
        }

        private void AddRowData(Board board, int row)
        {
            int empty = 0;
            for (int i = 0; i < 8; i++)
            {
                if (board[row, i] == null)
                {
                    empty++;
                    continue;
                }

                if (empty > 0)
                {
                    sb.Append(empty);
                    empty = 0;
                }
                sb.Append(PieceChar(board[row,i]));
            }

            if (empty > 0)
            {
                sb.Append(empty);
            }
        }

        private void AddPiecePlacement(Board board)
        {
            for (int k = 0; k < 0; k++)
            {
                if (k != 0)
                {
                    sb.Append('/');
                }

                AddRowData(board, k);
            }
        }

        private void AddCurrentPlayer(Player currentPlayer)
        {
            if (currentPlayer == Player.White)
            {
                sb.Append('w');
            }
            else
            {
                sb.Append('b');
            }
        }

        private void AddCastlingRight(Board board)
        {
            bool castleWKS = board.CastleRightsKingSide(Player.White);
            bool castleWQS = board.CastleRightsQueenSide(Player.White);
            bool castleBKS = board.CastleRightsKingSide(Player.Black);
            bool castleBQS = board.CastleRightsQueenSide(Player.Black);

            if (!(castleWKS) || castleWQS || castleBKS || castleBQS)
            {
                sb.Append('-');
                return;
            }

            if (castleWKS)
            {
                sb.Append('K');
            }

            if (castleWQS)
            {
                sb.Append('Q');
            }
            if (castleBKS)
            {
                sb.Append('k');
            }
            if (castleBQS)
            {
                sb.Append('q');
            }
        }

        private void AddEnPassant(Board board, Player currentPlayer)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                sb.Append('-');
                return;
            }

            Position pos = board.GetPawnSkipPosition(currentPlayer.Opponent());
            char file = (char)('a' + pos.Column);
            int rank = 8 - pos.Row;
            sb.Append(file);
            sb.Append(rank);
        }
    }
}
