using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Math4
{
    class Program
    {
        static int number;
        static int n;
        static double determinant = 1;
        static void Main(string[] args)
        {
            FileStream file1 = new FileStream(@"C:\Users\User\source\repos\Gauss\input.txt", FileMode.Open); //создаем файловый поток           
            double[] mas;
            double[][] a;
            double[] b;

            //Считываем массив из файла
            using (var file = new StreamReader(file1))
            {
                n = int.Parse(file.ReadLine());
                number = int.Parse(file.ReadLine());
                a = new double[n][];
                mas = new double[n];
                for (int i = 0; i < n; i++)
                {
                    a[i] = new double[n];
                    for (int j = 0; j < n; j++)
                    {
                        if (i == j)
                        {
                            a[i][j] = number;
                            mas[i] = number;
                        }
                        else
                        {
                            a[i][j] = number * 0.01;
                        }
                    }
                    number++;
                }

                b = new double[n];
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        sum += a[i][j] * mas[j];
                    }
                    b[i] = sum;
                }
            }
            PrintTwo(a);
            PrintOne(b);

            double[] x = MethodGauss(a, b, 1);
            PrintOne(x);
            ReverseMatrix(a);

        }
        public static double[] MethodGauss(double[][] aFirst, double[] b, int f)
        {
            double[][] a = new double[n][];
            for (int i = 0; i < n; i++)
            {
                a[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    a[i][j] = aFirst[i][j];
                }
            }

            int y = 0;
            //Прямой ход
            for (int i = 0; i < n; i++)
            {
                if (a[i][i] == 0)
                {
                    y++;
                    int indexRows = FindMainInRows(a, i);
                    if (indexRows == -1)
                    {
                        break;
                    }
                    SwapRows(a, i, indexRows);
                    double tmp = b[i];
                    b[i] = b[indexRows];
                    b[indexRows] = tmp;
                }
                double mainElement = a[i][i];
                determinant *= mainElement;
                for (int j = 0; j < n; j++)
                {
                    a[i][j] /= mainElement;
                }
                b[i] /= mainElement;
                NextInter(a, i, b);
            }
            if (f == 1)
            {
                determinant *= Math.Pow(-1, y);
                Console.WriteLine("Определитель = " + (determinant));
            }
            double[] x = new double[n];
            //Обратный ход
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = b[i];
                for (int j = i + 1; j < n; j++)
                {
                    x[i] -= x[j] * a[i][j];
                }
            }
            return x;
        }
        public static void PrintTwo(double[][] a)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(a[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        public static void PrintOne(double[] a)
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(a[i]);
            }
        }
        public static int FindMainInRows(double[][] a, int index)
        {
            if (index + 1 < n)
            {
                int indexMax = index + 1;
                double maxElementAbs = Math.Abs(a[indexMax][index]);
                for (int j = index + 1; j < n; j++)
                {
                    if (Math.Abs(a[j][index]) > maxElementAbs)
                    {
                        maxElementAbs = Math.Abs(a[j][index]);
                        indexMax = j;
                    }
                }
                return indexMax;
            }
            else
                return -1;
        }
        public static void SwapRows(double[][] a, int i, int j)
        {
            double[] temp = a[i];
            a[i] = a[j];
            a[j] = temp;
        }
        public static void NextInter(double[][] a, int i, double[] b)
        {
            for (int l = i + 1; l < n; l++)
            {
                double d = a[l][i];
                for (int j = i; j < n; j++)
                {
                    a[l][j] = a[l][j] - a[i][j] * d;
                }
                b[l] = b[l] - b[i] * d;
            }
        }
        public static void ReverseMatrix(double[][] a)
        {
            double[][] aReverse = new double[n][];
            double[] b = new double[n];
            double[][] e = new double[n][];
            for (int i = 0; i < n; i++)
            {
                b[i] = 0;
                aReverse[i] = new double[n];
                e[i] = new double[n];
            }
            double[] helper = new double[n];
            for (int i = 0; i < n; i++)
            {
                b[i] = 1;
                helper = MethodGauss(a, b, 0);
                for (int j = 0; j < n; j++)
                {
                    aReverse[j][i] = helper[j];
                }
                b[i] = 0;
            }
            //PrintTwo(aReverse);
            Console.WriteLine("Проверка обратной матрицы ");
            double sum;
            int k = 0;
            for (int i = 0; i < n; i++)
            {
                sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += a[i][j] * aReverse[j][i];
                }
                e[i][k] = sum;
                if (k < 8)
                    k++;
                else
                    k = 0;
            }
            PrintTwo(e);
        }
    }
}

