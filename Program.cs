using System;
using System.Collections.Generic;

/// <summary>
/// сложность1 = (2^(3/2)+1)*2*n^2 + n                                   (28 случаев из 64)
/// сложность2 = (2^(3/2)+1)*3*n^2 + ~1,5n                               (36 случаев из 64)
/// в любом случае сложность О(n^2)
/// </summary>

namespace Elephants
{
    class Program
    {
        public enum Field : byte { EMPTY = 0, CAPTURED, FIGURE }

        public static Field[,] cells;

        static void Main(string[] args)
        {
            ushort n = Convert.ToUInt16(Console.ReadLine());///четное
            ushort x = Convert.ToUInt16(Console.ReadLine());
            ushort y = Convert.ToUInt16(Console.ReadLine());
            bool check = false;

            cells = new Field[n, n];
            SetEmptyBoard(n);///n^2

            ushort specularX = x > ((n / 2) - 1) ? //
                (ushort)(Math.Abs(x - (n / 2))) :
                (ushort)(Math.Abs(x - ((n / 2) - 1)));

            ushort specularY = y > ((n / 2) - 1) ? //
                (ushort)(Math.Abs(y - (n / 2))) :
                (ushort)(Math.Abs(y - ((n / 2) - 1)));

            ///(n^2 * 2^(3/2)) + (n^2 * 2^(3/2))
            if (specularX > specularY)                                 //горизонталь
            {
                for (int i = x; i > -1; i -= 2)/// n^2 /2 * 2^(3/2)
                {
                    cells[i, y] = Field.FIGURE;
                    CaptureDiagonals(n, (ushort)i, y);
                }
                for (ushort i = x; i < n; i += 2)/// n^2 /2 * 2^(3/2)
                {
                    cells[i, y] = Field.FIGURE;
                    CaptureDiagonals(n, i, y);
                }
                if (y < 4)
                {
                    for (ushort i = 0; i < n; i++)/// n^2 * 2^(3/2)
                    {
                        if (cells[i, y] == Field.FIGURE)
                        {
                            cells[i, y + 2 * specularY + 1] = Field.FIGURE;
                            CaptureDiagonals(n, i, (ushort)(y + 2 * specularY + 1));
                        }
                    }
                }
                else
                {
                    for (ushort i = 0; i < n; i++)/// n^2 * 2^(3/2)
                    {
                        if (cells[i, y] == Field.FIGURE)
                        {
                            cells[i, y - 2 * specularY - 1] = Field.FIGURE;
                            CaptureDiagonals(n, i, (ushort)(y - 2 * specularY - 1));
                        }
                    }
                }
            }
            else if (specularY >= specularX)                                //вертикаль
            {
                for (int j = y; j > -1; j -= 2)/// n^2 /2 * 2^(3/2)
                {
                    cells[x, j] = Field.FIGURE;
                    CaptureDiagonals(n, x, (ushort)j);
                }
                for (ushort j = y; j < n; j += 2)/// n^2 /2 * 2^(3/2)
                {
                    cells[x, j] = Field.FIGURE;
                    CaptureDiagonals(n, x, j);
                }
                if (x < 4)
                {
                    for (ushort j = 0; j < n; j++)/// n^2 * 2^(3/2)
                    {
                        if (cells[x, j] == Field.FIGURE)
                        {
                            cells[x + 2 * specularX + 1, j] = Field.FIGURE;
                            CaptureDiagonals(n, (ushort)(x + 2 * specularX + 1), j);
                        }
                    }
                }
                else
                {
                    for (ushort j = 0; j < n; j++)/// n^2 * 2^(3/2)
                    {
                        if (cells[x, j] == Field.FIGURE)
                        {
                            cells[x - 2 * specularX - 1, j] = Field.FIGURE;
                            CaptureDiagonals(n, (ushort)(x - 2 * specularX - 1), j);
                        }
                    }
                }
            }


            check = Check(n);/// n^2
            if (check == true)
            {
                Console.WriteLine("Количество слонов:{0}", n);
                GetElephants(n);/// n +- n/2
                Console.ReadKey();
                return;
            }
            else
            {
                ushort thirdLine = 0;
                ushort length = n;
                if (specularX > specularY)                         //горизонталь
                {
                    for (ushort i = 0; i < n; i++)/// n^2
                    {
                        for (ushort j = (ushort)(n / 2); j < n; j++)
                        {
                            if (cells[i, j] == Field.EMPTY)
                                if (j < length)
                                {
                                    length = j;
                                    thirdLine = i;
                                }
                        }
                    }
                    for (ushort j = 0; j < n; j++)/// n^2 * 2^(3/2)
                    {
                        if (cells[thirdLine, j] == Field.EMPTY)
                        {
                            cells[thirdLine, j] = Field.FIGURE;
                            CaptureDiagonals(n, thirdLine, j);
                        }
                    }
                }
                else if (specularY >= specularX)                   //вертикаль
                {
                    length = n;
                    for (ushort j = 0; j < n; j++)/// n^2
                    {
                        for (ushort i = (ushort)(n / 2); i < n; i++)
                        {
                            if (cells[i, j] == Field.EMPTY)
                                if (i < length)
                                {
                                    length = i;
                                    thirdLine = j;
                                }
                        }
                    }
                    for (ushort i = 0; i < n; i++)/// n^2 * 2^(3/2)
                    {
                        if (cells[i, thirdLine] == Field.EMPTY)
                        {
                            cells[i, thirdLine] = Field.FIGURE;
                            CaptureDiagonals(n, i, thirdLine);
                        }
                    }
                }
                check = Check(n);/// n^2
                if (check == true)
                {
                    GetElephants(n);/// n + минимум 1
                    Console.ReadKey();
                    return;
                }
            }
        }

