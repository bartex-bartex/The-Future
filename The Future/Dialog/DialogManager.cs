using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace The_Future
{
    public static class DialogManager
    {
        public static void DisplayDialog(string path, DialogBox dialogBox, int dialogNumber)
        {
            StreamReader sr = new StreamReader(path);
            string[] dialogs = sr.ReadToEnd().Split('\n');

            
            if(dialogNumber > 0)
            { 
                dialogBox.Initialize(dialogs, dialogNumber);
            }
            else
            {
                dialogBox.Initialize(dialogs);
            }

        }
    }
}
