using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaussianElimination
{
    class LinearSystem
    {
        class LinearEquation
        {
            private List<double> coefficients;
            private double freeTerm;

            public LinearEquation(double[] coefficients, double freeTerm)
            {
                this.coefficients = new List<double>(coefficients);
                this.freeTerm = freeTerm;
            }

            public int NumberOfVariables
            {
                get { return coefficients.Count; }
            }
            public double this[int i]
            {
                get { return coefficients[i]; }
            }
            public double FreeTerm
            {
                get { return freeTerm; }
            }

            public double CalculateError(double[] solution)
            {
                double sum = 0;

                for ( int i = 0; i < coefficients.Count; ++i )
                    sum += coefficients[i] * solution[i];

                return freeTerm - sum;
            }

            public static LinearEquation operator -(LinearEquation le1, LinearEquation le2)
            {
                if ( le1.NumberOfVariables != le2.NumberOfVariables )
                    throw new ArgumentException("Number of variables must be equal in both equations");

                double[] coefs = new double[le1.NumberOfVariables];
                double freeTerm = le1.FreeTerm - le2.FreeTerm;

                for ( int i = 0; i < le1.NumberOfVariables; ++i )
                    coefs[i] = le1[i] - le2[i];

                return new LinearEquation(coefs, freeTerm);
            }
            public static LinearEquation operator *(LinearEquation le, double m)
            {
                double[] coefs = new double[le.NumberOfVariables];
                double freeTerm = le.FreeTerm * m;

                for ( int i = 0; i < le.NumberOfVariables; ++i )
                    coefs[i] = le[i] * m;

                return new LinearEquation(coefs, freeTerm);
            }
            public static LinearEquation operator /(LinearEquation le, double m)
            {
                return le * (1.0 / m);
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                for ( int i = 0; i < NumberOfVariables; ++i )
                    sb.Append(String.Format("{0,8:###0.000}", coefficients[i]));
                sb.Append(String.Format(" |{0,8:###0.000}", FreeTerm));

                return sb.ToString();
            }
        }

        private List<LinearEquation> equations;
        private List<int> indices;

        public LinearSystem(double[][] matrix, double[] rhs)
        {
            equations = new List<LinearEquation>();
            indices = new List<int>();

            for ( int i = 0; i < matrix.Length; ++i )
            {
                equations.Add(new LinearEquation(matrix[i], rhs[i]));
                indices.Add(i);
            }
        }

        private int FindGeneralElementIndex(int row)
        {
            int geIndex = row;

            for ( int i = row + 1; i < indices.Count; ++i )
                if ( Math.Abs(equations[row][indices[i]]) > Math.Abs(equations[row][indices[geIndex]]) )
                    geIndex = i;
            SwapIndices(row, geIndex);

            return indices[row];
        }
        private void SwapIndices(int i, int j)
        {
            int temp = indices[i];

            indices[i] = indices[j];
            indices[j] = temp;
        }
        private void Reduce(int row)
        {
            int index = FindGeneralElementIndex(row);
            double generalElement = equations[row][index];

            if ( generalElement == 0 )
                throw new Exception("Linear system is undefined");

            equations[row] /= generalElement;

            for ( int i = row + 1; i < equations.Count; ++i )
                equations[i] -= equations[row] * equations[i][index];
        }

        public double[] Solve()
        {
            if ( equations.Count != equations[0].NumberOfVariables )
            {
                throw new Exception("Number of variables must be equal to the number of equations");
            }

            double[] solution = new double[equations.Count];

            Console.WriteLine("Reducing linear system:");
            Console.WriteLine();

            for ( int i = 0; i < equations.Count; ++i )
            {
                Console.WriteLine("{0} step:", i + 1);
                Console.WriteLine();
                Reduce(i);
                Console.WriteLine(this);
            }
                

            for ( int i = equations.Count - 1; i >= 0; --i )
            {
                double sum = 0;

                for ( int j = i+1; j < equations.Count; ++j )
                    sum += equations[i][indices[j]] * solution[indices[j]];

                solution[indices[i]] = equations[i].FreeTerm - sum;
            }

            return solution;
        }
        public double[] CalculateError(double[] solution)
        {
            if ( solution.Length != equations[0].NumberOfVariables )
                throw new Exception("Invalid lengths");

            double[] error = new double[equations.Count];

            for ( int i = 0; i < equations.Count; ++i )
                error[i] = equations[i].CalculateError(solution);

            return error;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach ( LinearEquation le in equations )
            {
                sb.Append(le.ToString());
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
