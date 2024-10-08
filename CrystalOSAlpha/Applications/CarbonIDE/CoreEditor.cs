﻿using Cosmos.System;
using CrystalOSAlpha.System32;
using System;
using System.Linq;
using System.Text;

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
                    if (KeyboardManager.AltPressed)
                    {
                        // Move the line up
                        string temp = lines[lineIndex - 1];
                        lines[lineIndex - 1] = lines[lineIndex];
                        lines[lineIndex] = temp;
                        lineIndex--; // Update line index to reflect the moved line's new position
                    }
                    else
                    {
                        if (lineIndex > 0)
                        {
                            lineIndex--;
                            cursorIndex = Math.Min(cursorIndex, lines[lineIndex].Length);
                        }
                    }
                    break;
                case ConsoleKeyEx.DownArrow:
                    if(KeyboardManager.AltPressed)
                    {
                        // Move the line down
                        string temp = lines[lineIndex + 1];
                        lines[lineIndex + 1] = lines[lineIndex];
                        lines[lineIndex] = temp;
                        lineIndex++; // Update line index to reflect the moved line's new position
                    }
                    else
                    {
                        if (lineIndex < lines.Length - 1)
                        {
                            lineIndex++;
                            cursorIndex = Math.Min(cursorIndex, lines[lineIndex].Length);
                        }
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
                    if (KeyboardManager.ControlPressed)
                    {
                        if(keyInfo.Key == ConsoleKeyEx.D)
                        {
                            string duplicatedLine = lines[lineIndex];
                            lines = lines.Take(lineIndex + 1).Concat(new[] { duplicatedLine }).Concat(lines.Skip(lineIndex + 1)).ToArray();
                            lineIndex++; // Move to the duplicated line
                        }
                    }
                    else
                    {
                        string temp = Keyboard.HandleKeyboard("", keyInfo);
                        lines[lineIndex] = lines[lineIndex].Insert(cursorIndex, temp);
                        //cursorIndex++;
                        cursorIndex += temp.Length;
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

        public static (string, string, int, int) Update(string text, string textWithCursor, int cursorIndex, int lineIndex)
        {
            string[] lines = text.Split('\n');

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
