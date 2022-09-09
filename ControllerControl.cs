using System;
using System.Collections.Generic;
using System.Threading;
using SharpDX.XInput;
using System.Windows;
using System.Windows.Threading;

namespace UnderwaterHockeyTimer
{
    public class ControllerControl
    {        
        private static Controller controller = new Controller(UserIndex.One);
        private static string stringToRemove = "abcdefghijklmnopqrstuvwxyz, P", newState;
        private static List<string> charsToRemove = new List<string>();        
        private static bool[] oldBool = new bool[10], newBool;        
        private static char[] newBtn;
        
        private static DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);
        public static bool RefTimeEnabled { get; set; } = false;
        public static bool TeamTimeEnabled { get; set; } = false;
        public static bool GoalAwardEnabled { get; set; } = false;
        public bool IsLoopingEnabled { get; set; } = false;  
        public ControllerControl()
        {            
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += TimerTick;
            foreach (char c in stringToRemove.ToCharArray())
            {
                charsToRemove.Add(c.ToString());
            }            
        }
        public void CheckControllerConnection()
        {
            if (controller.IsConnected)
            {
                MessageBox.Show("A Controller is connected");
            }
            else
            {                
                MessageBox.Show("No Controller(s) are connected");
            }
        }
        public void StartTimer()
        {
            if (controller.IsConnected)
            {                
                timer.Start();
                MessageBox.Show("Controller input should now be active");
            }
            else
            {
                MessageBox.Show("No Controller(s) are connected");
                MainWindow.settingsWindow.chkControllerEnabled.IsChecked = false;
            }        
        }
        public void StopTimer()
        {
            if (controller.IsConnected)
            {
                timer.Stop();
                MessageBox.Show("Controller input should now be inactive");
            }            
        }        
        private void TimerTick(object sender, EventArgs e)
        {
            if (controller.IsConnected == false)
            {
                MessageBox.Show("Controller Connection Lost!\nPlease re-connect controller");
                timer.Stop();
            }
            newState = controller.GetState().Gamepad.Buttons.ToString();            
            newState = newState.Replace("Back", string.Empty);
            newState = newState.Replace("None", string.Empty);
            newState = newState.Replace("Start", string.Empty);
            foreach (var c in charsToRemove)
            {
                newState = newState.Replace(c, string.Empty);
            }
            newState = newState.Replace("DL", "L");
            newState = newState.Replace("DR", "R");
            newState = newState.Replace("DU", "U");
            newState = newState.Replace("DD", "D");
            newState = newState.Replace("LS", "l");
            newState = newState.Replace("RS", "r");
            newState = newState.Replace("RT", string.Empty);
            newState = newState.Replace("LT", string.Empty);
            newBtn = newState.ToCharArray();
            newBool = new bool[10];
            for (int i = 0; i < newBtn.Length; i++)
            {
                if (newBtn[i] == 'A') { newBool[0] = true; }
                if (newBtn[i] == 'B') { newBool[1] = true; }
                if (newBtn[i] == 'X') { newBool[2] = true; }
                if (newBtn[i] == 'Y') { newBool[3] = true; }
                if (newBtn[i] == 'U') { newBool[4] = true; }
                if (newBtn[i] == 'D') { newBool[5] = true; }
                if (newBtn[i] == 'L') { newBool[6] = true; }
                if (newBtn[i] == 'R') { newBool[7] = true; }
                if (newBtn[i] == 'l') { newBool[8] = true; }
                if (newBtn[i] == 'r') { newBool[9] = true; }
            }            
            if (oldBool[0] == true && newBool[0] != true) { SoundController.BuzzerStop(); }
            if (oldBool[0] != true && newBool[0] == true) { SoundController.BuzzerStart(); }
            if (oldBool[1] != true && newBool[1] == true) { }
            if (oldBool[2] != true && newBool[2] == true) { }
            if (oldBool[3] != true && newBool[3] == true) { }
            if (oldBool[4] != true && newBool[4] == true) { }
            if (RefTimeEnabled)            
                if (oldBool[5] != true && newBool[5] == true) { MainWindow.RefTime(); }
            if (GameKeeper.TeamTimeEnabled)
            {
                if (TeamTimeEnabled)
                {
                    if (oldBool[6] != true && newBool[6] == true) { MainWindow.TeamTimeBlack(); }
                    if (oldBool[7] != true && newBool[7] == true) { MainWindow.TeamTimeWhite(); }
                }                
            }
            if (GoalAwardEnabled)
            {
                if (oldBool[8] != true && newBool[8] == true) { MainWindow.GoalBlack(); }
                if (oldBool[9] != true && newBool[9] == true) { MainWindow.GoalWhite(); }
            }            
            Thread.Sleep(10);
            oldBool = newBool;
            
        }        
    }
}
