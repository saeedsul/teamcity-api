using Microsoft.EntityFrameworkCore;
using People.Api.DTOs;
using People.Data.Context;
using People.Data.Entities;
namespace People.Api.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly Context _context; 

        public PeopleService(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PersonDto>> GetAllPeopleAsync()
        {
            var peopleEntities = await _context.People.ToListAsync();

            // Map Person entities to PersonDto
            return [.. peopleEntities.Select(p => new PersonDto
            {
                Id = p.Id,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth
            })];
        }

        public async Task<PersonDto?> GetPersonByIdAsync(int id)
        {
            var personEntity = await _context.People.FindAsync(id);

            if (personEntity == null)
            {
                return null;
            }

            return new PersonDto
            {
                Id = personEntity.Id,
                Name = personEntity.Name,
                DateOfBirth = personEntity.DateOfBirth
            };
        }

        public async Task<PersonDto> AddPersonAsync(PersonDto personDto)
        {
            if (string.IsNullOrWhiteSpace(personDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(personDto.Name));
            }

            var personEntity = new Person
            {
                Name = personDto.Name,
                DateOfBirth = personDto.DateOfBirth,
                Id = _context.People.Any() ? _context.People.Max(p => p.Id) + 1 : 1
            };


            _context.People.Add(personEntity);
            await _context.SaveChangesAsync();

            personDto.Id = personEntity.Id;
            return personDto;
        }

        public async Task<bool> UpdatePersonAsync(int id, UpdatePersonDto personDto)
        {
            if (string.IsNullOrWhiteSpace(personDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(personDto.Name));
            }

            var existingPersonEntity = await _context.People.FindAsync(id);
            if (existingPersonEntity == null)
            {
                return false; 
            }

            existingPersonEntity.Name = personDto.Name;
            existingPersonEntity.DateOfBirth = personDto.DateOfBirth;
       
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            var personEntity = await _context.People.FindAsync(id); 

            if (personEntity == null)
            {
                return false; 
            }

            _context.People.Remove(personEntity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
