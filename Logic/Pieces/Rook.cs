namespace Logic.Pieces
{
    public class Rook : Piece
    {
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
    }
}
