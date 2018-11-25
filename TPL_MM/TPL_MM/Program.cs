using System;
using System.Diagnostics;
using System.Threading.Tasks;

class TPL_MatrixMultip
{
    #region Sequential_Loop
    static void Sequential_MM(double[,] matA, double[,] matB,
                                            double[,] result)
    {
        int matACols = matA.GetLength(1);
        int matARows = matA.GetLength(0);
        int matBCols = matB.GetLength(1);

        for (int i = 0; i < matARows; i++)
        {
            for (int j = 0; j < matBCols; j++)
            {
                double temp = 0;
                for (int k = 0; k < matACols; k++)
                {
                    temp += matA[i, k] * matB[k, j];
                }
                result[i, j] += temp;
            }
        }
    }
    #endregion

    #region Parallel_Loop
    static void Parallel_MM(double[,] matA, double[,] matB, double[,] result)
    {
        int matACols = matA.GetLength(1);
        int matARows = matA.GetLength(0);
        int matBCols = matB.GetLength(1);

        // A basic matrix multiplication.
        // Parallelize the outer loop to partition the source array by rows.
        Parallel.For(0, matARows, i =>
        {
            for (int j = 0; j < matBCols; j++)
            {
                double temp = 0;
                for (int k = 0; k < matACols; k++)
                {
                    temp += matA[i, k] * matB[k, j];
                }
                result[i, j] = temp;
            }
        }); 
    }
    #endregion


    #region Main
    static void Main(string[] args)
    {
        // Set up matrices. Use small values to better view 
        // result matrix. Increase the counts to see greater 
        // speedup in the parallel loop vs. the sequential loop.
        int rowCount = 2000;
        int colCount = 2000;        
        int colCount2 = 2000;
        double[,] matrix1 = InitializeMatrix(rowCount, colCount);
        double[,] matrix2 = InitializeMatrix(colCount, colCount2);
        double[,] result = new double[rowCount, colCount2];

        // Sequential loop
        Console.Error.WriteLine("Executing sequential loop...");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Sequential_MM(matrix1, matrix2, result);
        stopwatch.Stop();
        Console.Error.WriteLine("Sequential loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);

        //OfferToPrint(rowCount, colCount2, result);

        // Reset timer and results matrix. 
        stopwatch.Reset();
        result = new double[rowCount, colCount2];

        // Parallel loop.
        Console.Error.WriteLine("Executing parallel loop...");
        stopwatch.Start();
        Parallel_MM(matrix1, matrix2, result);
        stopwatch.Stop();
        Console.Error.WriteLine("Parallel loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);

        //OfferToPrint(rowCount, colCount2, result);

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

    ////Printing results
    //private static void OfferToPrint(int rowCount, int colCount, double[,] matrix)
    //{
    //    Console.Error.Write("Computation complete. Print results (y/n)? ");
    //    char c = Console.ReadKey(true).KeyChar;
    //    Console.Error.WriteLine(c);
    //    if (Char.ToUpperInvariant(c) == 'Y')
    //    {
    //        if (!Console.IsOutputRedirected) Console.WindowWidth = 160;
    //        Console.WriteLine();
    //        for (int x = 0; x < rowCount; x++)
    //        {
    //            Console.WriteLine("ROW {0}: ", x);
    //            for (int y = 0; y < colCount; y++)
    //            {
    //                Console.Write("{0:#.##} ", matrix[x, y]);
    //            }
    //            Console.WriteLine();
    //        }
    //    }
    //}
    #endregion
}