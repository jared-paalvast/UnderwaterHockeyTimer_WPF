using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace UnderwaterHockeyTimer
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        
        private static ControllerControl controllerControl = MainWindow.controllerControl;
        
        public SettingsWindow() { InitializeComponent(); }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        private void btnCloseSettings_Click(object sender, RoutedEventArgs e) { this.Hide(); }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to shutdown the program?", "Shutdown", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)            
                Application.Current.Shutdown();            
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {            
            int.TryParse(txtCatchUp.Text, out int catchUp);
            int.TryParse(txtGameMinutes.Text, out int gameMin);
            int.TryParse(txtGameHalves.Text, out int gameHalves);
            int.TryParse(txtGoalsWhite.Text, out int goalsWhite);
            int.TryParse(txtGoalsBlack.Text, out int goalsBlack);
            int.TryParse(txtGameNoIncrement.Text, out int gameNoIncrement);
            int.TryParse(txtGameNoStart.Text, out int gameNo);
            int.TryParse(txtHalfTimeLength.Text, out int halfTimeLength);
            int.TryParse(txtTimeBetweenGamesMax.Text, out int timeBetweenGamesMax);
            int.TryParse(txtTimeBetweenGamesMin.Text, out int timeBetweenGamesMin);
            string messagetext = "Error(s):";
            if (gameMin < 1)
                messagetext += "\nGame minutes: Please enter a value that is greater than 1";
            if (gameHalves > 2 || gameHalves < 1)
                messagetext += "\nGame Halves: Please enter the value 1 or 2";
            if (goalsBlack < 0 || goalsWhite < 0)
                messagetext += "\nGoal Scores: Please enter a value that is greater than 0";
            if (catchUp < 0)
                messagetext += "\nCatch up time: Please enter a value that is greater than 0";
            if (gameNoIncrement > 2 || gameNoIncrement < 1)
                messagetext += "\nGame Number Increment: Please enter the value 1 or 2";
            if (halfTimeLength < 1)
                messagetext += "\nHalf time length: Please enter a value that is greater than 1";
            if (timeBetweenGamesMin < 1 || timeBetweenGamesMax < 1)            
                messagetext += "\nTime between games length: Please enter a value that is greater than 1";            
            if (messagetext != "Error(s):")
                MessageBox.Show(messagetext);            
            else
            {
                GameKeeper.GameLength = gameMin;
                GameKeeper.CatchUpTime = catchUp;
                GameKeeper.GameHalves = gameHalves;
                GameKeeper.GoalsBlack = goalsBlack;
                GameKeeper.GoalsWhite = goalsWhite;
                GameKeeper.GameNo = gameNo;
                GameKeeper.GameNoIncrement = gameNoIncrement;
                GameKeeper.GameHalfTimeLength = halfTimeLength;
                GameKeeper.TimeBetweenGamesMax = timeBetweenGamesMax;
                GameKeeper.TimeBetweenGamesMin = timeBetweenGamesMin;
                if (chkTeamTimeout.IsChecked == true)
                    GameKeeper.TeamTimeEnabled = true;
                else if (chkTeamTimeout.IsChecked == false)
                    GameKeeper.TeamTimeEnabled = false;
                if (chkSuddenDeath.IsChecked == true)
                    GameKeeper.SuddenDeath = true;
                else if (chkSuddenDeath.IsChecked == false)
                    GameKeeper.SuddenDeath = false;
                MessageBox.Show("Options Saved!");
            }            
        }
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsActive == false)
            {
                tabControlSettings.SelectedIndex = 0;
                txtGoalsBlack.Text = GameKeeper.GoalsBlack.ToString();
                txtGoalsWhite.Text = GameKeeper.GoalsWhite.ToString();
                txtCatchUp.Text = GameKeeper.CatchUpTime.ToString();
                txtGameNoStart.Text = GameKeeper.GameNo.ToString();
            }
        }
        private void btnSetSecondGUI_Click(object sender, RoutedEventArgs e)
        { GameKeeper.SetSecondGUI = true; }
        private void btnSetFirstGame_Click(object sender, RoutedEventArgs e)
        {            
            bool isTimeValid = DateTime.TryParse(txtFirstGame.Text, out DateTime setGameTime);
            if (isTimeValid)
            {
                GameKeeper.FirstGame = setGameTime;
                MessageBox.Show($"First game set to: {GameKeeper.FirstGame.ToString("h:mmtt")}");
                GameKeeper.StartFirstGame = true;
            }
            else            
                MessageBox.Show("Improper format used! Please try again.\nFormat required: HH:MMtt");            
        }        
        private void btnCheckController_Click(object sender, RoutedEventArgs e)
        {
            controllerControl.CheckControllerConnection();
        }
        private void chkControllerEnabled_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (chkControllerEnabled.IsChecked == true)                        
                controllerControl.StartTimer();                            
            else            
                controllerControl.StopTimer();                           
        }
        private void btnSetBuzzer_Click(object sender, RoutedEventArgs e)
        {
            if (txtBuzzerName.Text == "" || txtBuzzerName.Text == null)            
                MessageBox.Show("Please input a file name");            
            else            
                FileController.SetBuzzer(txtBuzzerName.Text);                        
        }
        private void btnKeyBindings_Click(object sender, RoutedEventArgs e)
        {
            KeyBindingsWindow keyBindingsWindow = new KeyBindingsWindow();            
            keyBindingsWindow.Show();                        
        }
        private void chkGoalTextOutput_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (chkGoalTextOutput.IsChecked == true)
                FileController.GoalsOutput = true;
            else
                FileController.GoalsOutput = false;
        }
        private void chkGameTextOutput_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (chkGameTextOutput.IsChecked == true)
                FileController.GameTextOutput = true;
            else
                FileController.GameTextOutput = false;
        }
        private void chkGameNumberTextOutput_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (chkGameNumberTextOutput.IsChecked == true)
                FileController.GameNoOutput = true;
            else
                FileController.GameNoOutput = false;
        }
        private void chkGameTimeOutput_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (chkGameTimeOutput.IsChecked == true)
                FileController.GameTimeOutput = true;
            else
                FileController.GameTimeOutput = false;
        }
        private void chkGoalAward_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (chkGoalAward.IsChecked == true)
                ControllerControl.GoalAwardEnabled = true;
            else
                ControllerControl.GoalAwardEnabled = false;
        }
        private void chkTeamTimeControl_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (chkTeamTimeControl.IsChecked == true)
                ControllerControl.TeamTimeEnabled = true;
            else
                ControllerControl.TeamTimeEnabled = false;
        }
        private void chkRefTimeControl_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (chkRefTimeControl.IsChecked == true)
                ControllerControl.RefTimeEnabled = true;
            else
                ControllerControl.RefTimeEnabled = false;
        }        
    }
}
