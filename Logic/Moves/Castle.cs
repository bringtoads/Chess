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
                rookToPosition = new Position(kingPos.Row, 5);
            }
            else if (type == MoveType.CastleQS)
            {
                kingMoveDirection = Direction.West;
                ToPos = new Position(kingPos.Row, 2);
                rookFromPosition = new Position(kingPos.Row, 0);
                rookToPosition = new Position(kingPos.Row, 3);
            }
        }

        public override void Execute(Board board)
        {
            new NormalMove(FromPos, ToPos).Execute(board);
            new NormalMove(rookFromPosition, rookToPosition).Execute(board);
        }

        public override bool IsLegal(Board board)
        {
            Player player = board[FromPos].Color;

            if (board.IsInCheck(player))
            {
                return false;
            }

            Board copy = board.Copy();
            Position kingPosInCopy = FromPos;

            for (int i = 0; i < 2; i++)
            {
                new NormalMove(kingPosInCopy, kingPosInCopy + kingMoveDirection).Execute(copy);
                kingPosInCopy += kingMoveDirection;

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
