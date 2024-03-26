using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Graphics.TaskBar
{
    public class SortMenuItems
    {
        public static List<Menu_Items> SortMenuItemsAlphabetically(List<Menu_Items> items)
        {
            // Implement bubble sort for sorting alphabetically
            int n = items.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (CompareNames(items[j].Name, items[j + 1].Name) > 0)
                    {
                        Menu_Items temp = items[j];
                        items[j] = items[j + 1];
                        items[j + 1] = temp;
                    }
                }
            }

            // Group items by start letter
            List<Menu_Items> sortedItemsWithStartLetters = new List<Menu_Items>();
            char currentStartLetter = '\0';
            foreach (var item in items)
            {
                char firstLetter = item.Name[0];
                if (firstLetter != currentStartLetter)
                {
                    currentStartLetter = firstLetter;
                    sortedItemsWithStartLetters.Add(new Menu_Items
                    {
                        Name = currentStartLetter.ToString(), // Add start letter
                        Source = "", // You can set other properties as needed
                        // Add other properties accordingly
                    });
                }
                sortedItemsWithStartLetters.Add(item);
            }

            return sortedItemsWithStartLetters;
        }

        // Compare two strings lexicographically
        public static int CompareNames(string str1, string str2)
        {
            int minLength = Math.Min(str1.Length, str2.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (str1[i] != str2[i])
                {
                    return str1[i].CompareTo(str2[i]);
                }
            }
            return str1.Length.CompareTo(str2.Length);
        }
    }
}
