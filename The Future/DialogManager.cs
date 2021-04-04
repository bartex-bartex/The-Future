using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace The_Future
{
    public static class DialogManager
    {
        public static void DisplayDialog(string path, DialogBox dialogBox)
        {
            StreamReader sr = new StreamReader(path);
            string[] dialogs = sr.ReadToEnd().Split('\n');

            dialogBox.Initialize(dialogs);
        }
    }
}
