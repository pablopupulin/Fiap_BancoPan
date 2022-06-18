namespace Application.Models;

public class Keyboard
{
    public Keyboard()
    {
        Values = new List<int>();
    }

    public Guid Id { get; set; }

    public ICollection<int> Values { get; set; }
}