using BM.Core.DTO;
using BM.Core.Model;

namespace BM.Core.Mapper
{
    public static class BookMapper
    {
        public static BookDTO ModelToDTO(this Book data)
        {
            if (data == null) return default(BookDTO)!;
            return new BookDTO
            {
                Id = data.Id,
                Title = data.Title,
                Author = data.Author,
                YearPublished = data.YearPublished
            };
        }

        public static Book DtoToModel(this BookDTO dto)
        {
            if (dto == null) return default!;
            return new Book
            {
                Id = dto.Id,
                Title = dto.Title,
                Author = dto.Author,
                YearPublished = dto.YearPublished
            };
        }
    }
}
