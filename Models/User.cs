namespace IsMyPlantSickApp.Models;

public class User : Entity<int> {
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public override bool IsValid() =>
        !string.IsNullOrWhiteSpace(Name) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password);
}

public class UserFilter : EntityFilter<int, User> {
    public HashSet<string>? Names { get; set; }

    public HashSet<string>? Email { get; set; }

    protected override bool FilterMatches(User entity) =>
        DefaultContains(Names, entity.Name) &&
        DefaultContains(Email, entity.Email);
}