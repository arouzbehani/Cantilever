using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cantilever;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace DLL_Tester
{



    internal class Program
    {
        static void Main(string[] args)
        {
            BeamElement beam = new BeamElement(3000, 1,1000);
            Console.WriteLine(beam.Displacement());

        }
        static double Displacement()
        {
            double L = 3000;
            double P = 1000;
            double E = 200000;
            double b = 50;
            double d = 50;
            int mesh = 10;

            double el = L / mesh;

            // Matrix formulation
            double[] Force = { -P, 0 };
            double[] F = new double[2 * mesh + 2];
            for (int i = 2 * mesh; i < 2 * mesh + 1; i++)
            {
                F[i] = Force[0];
            }

            Console.WriteLine("\nForce Matrix:");
            foreach (double value in F)
            {
                Console.WriteLine(value);
            }

            double[,] kb = new double[4, 4];
            double factor = (E * b * Math.Pow(d, 3)) / (12 * Math.Pow(el, 3));
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    kb[i, j] = factor * new double[,] { { 12, 6 * el, -12, 6 * el },
                                                        { 6 * el, 4 * el * el, -6 * el, 2 * el * el },
                                                        { -12, -6 * el, 12, -6 * el },
                                                        { 6 * el, 2 * el * el, -6 * el, 4 * el * el } }[i, j];
                }
            }
            
            Console.WriteLine("\nElement stiffness matrix K = ");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(kb[i, j] + " ");
                }
                Console.WriteLine();
            }

            //double[,] K = new double[2 + 2 * mesh, 2 + 2 * mesh];
             var K = new List<List<double>>();

            for (int i = 0; i < 2*mesh+2;i++)
            {
                var list = new List<double>();
                for (int j = 0; j < 2 * mesh + 2; j++)
                {
                    list.Add(0);

                }
                K.Add(list);
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    K[i][j] = kb[i, j];
                }
            }

            int k = 1;
            while (k <= 2 * mesh - 2)
            {
                int p = k;
                int i = 1;
                while (i <= 4)
                {
                    int j = 1;
                    int q = k;
                    while (j <= 4)
                    {
                        K[p + 1][q + 1] = K[p + 1][ q + 1] + kb[i - 1, j - 1];
                        j = j + 1;
                        q = q + 1;
                    }
                    i = i + 1;
                    p = p + 1;
                }
                k = k + 2;
            }

            var Kinv = new List<List<double>>();
            for (int i = 2; i < 2 * mesh+2; i++)
            {
                var list = new List<double>();
                for(int j = 2; j < 2 * mesh+2; j++)
                {
                    list.Add(0);
                }
                Kinv.Add(list);

            }
            for (int i = 0; i < 2 * mesh; i++)
            {
                for (int j = 0; j < 2 * mesh; j++)
                {
                    Kinv[i][j] = K[i + 2][ j + 2];
                }
            }
            
            double[] D = new double[2 + 2 * mesh];
            double[] tempD = new double[2 * mesh];
            for (int i = 0; i < 2 * mesh; i++)
            {
                tempD[i] = F[i + 2];
            }
            double[] resultD = MatrixVectorMultiply(Kinv, tempD);
            for (int i = 0; i < 2 * mesh; i++)
            {
                D[i + 2] = resultD[i];
            }

            Console.WriteLine("\nDisp matrix: ");
            foreach (double value in D)
            {
                Console.WriteLine(value);
            }

            // Return the desired value
            return D[D.Length - 2];
        }

        static double[] MatrixVectorMultiply(List<List<double>> matrix, double[] vector)
        {
            int rows = matrix.Count;
            int cols = matrix[0].Count;
            double[] result = new double[rows];

            for (int i = 0; i < rows; i++)
            {
                result[i] = Enumerable.Range(0, cols)
                                     .Select(j => matrix[i][j] * vector[j])
                                     .Sum();
            }

            return result;
        }
        static void test()
        {
            double P = 10;  // Example value for force magnitude
            int mesh = 10;  // Example value for mesh count

            double[][] Force = new double[][] { new double[] { -P,0 } };

            double[] F = new double[2 * mesh + 2];

            // Adding the x-component of Force to specific indices in F
            for (int i = 2 * mesh; i < 2 * mesh + 1; i++)
            {
                F[i] += Force[0][ 0];
            }

            // Printing the contents of the F array
            for (int i = 0; i < F.Length; i++)
            {
                Console.WriteLine(F[i]);
            }
        }
   

    }
}
