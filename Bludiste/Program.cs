using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace maze
{
    class Program
    {
        public static int DetekceMapaX(int x)
        {
            if (x >= 79) x = 78;
            if (x < 0) x = 0;
            return x;
        }

        public static int DetekceMapaY(int x, int y)
        {
            if (y < 0) y = 0;
            if (y > 23) y = 23;
            return y;
        }

        public static int DetekceCile(int x, int y, int lvl, DateTime start, char znak)
        {
            DateTime konec;
            konec = DateTime.Now;
            Console.Clear();
            Console.SetCursorPosition(35, 11);
            Console.WriteLine("Level dokončen!");
            Console.SetCursorPosition(28, 13);
            Console.WriteLine("Váš čas byl: {0}", konec - start);
            lvl++;
            Console.ReadKey();
            Console.Clear();
            return lvl;
        }

        public static void Main(string[] args)
        {
            //startovní pozice "Panáčka"
            int lvl = 1;
            int x = 41;
            int y = 22;
            ConsoleKeyInfo klavesa;
            Console.SetWindowSize(80, 25);
            Console.CursorVisible = false;
            
            //znaky reprezentující zeď, postavu hráče a cíl
            char zed = '#';
            char hrac = 'O';
            char cil = 'C';

            //  Úvodní zpráva
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(33, 9);
            Console.WriteLine("Bludiště v1.0.0");
            Console.SetCursorPosition(29, 10);
            Console.WriteLine("Vytvořil: Shorty & Dave");
            Console.SetCursorPosition(27, 12);
            Console.WriteLine("Pro pohyb používejte šipky");
            Console.SetCursorPosition(14, 13);
            Console.Write("Vašim cílem je dostat se na konec bludiště označen jako: \"{0}\"", cil);
            Console.SetCursorPosition(17, 15);
            Console.Write("Pro pokračování stiskněte libovolnou klávesu...");
            Console.ReadKey();
            Console.Clear();

            while (true)
            {

                //Začátek levelu
                Console.SetCursorPosition(17, 12);
                if (lvl == 1)
                {
                    Console.WriteLine("Až budeš připraven stiskni libovolnou klávesu ...");
                    Console.ReadKey();
                }

                DateTime start;
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                var map = new char[79, 25]; // Nastavení 2D pole s maximálními rozměry konzole

                //Načtení a vypsání mapy
                try
                {   //Zkusí načíst další level
                    StreamReader level = new StreamReader("level" + lvl + ".txt");
                    Console.SetCursorPosition(29, 12);
                    Console.WriteLine("Začína level číslo {0}", lvl);
                    Console.ReadKey();
                    Console.Clear();
                    string line;
                    var lineCount = 0;

                    while ((line = level.ReadLine()) != null)
                    {
                        if (lineCount < 26)
                        {
                            for (int i = 0; i < 79 && i < line.Length; i++)
                            {
                                map[i, lineCount] = line[i];

                                switch (map[i, lineCount].ToString())
                                {
                                    case "#":
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                        Console.Write(map[i, lineCount]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case "c":
                                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                                        map[i, lineCount] = 'C';
                                        Console.Write(map[i, lineCount]);
                                        break;
                                    case "C":
                                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                                        Console.Write(map[i, lineCount]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        break;
                                    case ("S"):             // Políčko obsahující "S/s" nastaví jako start a nahradí mezerou
                                        Console.Write(" ");
                                        x = i;
                                        y = lineCount;
                                        break;
                                    case ("s"):
                                        Console.Write(" ");
                                        x = i;
                                        y = lineCount;
                                        break;
                                    default:
                                        Console.Write(map[i, lineCount]);
                                        break;
                                }
                            }
                        }
                        Console.WriteLine();
                        lineCount++;
                    };
                }
                catch
                {   // Pokud mapa další neexistuje program vypíše příslušnou hlášku a ukončí se
                    Console.SetCursorPosition(25, 12);
                    if (lvl != 1)
                    {
                        Console.SetCursorPosition(14, 12);
                        Console.WriteLine("Úspěšně jste překonal/a všechny levely, Gratulujeme!");
                    }
                    else
                    {
                        Console.WriteLine("Nepodařilo se načíst úroveň.");
                    }
                    Console.ReadKey();
                    Environment.Exit(0);    // Ukončí konzoli
                };

                start = DateTime.Now;
                // Samotná hra > pohyb + detekce zdí
                while (true)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(hrac);
                    Console.SetCursorPosition(x, y);
                    klavesa = Console.ReadKey();

                    if (klavesa.Key == ConsoleKey.RightArrow)
                    {
                        x++;
                        x = DetekceMapaX(x);        // Detekce konce "mapy"
                        if (map[x, y] == zed) x--;  // Detekce zdi

                        if (map[x, y] == cil)
                        {
                            lvl = DetekceCile(x, y, lvl, start, map[x, y]);
                            break;
                        };
                    }

                    // Pohyb doleva
                    if (klavesa.Key == ConsoleKey.LeftArrow)
                    {
                        x--;
                        x = DetekceMapaX(x);        // Detekce konce mapy
                        if (map[x, y] == zed) x++;  // Detekce zdi

                        if (map[x, y] == cil)
                        {
                            lvl = DetekceCile(x, y, lvl, start, map[x, y]);
                            break;
                        };
                    }

                    // Pohyd dolu
                    if (klavesa.Key == ConsoleKey.DownArrow)
                    {
                        y++;
                        if (map[x, y] == zed) y--;  // Detekce zdi
                        y = DetekceMapaY(x, y);     // Detekce konce "mapy"

                        if (map[x, y] == cil)
                        {
                            lvl = DetekceCile(x, y, lvl, start, map[x, y]);
                            break;
                        };
                    }

                    // Pohyb nahoru
                    if (klavesa.Key == ConsoleKey.UpArrow)
                    {
                        y--;

                        //Detekce zdi
                        if (x > -1 && y > -1) if (map[x, y] == zed) y++;    // Detekce zdi
                        y = DetekceMapaY(x, y);                             // Detekce konce "mapy"
                        if (map[x, y] == cil)                               // Detekce cíle
                        {
                            lvl = DetekceCile(x, y, lvl, start, map[x, y]);
                            break;
                        };
                    }

                    // Vypsání "postavičky"
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

    }
}