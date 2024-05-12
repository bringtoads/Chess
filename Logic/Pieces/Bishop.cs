namespace Logic.Pieces
{
    internal class Bishop : Piece
    {
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
    }
}
