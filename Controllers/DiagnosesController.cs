using CliWrap;
using Humanizer;
using IsMyPlantSickApp.Database;
using IsMyPlantSickApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IsMyPlantSickApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosesController : ControllerBase {
        private readonly AppDbContext _dbContext;
        private readonly FileSystemWatcher _fileWatcher = new();
        private const string ScriptResultsPath = "/home/script_results";

        public DiagnosesController(AppDbContext dbContext) {
            _dbContext = dbContext;
            _fileWatcher.Path = ScriptResultsPath;
        }

        [HttpGet("Get/{userId}")]
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

        [HttpPost(nameof(RequestDiagnosis))]
        public async Task<ActionResult<Diagnosis>> RequestDiagnosis(RequestDiagnosisDto requestDto) {
            var userId = requestDto.UserId;

            var image = Convert.FromBase64String(requestDto.ImageBytesInBase64);

            if (userId == 0) return NotFound();
            if (image.Length == 0) return BadRequest();

            var inputImgFullPath = CreatedImagePathFromByteArray(image);

            var resultfileName = $"{userId}_{Guid.NewGuid()}_";

            var resultFileFullPath = Path.Combine(ScriptResultsPath, resultfileName);
            using (System.IO.File.Create(resultFileFullPath)) { };

            _fileWatcher.Filter = resultfileName;

            await Cli.Wrap("/home/python3.10")
                .WithArguments([inputImgFullPath, resultFileFullPath])
                .ExecuteAsync();

            var resultFile = _fileWatcher.WaitForChanged(WatcherChangeTypes.Renamed, 1.Minutes());
            
            var diseaseCodeInFileName = resultFile.Name![^1].ToString();
                
            if (!int.TryParse(diseaseCodeInFileName, out int diseaseCode)) return BadRequest();

            if (!Enum.TryParse<PlantDisease>(diseaseCodeInFileName, out var plantDisease)) return BadRequest();

            var newDiagnosis = new Diagnosis {
                PlantSpecies = PlantSpecies.Cassava,
                PlantDisease = plantDisease,
                UserId = userId
            };

            await _dbContext.Diagnoses.AddAsync(newDiagnosis);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDiagnosis), new { id = newDiagnosis.Id }, newDiagnosis);
        }

        private static string CreatedImagePathFromByteArray(byte[] image) {
            var fileName = $"{image.Length}_{Guid.NewGuid()}.jpg";
            var path = Path.Combine("/home/input_images", fileName);
            System.IO.File.WriteAllBytes(path, image);
            return path;
        }
    }
}
