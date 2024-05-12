namespace Logic.Pieces
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;

        public override Player Color { get; }

        public King(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
    }
}
