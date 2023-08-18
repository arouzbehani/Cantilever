namespace Cantilever
{
    public class Material
    {
        public Material(int id, string name, double youngsModulus)
        {
            ID = id;
            Name = name;
            YoungsModulus = youngsModulus;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public double YoungsModulus { get; set; }

    }
}
