using System.Windows.Media;
using System;

namespace UnderwaterHockeyTimer
{
    public static class DesignController
    {
        private static MainWindow mainWindow = MainWindow.mainWindow;
        private static SecondGUI_Window secondGUI = MainWindow.secondGUI;
        public static void SetBackgrounds(string colour)
        {
            if (colour == "red")
            {
                secondGUI.Background = Brushes.Red;
                mainWindow.Background = Brushes.Red;
            }
            else if (colour == "grey")
            {
                secondGUI.Background = Brushes.Gray;
                mainWindow.Background = Brushes.Gray;
            }            
        }
        public static void DisplayCourtTime()
        {
            string time = $"Court time: {DateTime.Now:h:mm:sstt}";
            mainWindow.lblCourtTime.Content = time;
            secondGUI.lblCourtTime.Content = time;
        }
        public static void DisplayGameTime(int minutes, int seconds)
        {
            string disSec, disMin;
            if (minutes < 10)
                disMin = $"0{minutes}";
            else
                disMin = minutes.ToString();
            if (seconds < 10)
                disSec = $"0{seconds}";
            else
                disSec = seconds.ToString();
            mainWindow.lblGameTime.Content = $"{disMin}:{disSec}";
            secondGUI.lblGameTime.Content = $"{disMin}:{disSec}";
        }   
        public static void DisplayGoals()
        {
            mainWindow.lblScoreWhite.Content = GameKeeper.GoalsWhite;
            secondGUI.lblScoreWhite.Content = GameKeeper.GoalsWhite;
            mainWindow.lblScoreBlack.Content = GameKeeper.GoalsBlack;
            secondGUI.lblScoreBlack.Content = GameKeeper.GoalsBlack;
        }
        public static void DisplayGameText(string text)
        {
            mainWindow.txtGameText.Text = text;
            secondGUI.txtGameText.Text = text;
        }
        public static void DisplayGameNo()
        {
            string text = $"Game No. {GameKeeper.GameNo}";
            mainWindow.lblGameNo.Content = text;
            secondGUI.lblGameNo.Content = text;
        }
    }
}
