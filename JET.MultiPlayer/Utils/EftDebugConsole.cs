using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerLib.Utils
{
    public static class EftDebugConsole
    {
        [DllImport("Kernel32.dll")]
        static extern bool AllocConsole();

        private static int NextLine = 0;
        static object IsAllocated = false;
        public static object ConsoleLock = new object();
        public static object ListLock = new object();
        public static List<string> StaticLines = new List<string>();
        public static SemaphoreSlim WriteLock = new SemaphoreSlim(1);


        static EftDebugWriter debugWriter;

        public static void ShowConsole()
        {
            lock (IsAllocated)
            {
                if ((bool) IsAllocated)
                    return;

                AllocConsole();

                debugWriter = new EftDebugWriter(Console.OpenStandardOutput()) {AutoFlush = true};
                Console.SetOut(debugWriter);

                Console.Title = "DO NOT CLOSE! GAME WILL CLOSE IMMEDIATELY!";
                Console.CursorVisible = false;

                Console.BufferHeight = Console.WindowHeight;
                Console.BufferWidth = Console.WindowWidth;

                IsAllocated = true;
            }
        }

        public static void WriteAt(ref int line, string text, params object[] args)
        {
            lock (ListLock)
            {
                var textLine = string.Format(text, args);

                line = line > 0 ? line : ++NextLine;
                int linesMissing = (line + 1) - StaticLines.Count;

                while (linesMissing-- > 0)
                    StaticLines.Add(string.Empty);

                StaticLines[line] = textLine;
            }

            debugWriter.RenderLines();
        }
    }

    public class EftDebugWriter : StreamWriter
    {
        struct Pos
        {
            public int Left, Top;

            public Pos(int x, int y)
            {
                Left = x;
                Top = y;
            }
        }

        bool rendering = false;
        string clearLine = string.Empty;
        string dividerLine = string.Empty;

        public EftDebugWriter(Stream stream)
            : base(stream)
        {
        }

        StringBuilder sb = new StringBuilder();

        public void RenderLines()
        {
            if (rendering)
                return;

            EftDebugConsole.WriteLock.Wait();
            rendering = true;

            var window = new Pos(0, Console.WindowTop);
            var cursor = new Pos(Console.CursorLeft, Console.CursorTop);

            string[] staticLines;

            lock (EftDebugConsole.ListLock)
            {
                staticLines = EftDebugConsole.StaticLines.ToArray();
            }

            int writeTop = window.Top;

            if (dividerLine.Length != Console.BufferWidth)
                dividerLine = new string('=', Console.BufferWidth);

            Console.SetCursorPosition(0, writeTop);

            for (int line = 0; line < staticLines.Length; ++line)
            {
                var lineStr = staticLines[line];
                sb.Append(lineStr + new string(' ', Console.BufferWidth - lineStr.Length));
            }

            sb.AppendLine(dividerLine);
            base.Write(sb.ToString());

            sb.Clear();

            Console.SetCursorPosition(cursor.Left, cursor.Top);

            rendering = false;
            EftDebugConsole.WriteLock.Release();
        }

        public override void Write(char value)
        {
            EftDebugConsole.WriteLock.Wait();

            base.Write(value);
            RenderLines();

            EftDebugConsole.WriteLock.Release();
        }
    }
}
