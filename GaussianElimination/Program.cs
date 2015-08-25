using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaussianElimination
{
    class Program
    {
        static void Print(double[] array)
        {
            int last = array.Length - 1;

            Console.Write("{ ");
            for ( int i = 0; i < last; ++i )
                Console.Write("{0}, ", array[i]);
            Console.WriteLine("{0} }}", array[last]);
        }

        static void Main(string[] args)
        {
            double[][] m =  {
                                new double[] {   -3, 4,  1,  4 },
                                new double[] {    0, 1, -3,  2 },
                                new double[] {    4, 0, -2, -3 },
                                new double[] { 1000, 3,  1, -5 }
                            };
            double[] b = { -1, -1, 4, -2 };

            LinearSystem ls = new LinearSystem(m, b);

            Console.WriteLine("Initial linear system:");
            Console.WriteLine();

            Console.WriteLine(ls);

            double[] solution = ls.Solve();
            double[] error = ls.CalculateError(solution);

            Console.WriteLine("Solution:");
            Print(solution);
            Console.WriteLine("Error:");
            Print(error);

            Console.ReadKey();
        }
    }
}
