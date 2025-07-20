using People.Api.DTOs;
namespace People.Api.Services
{
    public interface IPeopleService
    {
        Task<IEnumerable<PersonDto>> GetAllPeopleAsync();
        Task<PersonDto?> GetPersonByIdAsync(int id);
        Task<PersonDto> AddPersonAsync(PersonDto person);
        Task<bool> UpdatePersonAsync(int id, UpdatePersonDto person);
        Task<bool> DeletePersonAsync(int id);
    }
}
