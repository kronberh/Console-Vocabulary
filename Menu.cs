using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using n_Vocabulary;

namespace n_Menu
{
    internal interface IMenu
    {
        void MainMenu();
        int PrintMenu(string nav, string title, bool searchBarAvailable, List<string> menuItems);
        void PrintInMiddle(params string[] strings);
    }
    internal abstract class Menu : IMenu
    {
        public abstract void MainMenu();
        public virtual int PrintMenu(string nav, string title, List<string> menuItems) => PrintMenu(nav, title, false, menuItems.Select(item => (item, true)).ToList());
        public virtual int PrintMenu(string nav, string title, bool searchBarAvailable, List<string> menuItems) =>  PrintMenu(nav, title, searchBarAvailable, menuItems.Select(item => (item, true)).ToList());
        public virtual int PrintMenu(string nav, string title, List<(string, bool)> menuItems) => PrintMenu(nav, title, false, menuItems);
        public virtual int PrintMenu(string nav, string title, bool searchBarAvailable, List<(string, bool)> menuItems)
        {
            int currentSelection = 0;
            foreach (var item in menuItems)
            {
                if (item.Item2)
                {
                    break;
                }
                currentSelection++;
            }
            ConsoleKeyInfo keyinfo;
            StringBuilder desiredOption = new();
            string tempNav = nav;
            do
            {
                Console.Clear();
                Console.Write("\x1b[3J");
                while (tempNav.Length > Console.WindowWidth)
                {
                    string[] nav_arr = tempNav.Split(' ').Skip(1).ToArray();
                    if (nav_arr.Length <= 1)
                    {
                        break;
                    }
                    nav_arr[0] = "...";
                    tempNav = string.Join(" ", nav_arr);
                }
                Console.WriteLine($"\x1b[34m{tempNav}\x1b[0m");
                Console.WriteLine($"   \x1b[1m{title}\x1b[0m\n");
                if (searchBarAvailable)
                {
                    if (currentSelection == -1)
                    {
                        Console.WriteLine($"\x1b[3;96m-> [Search: {desiredOption}|]\x1b[0m");
                    }
                    else
                    {
                        Console.WriteLine($" \x1b[3;36m[Search: {desiredOption}]\x1b[0m");
                    }
                }
                for (int i = 0; i < menuItems.Count; i++)
                {
                    if (IsContaining(menuItems[i].Item1, desiredOption.ToString()))
                    {
                        if (i == currentSelection)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("-> " + menuItems[i].Item1);
                            Console.ResetColor();
                        }
                        else if (!menuItems[i].Item2)
                        {
                            Console.WriteLine("  \x1b[9;90m" + menuItems[i].Item1 + "\x1b[0m");
                        }
                        else
                        {
                            Console.WriteLine("  " + menuItems[i].Item1);
                        }
                    }
                }
                keyinfo = Console.ReadKey(true);
                if (currentSelection == -1)
                {
                    switch (keyinfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            currentSelection = menuItems.Count;
                            do
                            {
                                currentSelection = (currentSelection == 0) ? -1 : ((currentSelection - 1 + menuItems.Count) % menuItems.Count);
                            } while ((currentSelection != -1 && !IsContaining(menuItems[currentSelection].Item1, desiredOption.ToString())) || (currentSelection != -1 && !menuItems[currentSelection].Item2));
                            break;
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.Enter:
                            do
                            {
                                currentSelection = (currentSelection == menuItems.Count - 1) ? -1 : ((currentSelection + 1) % menuItems.Count);
                            } while ((currentSelection != -1 && !IsContaining(menuItems[currentSelection].Item1, desiredOption.ToString())) || (currentSelection != -1 && !menuItems[currentSelection].Item2));
                            keyinfo = default;
                            break;
                        case ConsoleKey.Backspace:
                            if (desiredOption.Length > 0)
                            {
                                desiredOption.Remove(desiredOption.Length - 1, 1);
                            }
                            else
                            {
                                do
                                {
                                    currentSelection = (currentSelection == menuItems.Count - 1) ? -1 : ((currentSelection + 1) % menuItems.Count);
                                } while ((currentSelection != -1 && !IsContaining(menuItems[currentSelection].Item1, desiredOption.ToString())) || (currentSelection != -1 && !menuItems[currentSelection].Item2));
                            }
                            break;
                        case ConsoleKey.Escape:
                            return -1;
                        default:
                            desiredOption.Append(keyinfo.KeyChar);
                            break;
                    }
                }
                else
                {
                    switch (keyinfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.W:
                            do
                            {
                                currentSelection = (searchBarAvailable && currentSelection == 0) ? -1 : ((currentSelection - 1 + menuItems.Count) % menuItems.Count);
                            } while ((currentSelection != -1 && !IsContaining(menuItems[currentSelection].Item1, desiredOption.ToString())) || (currentSelection != -1 && !menuItems[currentSelection].Item2));
                            break;
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.S:
                            do
                            {
                                currentSelection = (currentSelection + 1) % menuItems.Count;
                            } while ((currentSelection != -1 && !IsContaining(menuItems[currentSelection].Item1, desiredOption.ToString())) || (currentSelection != -1 && !menuItems[currentSelection].Item2));
                            break;
                        case ConsoleKey.Escape:
                            return -1;
                    }
                }
            } while (keyinfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return currentSelection;
        }
        public void PrintInMiddle(params string[] strings)
        {
            Console.Clear();
            Console.Write("\x1b[3J");
            Console.CursorTop = Math.Max((Console.WindowHeight - strings.Length) / 2, 0);
            foreach (string s in strings)
            {
                Console.CursorLeft = Math.Max((Console.WindowWidth - s.Length) / 2, 0);
                Console.WriteLine(s);
            }
        }
        public static bool IsContaining(string str, params string[] substrs)
        {
            return Array.Exists(substrs, substr => Regex.IsMatch(str, Regex.Escape(substr), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
        }
    }
}