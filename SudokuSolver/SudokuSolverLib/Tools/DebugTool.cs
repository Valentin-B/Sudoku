using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolverLib.Tools
{
    public static class DebugTool
    {

        [System.Diagnostics.Conditional("DEBUG")]
        public static void DisplayArray(string arrayName, int[,] arrayToPrint)
        {
            double size = Math.Sqrt(arrayToPrint.Length);
            if (Math.Abs(size % 1.0) > 0)
            {
                throw new ArgumentException(message: "Square root of the array is a decimal value", paramName: nameof(arrayToPrint));
            }

            Debug.WriteLine(arrayName);
            Debug.WriteLine("");
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Debug.Write(string.Format(size > 9 ? "{0:D2} " : "{0:D} ", arrayToPrint[row, col]));
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("");
        }

    }
}
