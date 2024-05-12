using Logic;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;
        public GameOverMenu(GameState gameState)
        { 
            InitializeComponent();

            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);
        }

        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.White => "White Wins!",
                Player.Black => "Black Wins",
                _ => "Draw"
            };
        }

        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.White=> "White",
                Player.Black=> "Black",
                _=> ""
            };
        }

        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            return reason switch
            {
                EndReason.Stalemate => $"Stalemate - {PlayerString(currentPlayer)} can't move",
                EndReason.Checkmate => $"Checkmate - {PlayerString(currentPlayer)} can't move",
                EndReason.FiftyMoveRule => $"FiftyMoveRule - {PlayerString(currentPlayer)} can't move",
                EndReason.ThreefoldRepetition => $"ThreefoldRepetition - {PlayerString(currentPlayer)} can't move",
                EndReason.InsufficientMaterial => $"InsufficientMaterial - {PlayerString(currentPlayer)} can't move",
                _ => ""
            };
        }

        private void Restart_btn(object sender, RoutedEventArgs e)
        {
            OptionSelected.Invoke(Option.Restart);
        }

        private void Exit_btn(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Exit);
        }
    }
}
