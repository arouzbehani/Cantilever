namespace Cantilever
{
    public interface ISection
    {
        int Id { get; set; }
        string Name { get; set; }
        int SectionTypeId { get; set; }

        double B { get; set; }
        double D { get; set; }

        double Area();
        double MomentOfInertia();

    }
}
