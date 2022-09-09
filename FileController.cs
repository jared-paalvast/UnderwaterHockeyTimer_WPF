using System;
using System.IO;
using System.Windows;

namespace UnderwaterHockeyTimer
{
    public static class FileController
    {
        private static readonly string directoryMain = Environment.CurrentDirectory, directorySounds = directoryMain + @"\Sounds\";
        private static readonly string directoryText = directoryMain + @"\Text_Output\";
        private static readonly string pathGoalsWhite = directoryText + "goalsWhite.txt", pathGoalsBlack = directoryText + "goalsBlack.txt";
        private static readonly string pathGameText = directoryText + "gameText.txt", pathGameTime = directoryText + "gameTime.txt";
        private static readonly string pathGameNumber = directoryText + "gameNo.txt";
        public static bool GoalsOutput { get; set; } = false;
        public static bool GameTextOutput { get; set; } = false;
        public static bool GameTimeOutput { get; set; } = false;
        public static bool GameNoOutput { get; set; } = false;
        public static void CreateDirectories()
        {
            if (!Directory.Exists(directorySounds))
                Directory.CreateDirectory(directorySounds);
            if (!Directory.Exists(directoryText))
                Directory.CreateDirectory(directoryText);                                            
        }
        public static void SetBuzzer(string name)
        {
            string location = directorySounds + name;
            if (!File.Exists(location))            
                MessageBox.Show($"File not found.\nPlease make sure the file name is spelt correctly\nPlease place file in sounds folder:\n{directorySounds}");            
            else
            {
                MessageBox.Show("Buzzer sound set");
                SoundController.SetBuzzer(location);                
            }
        }
        private static void GoalsTextFile()
        {
            if (GoalsOutput)
            {
                File.WriteAllText(pathGoalsWhite, GameKeeper.GoalsBlack.ToString());
                File.WriteAllText(pathGoalsBlack, GameKeeper.GoalsBlack.ToString());
            }
            
        }
        private static void GameTextFile()
        {
            if (GameTextOutput)            
                File.WriteAllText(pathGameText, MainWindow.mainWindow.txtGameText.Text);           
        }
        private static void GameTimeFile()
        {
            if (GameTimeOutput)            
                File.WriteAllText(pathGameTime, MainWindow.mainWindow.lblGameTime.Content.ToString());            
        }
        private static void GameNumberFile()
        {
            if (GameNoOutput)            
                File.WriteAllText(pathGameNumber, GameKeeper.GameNo.ToString());            
        }
        public static void AllFiles()
        {
            GoalsTextFile();
            GameTextFile();
            GameTimeFile();
            GameNumberFile();
        }
    }
}
