using System;
using System.Diagnostics;
using System.Threading.Tasks;

class MatrixDistance
{
    #region Sequential_Loop

    public static void Sequential_MD(double[,] matA, double[,] result)
    {
        int n = matA.GetLength(1);
        int m = matA.GetLength(0);
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < m; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    result[i, j] += Math.Pow((matA[i, k] - matA[j, k]), 2);
                }

            }
        }
    }
    #endregion

    #region Parallel_Loop
    static void Parallel_MD(double[,] matA, double[,] result)
    {
        int n = matA.GetLength(1);
        int m = matA.GetLength(0);

        Parallel.For(0, m, i =>
        {
            for (int j = 0; j < m; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    result[i, j] += Math.Pow((matA[i, k] - matA[j, k]), 2);
                }

            }
        });
    }
    #endregion

    #region Main
    static void Main(string[] args)
    {

        int rowCount = 1000;
        int colCount = 2000;
        double[,] matrix = InitializeMatrix(rowCount, colCount);
        double[,] result = new double[rowCount, colCount];

        // Sequential loop
        Console.Error.WriteLine("Executing sequential loop...");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Sequential_MD(matrix, result);
        stopwatch.Stop();
        Console.Error.WriteLine("Sequential loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);

        // Reset timer and results matrix. 
        stopwatch.Reset();
        result = new double[rowCount, colCount];

        //Parallel loop
        Console.Error.WriteLine("Executing parallel loop...");
        stopwatch.Start();
        Parallel_MD(matrix, result);
        stopwatch.Stop();
        Console.Error.WriteLine("Parallel loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);

        // Keep the console window open in debug mode.
        Console.Error.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
    #endregion

    #region Helper_Methods
    static double[,] InitializeMatrix(int rows, int cols)
    {
        double[,] matrix = new double[rows, cols];

        Random r = new Random();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = r.Next(100);
            }
        }
        return matrix;
    }

    #endregion
}