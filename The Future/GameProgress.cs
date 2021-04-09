using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace The_Future
{
    public enum EDoor
    {
        Open,
        Close
    }

    public static class GameProgress
    {
        public static bool[] AreDialogsActive { get; set; }
        public static EDoor[] Doors { get; set; }
        public static bool[] AreTeleportsActive { get; set; }
        public static string[] TerminalCodes { get; set; }
        public static bool[] AreTerminalActive { get; set; }
        public static string[] PathsToExercises { get; set; }

        static GameProgress()
        {
            AreDialogsActive = new bool[5];
            Doors = new EDoor[5];
            AreTeleportsActive = new bool[5];
            TerminalCodes = new string[5];
            AreTerminalActive = new bool[5];
            PathsToExercises = new string[5];

            AreDialogsActive.Select(x => x = true);
            Doors.Select(x => x = EDoor.Close);
            AreTeleportsActive.Select(x => x = false);

            for (int i = 0; i < AreDialogsActive.Length; i++) { AreDialogsActive[i] = true; }
            for (int i = 0; i < Doors.Length; i++) { Doors[i] = EDoor.Close; }
            for (int i = 0; i < AreTeleportsActive.Length; i++) { AreTeleportsActive[i] = false; }
            for (int i = 0; i < AreTerminalActive.Length; i++) { AreTerminalActive[i] = true; }

            TerminalCodes[0] = "1234";
            //PathsToExercises[1] = "AtlasA";

            PathsToExercises[0] = "1";
            PathsToExercises[1] = "2";
            PathsToExercises[2] = "3";
            PathsToExercises[3] = "4";
        }

        //public static void SetDoorsValue()
        //{

        //}

        public static void SetDialogsValue(int dialogNumber)
        {
            
            switch (dialogNumber)
            {
                case 1:
                    AreDialogsActive[dialogNumber - 1] = false;
                    Doors[0] = EDoor.Open;
                    break;

                case 2:
                    AreDialogsActive[dialogNumber - 1] = false;
                    Doors[1] = EDoor.Open;
                    AreTeleportsActive[0] = true;
                    AreTeleportsActive[1] = true;
                    Doors[2] = EDoor.Open;
                    break;

                default:
                    break;
            }
        }

        public static void SetTerminalValues(int terminalNumber)
        {
            switch (terminalNumber)
            {
                case 0:
                    Doors[3] = EDoor.Open;
                    AreTeleportsActive[2] = true;
                    AreTerminalActive[terminalNumber] = false;
                    GameMain.IsPlayerMovementBlock = false;
                    break;

                default:
                    break;
            }
        }

        //public static void SetTeleportsValue()
        //{

        //}
    }
}
