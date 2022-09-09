using System;

namespace UnderwaterHockeyTimer
{
    public static class GameKeeper
    {
        public static int Seconds { get; set; }
        public static int Minutes { get; set; }
        public static int GoalsWhite { get; set; }
        public static int GoalsBlack { get; set; }
        public static int CatchUpTime { get; set; }
        public static int GameLength { get; set; }        
        public static int GameHalves { get; set; }
        public static int GameHalfTimeLength { get; set; }
        public static int TimeBetweenGamesMax { get; set; }
        public static int TimeBetweenGamesMin { get; set; }
        public static int GameNo { get; set; } = 1;
        public static int GameNoIncrement { get; set; }
        public static DateTime FirstGame { get; set; }
        public static bool SuddenDeath { get; set; } = false;
        public static bool StartFirstGame { get; set; } = false;
        public static bool RefTimeEnabled { get; set; } = false;
        public static bool TeamTimeTakenBlack { get; set; } = false;
        public static bool TeamTimeTakenWhite { get; set; } = false;
        public static bool TeamTimeTaken { get; set; } = false;
        public static bool TeamTimeEnabled { get; set; } = false;
        public static bool SettingsSet { get; set; } = false;
        public static bool SetSecondGUI { get; set; } = false;        
        public static void ResetGoals()
        {
            GoalsBlack = 0;
            GoalsWhite = 0;            
        }
        public static void ResetTeamTimeTaken()
        {
            TeamTimeTakenBlack = false;
            TeamTimeTakenWhite = false;
        }
    }
}
