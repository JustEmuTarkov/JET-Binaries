using System;
using System.IO;
using Microsoft.VisualBasic;

namespace JET.Launcher.Utilities
{
    internal class FileManager
    {
        internal string ScanToConfirmDirectory(string directory, string[] namesToFind, string[] foldersToFind)
        {
            string[] filesInCurrentDirectory = Directory.GetFiles(directory);
            for (int i = 0; i < filesInCurrentDirectory.Length; i++)
            {
                string folderName = filesInCurrentDirectory[i].ToLower();
                for (int checkNo = 0; checkNo < namesToFind.Length; checkNo++)
                {
                    if (folderName.Contains(namesToFind[checkNo]))
                        return directory;
                }
                for (int checkNo = 0; checkNo < namesToFind.Length; checkNo++)
                {
                    if (folderName.Contains(namesToFind[checkNo]) && !folderName.Contains(".exe"))
                        return directory;
                }
            }
            return "Not found";
        }
        internal string CheckIfFileExistReturnDirectory(string directory, string nameToConfirm)
        {
            string[] filesInCurrentDirectory = Directory.GetFiles(directory);
            for (int i = 0; i < filesInCurrentDirectory.Length; i++)
            {
                if (filesInCurrentDirectory[i] == nameToConfirm)
                    return directory;
            }
            return "Not found";
        }
        internal void FindServerDirectory()
        {
            string LauncherDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            // all lower case cause we lowercase() all names of files
            string[] filesToMatch = new string[1] {
                "server.exe"
            };
            string[] foldersToMatch = new string[2] {
                "jet",
                "justemutarkov"
            };
            //check if folder exist in current directory
            Global.ServerLocation = ScanToConfirmDirectory(LauncherDirectory, filesToMatch, foldersToMatch);

            if (Global.ServerLocation == "Not found")
            {
                // if not found search 1 folder up
                LauncherDirectory = Path.GetFullPath(Path.Combine(LauncherDirectory, @"..\"));
                Global.ServerLocation = ScanToConfirmDirectory(LauncherDirectory, filesToMatch, foldersToMatch);
            }

            if (Global.ServerLocation == "Not found")
            {
                while (Global.ServerLocation == "Not found")
                {
                    Global.ServerLocation = Interaction.InputBox("Type windows location where server is located.", "I was unable to find server *_*", @"F:\Pulpit\JET\JustEmuTarkov-Server-1.0.1");
                    if (Global.ServerLocation == "")
                    {
                        Global.ServerLocation = "Not found";
                        continue;
                    }
                    Global.ServerLocation = ScanToConfirmDirectory(Global.ServerLocation, filesToMatch, foldersToMatch);
                }
                // hangs start untill user specify server location
            }
        }
    }
}
