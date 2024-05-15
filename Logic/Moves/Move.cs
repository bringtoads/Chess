namespace Logic.Moves
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPos { get; }
        public abstract Position ToPos { get; }
        
        public abstract bool Execute(Board board);

        public virtual bool IsLegal(Board board)
        {
            //refactor -- slow af
            Player player = board[FromPos].Color;
            var boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(player);
        }
    }
}
