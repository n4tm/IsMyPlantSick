namespace IsMyPlantSickApp.Models;

public class Diagnosis : Entity<int> {
    public required PlantSpecies PlantSpecies { get; set; }

    public required PlantDisease PlantDisease { get; set; }

    public int UserId { get; set; }
}

public enum PlantSpecies {
    Unknown,
    Cassava
    // TODO: Add more species
}

public enum PlantDisease {
    None,
    BacterialBlight,
    MosaicDisease,
    BrownStreakDisease,
    GreenMottle
}