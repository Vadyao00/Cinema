
namespace Cinema.Domain.DataTransferObjects
{
    public record MovieForCreationDto : MovieForManipulationDto
    {
        public static implicit operator MovieForCreationDto(MovieDto v)
        {
            throw new NotImplementedException();
        }
    }
}
