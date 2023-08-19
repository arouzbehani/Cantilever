using System;

using Cantilever;

namespace DLL_Tester
{



    internal class Program
    {
        static void Main(string[] args)
        {
            BeamElement beam = new BeamElement(3000, 1,1,1000);
            Console.WriteLine(beam.Displacements());

        }


    }
}
