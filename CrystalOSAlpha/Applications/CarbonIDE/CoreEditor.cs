using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Applications.CarbonIDE
{
    class CoreEditor
    {
        public static (string, string, int, int) Editor(string text, string textWithCursor, int cursorIndex, int lineIndex, KeyEvent keyInfo)
        {
            string[] lines = text.Split('\n');

            switch (keyInfo.Key)
            {
                case ConsoleKeyEx.LeftArrow:
                    if (cursorIndex > 0)
                    {
                        cursorIndex--;
                    }
                    break;
                case ConsoleKeyEx.RightArrow:
                    if (cursorIndex < lines[lineIndex].Length)
                    {
                        cursorIndex++;
                    }
                    break;
                case ConsoleKeyEx.UpArrow:
                    if (lineIndex > 0)
                    {
                        lineIndex--;
                        cursorIndex = Math.Min(cursorIndex, lines[lineIndex].Length);
                    }
                    break;
                case ConsoleKeyEx.DownArrow:
                    if (lineIndex < lines.Length - 1)
                    {
                        lineIndex++;
                        cursorIndex = Math.Min(cursorIndex, lines[lineIndex].Length);
                    }
                    break;
                case ConsoleKeyEx.Backspace:
                    if (cursorIndex > 0)
                    {
                        lines[lineIndex] = lines[lineIndex].Remove(cursorIndex - 1, 1);
                        cursorIndex = Math.Max(0, cursorIndex - 1);
                    }
                    else if (lineIndex > 0)
                    {
                        int prevLineLength = lines[lineIndex - 1].Length;
                        lines[lineIndex - 1] += lines[lineIndex];
                        lines = lines.Take(lineIndex).Concat(lines.Skip(lineIndex + 1)).ToArray();
                        lineIndex--;
                        cursorIndex = prevLineLength;
                    }
                    break;
                case ConsoleKeyEx.Enter:
                    string currentLine = lines[lineIndex];
                    string newLine = currentLine.Substring(cursorIndex);
                    currentLine = currentLine.Substring(0, cursorIndex);
                    lines[lineIndex] = currentLine;
                    lines = lines.Take(lineIndex + 1).Concat(new[] { newLine }).Concat(lines.Skip(lineIndex + 1)).ToArray();
                    lineIndex++;
                    cursorIndex = 0;
                    break;
                default:
                    if (keyInfo.KeyChar != '\0')
                    {
                        lines[lineIndex] = lines[lineIndex].Insert(cursorIndex, keyInfo.KeyChar.ToString());
                        cursorIndex++;
                    }
                    break;
            }

            text = string.Join("\n", lines);
            StringBuilder sbWithCursor = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == lineIndex)
                {
                    sbWithCursor.AppendLine(lines[i].Insert(cursorIndex, "|"));
                }
                else
                {
                    sbWithCursor.AppendLine(lines[i]);
                }
            }
            textWithCursor = sbWithCursor.ToString().TrimEnd('\n');

            return (text, textWithCursor, cursorIndex, lineIndex);
        }
    }
}
