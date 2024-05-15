﻿namespace Logic.Moves
{
    public class DoublePawn : Move
    {
        public override MoveType Type => MoveType.DoublePawn;

        public override Position FromPos { get; }

        public override Position ToPos { get; }
        private readonly Position skippedPosition;
        public DoublePawn(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
            skippedPosition = new Position((from.Row + to.Row) / 2, from.Column);
        }

        public override bool Execute(Board board)
        {
            Player player = board[FromPos].Color;
            board.SetPawnSkipPostion(player, skippedPosition);
            new NormalMove(FromPos, ToPos).Execute(board);

            return true;
        }
    }
}
