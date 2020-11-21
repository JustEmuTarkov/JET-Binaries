using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;

namespace JET.Launcher.Utilities
{
    internal class FileManager
    {
        private List<string> namesToFind = new List<string>()
        {
            "server.exe"
        };
        private List<string> foldersToFind = new List<string>()
        {
            "jet",
            "justemutarkov",
            "server"
        };
        internal string ScanToConfirmDirectory(string directory)
        {
            string[] filesInCurrentDirectory = Directory.GetFiles(directory);
            for (int i = 0; i < filesInCurrentDirectory.Length; i++)
            {
                string folderName = filesInCurrentDirectory[i].ToLower();
                for (int checkNo = 0; checkNo < namesToFind.Count; checkNo++)
                {
                    if (folderName.Contains(namesToFind[checkNo]))
                        return directory;
                }
            }
            for (int i = 0; i < filesInCurrentDirectory.Length; i++)
            {
                string folderName = filesInCurrentDirectory[i].ToLower();
                for (int checkNo = 0; checkNo < foldersToFind.Count; checkNo++)
                {
                    if (folderName.Contains(foldersToFind[checkNo]))
                    {
                        if (Directory.Exists(Path.Combine(directory, folderName)))
                            return ScanToConfirmDirectory(Path.Combine(directory, folderName));
                    }

                }
            }
            return "Not found";
        }
        internal string CheckIfFileExistReturnDirectory(string directory)
        {
            string[] filesInCurrentDirectory = Directory.GetFiles(directory);
            for (int i = 0; i < filesInCurrentDirectory.Length; i++)
            {
                for (int fileId = 0; i < namesToFind.Count; fileId++) {
                    if (filesInCurrentDirectory[i] == namesToFind[fileId])
                        return directory;
                }
            }
            return "Not found";
        }
        internal void FindServerDirectory(string initialDirectory = "")
        {
            if (initialDirectory != "")
            {
                Global.ServerLocation = ScanToConfirmDirectory(initialDirectory);
                if (Global.ServerLocation != "Not found") {
                    return;
                }
            }
            string LauncherDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            // all lower case cause we lowercase() all names of files
            //check if folder exist in current directory
            Global.ServerLocation = ScanToConfirmDirectory(LauncherDirectory);

            if (Global.ServerLocation == "Not found")
            {
                // if not found search 1 folder up
                LauncherDirectory = Path.GetFullPath(Path.Combine(LauncherDirectory, @"..\"));
                Global.ServerLocation = ScanToConfirmDirectory(LauncherDirectory);
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
                    Global.ServerLocation = ScanToConfirmDirectory(Global.ServerLocation);
                }
                // hangs start untill user specify server location
            }
        }
    }
}
