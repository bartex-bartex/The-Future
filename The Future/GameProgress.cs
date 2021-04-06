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

        static GameProgress()
        {
            AreDialogsActive = new bool[5];
            Doors = new EDoor[5];
            AreTeleportsActive = new bool[5];

            AreDialogsActive.Select(x => x = true);
            Doors.Select(x => x = EDoor.Close);
            AreTeleportsActive.Select(x => x = false);

            for (int i = 0; i < AreDialogsActive.Length; i++) { AreDialogsActive[i] = true; }
            for (int i = 0; i < Doors.Length; i++) { Doors[i] = EDoor.Close; }
            for (int i = 0; i < AreTeleportsActive.Length; i++) { AreTeleportsActive[i] = false; }

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
                    AreTeleportsActive[0] = true; //Should be set automaticaly with door
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
