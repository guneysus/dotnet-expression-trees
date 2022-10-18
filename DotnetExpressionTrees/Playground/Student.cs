namespace Playground;

public class Student
{
    public int Id { get; set; }

    public override string ToString()
    {
        return $"{this.GetType().FullName} {{ Id:{Id} }}";
    }
}
