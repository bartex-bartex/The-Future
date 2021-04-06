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
        public static void DisplayDialog(string path, DialogBox dialogBox)
        {
            StreamReader sr = new StreamReader(path);
            string[] dialogs = sr.ReadToEnd().Split('\n');

            bool IsSucced = int.TryParse(Regex.Match(path, "[0-9]+").Value, out int dialogNumber);

            if (IsSucced == true)
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
