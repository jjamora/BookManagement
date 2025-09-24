namespace BM.Core.DTO
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int YearPublished { get; set; }
    }
}
