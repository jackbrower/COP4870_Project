namespace Library.LearningManagement.Models;

public class Announcements
{ 
    public string? Name { get; set; }
    public string? Body { get; set; }
    //public Person? Author { get; set; }
    public string? Author { get; set; }
    
    public override string ToString()
    {
        return $"{Name} by {Author}: {Author}";
    }
}