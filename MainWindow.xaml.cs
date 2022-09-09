using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;


namespace UnderwaterHockeyTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static MainWindow mainWindow;
        public static SecondGUI_Window secondGUI = new SecondGUI_Window();
        public static ControllerControl controllerControl = new ControllerControl();
        public static SettingsWindow settingsWindow = new SettingsWindow();
        public static KeyboardControl keyboardControl = new KeyboardControl();
        private static bool gameSelect = false, tmrDown = false, tmrUp = false, soundAllowed = false;
        private static int numSec = 0, numMin = 0;
        private static string gameType = "", gameCurrent;
        public MainWindow() { InitializeComponent(); }
        private void wndwMain_Loaded(object sender, RoutedEventArgs e)
        {            
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            mainWindow = this;
            FileController.CreateDirectories();
            secondGUI.Show();
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            
        }
        private void timer_Tick(object sender, EventArgs e)
        {            
            if (GameKeeper.TeamTimeEnabled)
            {
                if (GameKeeper.TeamTimeTakenWhite)
                    btnTeamTimeWhite.IsEnabled = false;                
                else
                    btnTeamTimeWhite.IsEnabled = true;                
                if (GameKeeper.TeamTimeTakenBlack)
                    btnTeamTimeBlack.IsEnabled = false;                
                else
                    btnTeamTimeBlack.IsEnabled = true;                
            }
            else
            {
                btnTeamTimeBlack.IsEnabled = false;
                btnTeamTimeWhite.IsEnabled = false;
            }
            DesignController.DisplayCourtTime();
            if (GameKeeper.StartFirstGame)
            {
                TimeSpan calc = GameKeeper.FirstGame.Subtract(DateTime.Now);
                int.TryParse(calc.ToString(@"hh"), out int hours);
                int.TryParse(calc.ToString(@"mm"), out int minutes);
                int.TryParse(calc.ToString(@"ss"), out int seconds);
                while (hours > 0)
                {
                    hours--;
                    minutes += 60;
                }
                GameKeeper.Minutes = minutes;
                GameKeeper.Seconds = seconds+2;
                DesignController.SetBackgrounds("red");
                DesignController.DisplayGameText("First game starts in");                               
                GameKeeper.StartFirstGame = false;
                tmrDown = true;
                soundAllowed = true;
                gameType = "firsthalf";
                
            }
            if (tmrDown)
            {
                if (GameKeeper.Seconds > 0)
                    GameKeeper.Seconds--;                
                else
                {
                    if (GameKeeper.Minutes > 0)
                    {
                        GameKeeper.Seconds += 59;
                        GameKeeper.Minutes--;
                    }
                    else
                        gameSelect = true;                    
                }
                if (soundAllowed)
                {
                    if (GameKeeper.Minutes == 0)
                    {
                        if (GameKeeper.Seconds == 30)
                        {
                            SoundController.DefaultDing();
                        }
                        else if (GameKeeper.Seconds > 0 && GameKeeper.Seconds <= 10)
                        {
                            SoundController.DefaultDing();
                        }
                        else if (GameKeeper.Seconds == 0)
                        {
                            SoundController.DefaultDing();
                            soundAllowed = false;
                        }
                    }
                }                
                numMin = 0;
                numSec = 0;
            }
            if (tmrUp)
            {
                if (numSec >= 60)
                {
                    numMin++;
                    numSec = 0;
                }
                else
                    numSec++;
                GameKeeper.CatchUpTime++;
            }
            if (GameKeeper.TeamTimeTaken)
            {                
                if (numSec > 0)
                    numSec--;                
                else
                {
                    if (numMin > 0)
                    {
                        numSec += 59;
                        numMin--;
                    }
                    else
                    {                        
                        GameKeeper.TeamTimeTaken = false;
                        tmrDown = true;
                        DesignController.DisplayGameText(gameCurrent);
                        DesignController.SetBackgrounds("grey");
                    }
                }                
            }
            if (gameSelect)
            {
                if (gameType == "firsthalf")
                {
                    soundAllowed = false;
                    DesignController.SetBackgrounds("grey");
                    DesignController.DisplayGameText("First Half");
                    GameKeeper.ResetGoals();
                    GameKeeper.ResetTeamTimeTaken();
                    GameKeeper.Minutes = GameKeeper.GameLength;
                    gameType = "halftime";                    
                }
                else if (gameType == "halftime")
                {
                    soundAllowed = true;
                    DesignController.SetBackgrounds("red");
                    if (GameKeeper.GameHalves == 1)
                        gameType = "endtime";                                           
                    else if (GameKeeper.GameHalves == 2)
                    {
                        GameKeeper.Minutes = GameKeeper.GameHalfTimeLength;
                        DesignController.DisplayGameText("Half Time");
                        gameType = "secondhalf";                        
                    }
                }
                else if (gameType == "secondhalf")
                {
                    soundAllowed = false;
                    DesignController.SetBackgrounds("grey");
                    DesignController.DisplayGameText("Second Half");                    
                    GameKeeper.Minutes = GameKeeper.GameLength;
                    GameKeeper.ResetTeamTimeTaken();
                    gameType = "endtime";
                }
                else if (gameType == "endtime")
                {
                    soundAllowed = true;
                    if (GameKeeper.SuddenDeath && GameKeeper.GoalsBlack == GameKeeper.GoalsWhite)
                        gameType = "suddendeath";                    
                    else
                        gameType = "gameover";
                }
                if (gameType == "suddendeath" || gameType == "suddendeath2")
                {
                    DesignController.SetBackgrounds("red");
                    if (gameType == "suddendeath")
                    {
                        DesignController.DisplayGameText("SUDDEN DEATH\nstarts in");
                        GameKeeper.Minutes = 1;
                        GameKeeper.CatchUpTime += 60;
                        gameType = "suddendeath2";
                    }
                    else if (gameType == "suddendeath2")
                    {
                        DesignController.DisplayGameText("SUDDEN DEATH");
                        tmrUp = true;
                        tmrDown = false;
                        gameType = "suddendeath3";
                    }                    
                }                
                if (gameType == "gameover")
                {
                    DesignController.SetBackgrounds("red");
                    DesignController.DisplayGameText("Game over!\nNext game in");
                    int numMax = GameKeeper.TimeBetweenGamesMax * 60;                    
                    while (GameKeeper.CatchUpTime > 0)
                    {
                        if (numMax > (GameKeeper.TimeBetweenGamesMin * 60))
                        {
                            GameKeeper.CatchUpTime--;
                            numMax--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    do
                    {
                        if (numMax >= 60)
                        {
                            numMin++;
                            numMax -= 60;
                        }
                        else
                        {
                            break;
                        }
                    } while (true); 
                    GameKeeper.Minutes = numMin;
                    GameKeeper.Seconds = numMax;                    
                    numMin = 0;
                    GameKeeper.GameNo += GameKeeper.GameNoIncrement;                    
                    gameType = "firsthalf";
                }                               
                gameSelect = false;
                GameKeeper.CatchUpTime++;
            }           
            if (gameType == "suddendeath3" || GameKeeper.RefTimeEnabled || GameKeeper.TeamTimeTaken)
            {
                DesignController.SetBackgrounds("red");                
                DesignController.DisplayGameTime(numMin,numSec);
            }
            else            
                DesignController.DisplayGameTime(GameKeeper.Minutes, GameKeeper.Seconds);                         
            DesignController.DisplayGameNo();
            FileController.AllFiles();
            DesignController.DisplayGoals();
        } 
        public static void GoalBlack()
        {
            string gameText = mainWindow.txtGameText.Text;
            if (gameText == "First Half" || gameText == "Second Half")
                GameKeeper.GoalsBlack++;
            else
            {
                if (gameType == "suddendeath3")
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to award a goal to black?", "", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        GameKeeper.GoalsBlack++;
                        gameType = "gameover";
                        gameSelect = true;
                        tmrUp = false;
                        tmrDown = true;
                    }
                }
                else if (GameKeeper.RefTimeEnabled || gameText == "Half Time" || gameText == "Game Over!\nNext game in" || GameKeeper.TeamTimeTaken)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to award a goal to black?", "", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                        GameKeeper.GoalsBlack++;
                }
                else
                    MessageBox.Show("Goal cannot be awarded right now");
            }
            DesignController.DisplayGoals();
        }
        private void btnGoalBlack_Click(object sender, RoutedEventArgs e) { GoalBlack(); }
        public static void GoalWhite()
        {
            string gameText = mainWindow.txtGameText.Text;
            if (gameText == "First Half" || gameText == "Second Half")
                GameKeeper.GoalsWhite++;
            else
            {
                if (gameType == "suddendeath3")
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to award a goal to white?", "", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        GameKeeper.GoalsWhite++;
                        gameType = "gameover";
                        gameSelect = true;
                        tmrUp = false;
                        tmrDown = true;
                    }
                }
                else if (GameKeeper.RefTimeEnabled || gameText == "Half Time" || gameText == "Game Over!\nNext game in" || GameKeeper.TeamTimeTaken)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to award a goal to white?", "", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                        GameKeeper.GoalsWhite++;
                }
                else
                    MessageBox.Show("Goal cannot be awarded right now");
            }
            DesignController.DisplayGoals();
        }
        private void btnGoalWhite_Click(object sender, RoutedEventArgs e) { GoalWhite(); }
        public static void RefTime()
        {
            string gameText = mainWindow.txtGameText.Text;
            if (gameText == "First Half" || gameText == "Second Half" || GameKeeper.RefTimeEnabled && GameKeeper.TeamTimeTaken == false)
            {
                if (GameKeeper.RefTimeEnabled)
                {
                    DesignController.DisplayGameText(gameCurrent);
                    DesignController.SetBackgrounds("grey");
                    mainWindow.lblGameTime.Content = "";
                    GameKeeper.RefTimeEnabled = false;
                    tmrUp = false;
                    tmrDown = true;
                }
                else
                {
                    gameCurrent = mainWindow.txtGameText.Text;
                    DesignController.DisplayGameText("Ref time");
                    mainWindow.lblGameTime.Content = "";
                    DesignController.SetBackgrounds("red");
                    GameKeeper.RefTimeEnabled = true;
                    tmrUp = true;
                    tmrDown = false;
                }
            }
            else
                MessageBox.Show("Referee time can only be called during play.");
        }
        private void btnRefTime_Click(object sender, RoutedEventArgs e) { RefTime(); }
        public static void TeamTimeBlack()
        {
            string gameText = mainWindow.txtGameText.Text;
            if (gameText == "First Half" || gameText == "Second Half" && GameKeeper.TeamTimeTaken == false)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to start team timeout for Black?", "", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    GameKeeper.CatchUpTime += 61;
                    GameKeeper.TeamTimeTakenBlack = true;
                    GameKeeper.TeamTimeTaken = true;
                    tmrDown = false;
                    numMin = 1;
                    gameCurrent = mainWindow.txtGameText.Text;
                    DesignController.DisplayGameTime(numMin, numSec);
                    DesignController.DisplayGameText("Timeout Black");
                    DesignController.SetBackgrounds("red");
                }
            }
            else
                MessageBox.Show("Team timeout can only be taken during play");
        }
        private void btnTeamTimeBlack_Click(object sender, RoutedEventArgs e) { TeamTimeBlack(); }
        public static void TeamTimeWhite()
        {
            string gameText = mainWindow.txtGameText.Text;
            if (gameText == "First Half" || gameText == "Second Half" && GameKeeper.TeamTimeTaken == false)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to start team timeout for White?", "", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    GameKeeper.CatchUpTime += 61;
                    GameKeeper.TeamTimeTakenWhite = true;
                    GameKeeper.TeamTimeTaken = true;
                    tmrDown = false;
                    numMin = 1;
                    gameCurrent = mainWindow.txtGameText.Text;
                    DesignController.DisplayGameTime(numMin, numSec);
                    DesignController.DisplayGameText("Timeout White");
                    DesignController.SetBackgrounds("red");
                }
            }
            else
                MessageBox.Show("Team timeout can only be taken during play");
        }
        private void btnTeamTimeWhite_Click(object sender, RoutedEventArgs e) { TeamTimeWhite(); }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (settingsWindow.IsActive == false)
                settingsWindow.ShowDialog();
            else 
                MessageBox.Show("Settings window is already open");             
        }
    }
}
