using System.Media;

namespace UnderwaterHockeyTimer
{
    public static class SoundController
    {
        private static SoundPlayer dingSound = new SoundPlayer(@"C:\Windows\Media\Windows Background.wav");
        private static SoundPlayer buzzerSound = new SoundPlayer();          
        private static bool buzzerSet = false;
        public static void DefaultDing()
        {
            dingSound.Play();
        }
        public static void BuzzerStart()
        {
            if (buzzerSet == true)
            {
                buzzerSound.PlayLooping();
            }            
        }
        public static void BuzzerStop()
        {
            if (buzzerSet == true)
            {
                buzzerSound.Stop();
            }
        }  
        public static void SetBuzzer(string location)
        {
            buzzerSound.SoundLocation = location;
            buzzerSet = true;
        }
    }
}