        static void GetElephants(ushort n)
        {
            for (ushort i = 0; i < n; i++)                 ///n^2
            {
                for (ushort j = 0; j < n; j++)
                {
                    if (cells[i, j] == Field.FIGURE)
                    {
                        Console.WriteLine("{0}, {1}", i, j);
                    }
                }
            }
        }

        static void SetEmptyBoard(ushort n)
        {
            for (ushort i = 0; i < n; i++)                 ///n^2
            {
                for (ushort j = 0; i < n; i++)
                {
                    cells[i, j] = Field.EMPTY;
                }
            }
        }

        static void CaptureDiagonals(ushort n, ushort x, ushort y)
        {
            int i = x + 1;
            while ((i < n))
            {
                for (ushort j = (ushort)(y + 1); j < n; j++)
                {
                    if (i < n)
                    {
                        if (cells[i, j] != Field.FIGURE) cells[i, j] = Field.CAPTURED;
                        i++;
                    }
                    else j = n;//i=n
                }
                i = x + 1;
                for (int j = y - 1; j > -1; j--)
                {
                    if (i < n)
                    {
                        if (cells[i, j] != Field.FIGURE) cells[i, j] = Field.CAPTURED;
                        i++;
                    }
                    else j = -1;//i=n
                }
                i = n;
            }

            i = x - 1;
            while (i > -1)
            {
                for (ushort j = (ushort)(y + 1); j < n; j++)
                {
                    if (i > -1)
                    {
                        if (cells[i, j] != Field.FIGURE) cells[i, j] = Field.CAPTURED;
                        i--;
                    }
                    else i = -1;
                }
                i = x - 1;
                for (int j = y - 1; j > -1; j--)
                {
                    if (i > -1)
                    {
                        if (cells[i, j] != Field.FIGURE) cells[i, j] = Field.CAPTURED;
                        i--;
                    }
                    else i = -1;
                }
                i = -1;
            }
        }

        static bool Check(ushort n)
        {
            for (ushort i = 0; i < n; i++)                 ///n^2
            {
                for (ushort j = 0; j < n; j++)
                {
                    if (cells[i, j] == Field.EMPTY)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
