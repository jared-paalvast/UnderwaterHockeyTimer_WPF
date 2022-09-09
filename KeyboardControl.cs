using System;
using System.Windows.Threading;
using System.Windows.Input;

namespace UnderwaterHockeyTimer
{
    public class KeyboardControl
    {
        private static bool keyWasDown = false;
        private static DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);
        public KeyboardControl()
        {
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {            
            if (keyWasDown == true)
            {
                if (!Keyboard.IsKeyDown(Key.F8))
                {
                    SoundController.BuzzerStop();
                }
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.F8))
                {
                    SoundController.BuzzerStart();
                }
            }
            keyWasDown = Keyboard.IsKeyDown(Key.F8);
        }
    }
}
