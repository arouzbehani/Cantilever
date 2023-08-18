namespace Cantilever
{
    public interface ISection
    {
        double B { get; set; }
        double D { get; set; }

        double Area();
        double MomentOfInertia();
        int SectionTypeId { get; set; }
    }
}
