using IsMyPlantSickApp.Database;
using IsMyPlantSickApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IsMyPlantSickApp.Controllers {
    [Route("[diagnoses]")]
    [ApiController]
    public class DiagnosesController : ControllerBase {
        private readonly AppDbContext _dbContext;

        public DiagnosesController(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetUserDiagnoses(int userId) {
            var diagnoses = await _dbContext.Diagnoses.Where(d => d.UserId == userId).ToListAsync();

            if (diagnoses.Count == 0) return NoContent();

            return diagnoses;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Diagnosis>> GetDiagnosis(int id) {
            var diagnosis = await _dbContext.Diagnoses.FindAsync(id);
            if (diagnosis == null) return NotFound();

            return diagnosis;
        }

        [HttpPost]
        public async Task<ActionResult<Diagnosis>> RequestDiagnosis(byte[] image) {
            // TODO:
            // 1. call python script (example: python classification_model.py)
            // 2. store the process standart output into a stream reader
            // 3. read the output lines as strings then check the validity of each field
            // 4. instantiate a new Diagnosis, assigning the required fields according to each output
            // 5. insert the new Diagnosis in _dbContext

            return CreatedAtAction(nameof(GetDiagnosis), new { id = newDiagnosis.Id, newDiagnosis);
        }

    }
}
