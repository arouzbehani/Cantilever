using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Cantilever
{

    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class BeamElement
    {
        private double _length = 1;
        private int _matid = 1;
        private Material material;
        private ISection section;
        private double _force = 0.1;
        public BeamElement()
        {

        }

        public BeamElement(double length, int matid,double force)
        {
            _length = length;
            _matid = matid;
            _force=force;
        }

        public double Force { get => _force; set => _force = value; }
        public double Length { get => _length; set => _length = value; }
        public int Matid { get => _matid; set => _matid = value;  }
        public double[] Displacements(int mesh=10)
        {
            section = new RectSection(50,50);
            var dr = new DataReader();
            var materials = dr.ReadMaterials();
            material = materials[_matid];

            // Beam properties
            double E = material.YoungsModulus; // Young's modulus 
            double L = _length; // Length
            double A = section.Area(); // Cross section area
            double I = section.MomentOfInertia(); // Moment Of Inertia 

            double P = _force;
            double b = section.B;
            double d = section.D;
            //int mesh = 10;

            double el = L / mesh;

            // Force matrix
            Vector<double> Force = DenseVector.OfArray(new double[] { -P, 0 });
            Vector<double> F = DenseVector.Create(2 * mesh + 2, 0);
            F.SetSubVector(2 * mesh, 2, Force);

            Console.WriteLine("Force Matrix :");
            Console.WriteLine(F);

            // Element stiffness matrix kb
            double factor = (E * b * Math.Pow(d, 3)) / (12 * Math.Pow(el, 3));
            Matrix<double> kb = DenseMatrix.OfArray(new double[,]
            {
            { 12, 6 * el, -12, 6 * el },
            { 6 * el, 4 * el * el, -6 * el, 2 * el * el },
            { -12, -6 * el, 12, -6 * el },
            { 6 * el, 2 * el * el, -6 * el, 4 * el * el }
            }).Multiply(factor);

            Console.WriteLine("\nElement stiffness matrix K = ");
            Console.WriteLine(kb);

            // Assembly of stiffness matrix K
            Matrix<double> K = DenseMatrix.Create(2 + 2 * mesh, 2 + 2 * mesh, 0);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    K[i, j] = kb[i, j];
                }
            }
            //Console.WriteLine(K);

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
                        K[p + 1, q + 1] = K[p + 1, q + 1] + kb[i - 1, j - 1];
                        j++;
                        q++;
                    }
                    i++;
                    p++;
                }
                k += 2;
            }
            //Console.WriteLine(K);

            // Boundary condition
            Matrix<double> Kinv = K.SubMatrix(2, 2 * mesh, 2, 2 * mesh).Inverse();
            Vector<double> D = DenseVector.Create(2 + 2 * mesh, 0);
            D.SetSubVector(2, 2 * mesh, Kinv.Multiply(F.SubVector(2, 2 * mesh)));

            Console.WriteLine("\nDisp matrix :" + D.At(2 * mesh ));
            //Console.WriteLine(D);

            double[] deflections=new double[mesh+1];
            for (int i=0; i <= 2 * mesh + 1; i+=2)
            {
                deflections[i / 2] = D.At(i);
            }
            return deflections;
        }

    }
}
