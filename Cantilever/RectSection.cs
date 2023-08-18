using System;

namespace Cantilever
{
    public class RectSection : ISection
    {
        public int SectionTypeId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double B { get => b; set => b=value; }
        public double D { get => d; set => d = value; }

        private double b = 0.01;
        private double d = 0.01;

        public RectSection(double b,double d)
        {
            this.b = b;
            this.d = d;
        }

        public double Area()
        {
            return b * d;
        }
        public double MomentOfInertia()
        {
            return b * Math.Pow(d,3)/12;
        }
    }
}
