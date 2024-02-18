using IsMyPlantSickApp.Database;
using IsMyPlantSickApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IsMyPlantSickApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosesController(AppDbContext dbContext) : ControllerBase {
        [HttpGet("Get/{userId}")]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetUserDiagnoses(int userId) {
            var diagnoses = await dbContext.Diagnoses.Where(d => d.UserId == userId).ToListAsync();

            if (diagnoses.Count == 0) return NoContent();

            return diagnoses;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Diagnosis>> GetDiagnosis(int id) {
            var diagnosis = await dbContext.Diagnoses.FindAsync(id);
            if (diagnosis == null) return NotFound();

            return diagnosis;
        }

        /*[HttpPost]
        public async Task<ActionResult<Diagnosis>> RequestDiagnosis(byte[] image) {
            // TODO:
            // 1. clear the script output file content
            // 2. call python script (example: python classification_model.py)
            // 3. wait until the file is written (so the script finished)
            // 4. read the file lines then check the validity of each field (PlantSpecies, PlantDisease)
            // 5. instantiate a new Diagnosis, assigning the required fields according to each output
            // 6. insert the new Diagnosis in _dbContext

            return CreatedAtAction(nameof(GetDiagnosis), new { id = newDiagnosis.Id }, newDiagnosis);
        }
        */
    }
}
