using Microsoft.AspNetCore.Mvc;
using People.Api.DTOs;
using People.Api.Services;

namespace People.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService peopleService) 
        {
            _peopleService = peopleService;
        }

        // GET: api/People
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PersonDto>>> List()
        {
            return Ok(await _peopleService.GetAllPeopleAsync()); 
        }

        // GET: api/People/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonDto>> GetById(int id)
        {
            var person = await _peopleService.GetPersonByIdAsync(id); 
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        // POST: api/People
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDto>> Add([FromBody] PersonDto person)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addedPerson = await _peopleService.AddPersonAsync(person);

            return CreatedAtAction(nameof(GetById), new { id = addedPerson.Id }, addedPerson);
        }

        // PUT: api/People/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonDto updatedPerson)
        {
            if(id <= 0 || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 

            var success = await _peopleService.UpdatePersonAsync(id, updatedPerson); 
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/People/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _peopleService.DeletePersonAsync(id); 
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
