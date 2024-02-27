using CrystalOS_Alpha;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;

class GenerateMenubar
{
    public MenuBar Generate(string input)
    {
        //string input = "#Menubar\n{\n\"File\":\n\"New\":\n//CodeNew\n.\n\"Open\":\n//CodeOpen\n.\n---\n\"Help\":\n\"Getting Started\":\n//Code\n.\n}";

        List<string> menus = new List<string>();
        List<Submenu> submenus = new List<Submenu>();

        string[] Lines = input.Split('\n');
        List<string> Modified = new List<string>();
        for (int i = 0; i < Lines.Length; i++)
        {
            if (i > 2 && i < Lines.Length - 1)
            {
                Modified.Add(Lines[i].Trim());
            }
        }

        string PutTogether = string.Empty;
        foreach (string s in Modified)
        {
            PutTogether += s + "\n";
        }
        PutTogether = PutTogether.Remove(PutTogether.Length - 1);

        string[] Chunks = PutTogether.Split("---");

        //Remove trailing and leading \n chars
        for (int i = 0; i < Chunks.Length; i++)
        {
            if (Chunks[i].StartsWith("\n"))
            {
                Chunks[i] = Chunks[i].Remove(0, 1);
            }
            if (Chunks[i].EndsWith("\n"))
            {
                Chunks[i] = Chunks[i].Remove(Chunks[i].Length - 1);
            }
        }

        for (int i = 0; i < Chunks.Length; i++)
        {
            string[] splittedLines = Chunks[i].Split('\n');
            string NameOfMenuItem = "";
            List<string> SubmenuItems = new List<string>();
            List<string> Codes = new List<string>();
            string TempCode = "";
            for (int j = 0; j < splittedLines.Length; j++)
            {
                if (j == 0)
                {
                    NameOfMenuItem = splittedLines[j].Remove(splittedLines[j].Length - 2).Remove(0, 1);
                }
                else
                {
                    if (splittedLines[j].StartsWith("\"") && splittedLines[j].EndsWith(":"))
                    {
                        SubmenuItems.Add(splittedLines[j].Remove(splittedLines[j].Length - 2).Remove(0, 1));
                    }
                    else if (splittedLines[j] == ".")
                    {
                        Codes.Add(TempCode);
                        TempCode = "";
                    }
                    else
                    {
                        TempCode += splittedLines[j] + "\n";
                    }
                }
            }
            menus.Add(NameOfMenuItem);
            List<Items> items = new List<Items>();
            for (int j = 0; j < SubmenuItems.Count; j++)
            {
                items.Add(new Items(SubmenuItems[j], Codes[j]));
            }
            submenus.Add(new Submenu(NameOfMenuItem, items));
        }
        return new MenuBar(menus, submenus);
    }
}