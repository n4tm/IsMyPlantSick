namespace IsMyPlantSickApp.Models;

public interface IAuditable {
    public DateTimeOffset CreationTime { get; set; }

    public DateTimeOffset LastUpdateTime { get; set; }
}
