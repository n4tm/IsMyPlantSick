namespace IsMyPlantSickApp.Models;

public class Diagnosis : Entity<int> {
    public required PlantSpecies PlantSpecies { get; set; }

    public required PlantDisease PlantDisease { get; set; }

    public int UserId { get; set; }
}

public record RequestDiagnosisDto(string ImageBytesInBase64, int UserId);


public enum PlantSpecies {
    Unknown,
    Cassava
    // TODO: Add more species
}

public enum PlantDisease {
    BacterialBlight,
    BrownStreakDisease,
    GreenMottle,
    MosaicDisease,
    None
}