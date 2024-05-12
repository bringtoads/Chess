using Logic.Pieces;

namespace Logic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
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

        public static Board Initial()
        {
            var board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 0] = new Knight(Player.Black);
            this[0, 0] = new Bishop(Player.Black);
            this[0, 0] = new Queen(Player.Black);
            this[0, 0] = new King(Player.Black);
            this[0, 0] = new Bishop(Player.Black);
            this[0, 0] = new Knight(Player.Black);
            this[0, 0] = new Rook(Player.Black);

            this[0, 0] = new Rook(Player.White);
            this[0, 0] = new Knight(Player.White);
            this[0, 0] = new Bishop(Player.White);
            this[0, 0] = new Queen(Player.White);
            this[0, 0] = new King(Player.White);
            this[0, 0] = new Bishop(Player.White);
            this[0, 0] = new Knight(Player.White);
            this[0, 0] = new Rook(Player.White);

            for (int c = 0; c < 8; c++)
            {
                this[1, c] = new Pawn(Player.Black);
                this[6, c] = new Pawn(Player.Black);
            }
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column <8;
        }

        public bool IsEmpyt(Position pos)
        {
            return this[pos] == null;
        }
    }
}
