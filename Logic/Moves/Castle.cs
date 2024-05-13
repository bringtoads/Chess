namespace Logic.Moves
{
    public class Castle : Move
    {
        public override MoveType Type { get; }

        public override Position FromPos { get; }

        public override Position ToPos { get; }

        private readonly Direction kingMoveDirection;
        private readonly Position rookFromPosition;
        private readonly Position rookToPosition;
        public Castle(MoveType type, Position kingPos)
        {
            Type = type;
            FromPos = kingPos;
            if (type == MoveType.CastleKS)
            {
                kingMoveDirection = Direction.East;
                ToPos = new Position(kingPos.Row, 6);
                rookFromPosition = new Position(kingPos.Row, 7);
                rookToPosition = new Position(kingPos.Row, 3);
            }
        }
        public override void Execute(Board board)
        {
            throw new NotImplementedException();
        }
    }
}
