namespace Nonfy.Domain.Entities;

public class User
{

    private User() {}

    public User(string businessName, string email, string passwordHash, string slug)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("BusinessName cannot be null or empty.", nameof(businessName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("PasswordHash cannot be null or empty.", nameof(passwordHash));
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be null or empty.", nameof(slug));
        
        Id = Guid.NewGuid();
        BusinessName = businessName;
        Email = email;
        PasswordHash = passwordHash;
        Slug = slug;
        CreatedAt = DateTime.UtcNow;
    }
    public Guid Id { get; private set; } // Setter privado para evitar mudanças externas
    public string BusinessName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public void UpdateBusinessName(string newBusinessName)
    {
        if(string.IsNullOrWhiteSpace(newBusinessName))
            throw new ArgumentException("BusinessName cannot be null or empty.", nameof(newBusinessName));
        BusinessName = newBusinessName;
    }

    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email cannot be null or empty.", nameof(newEmail));
        Email = newEmail;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("PasswordHash cannot be null or empty.", nameof(newPasswordHash));
        PasswordHash = newPasswordHash;
    }

    public override bool Equals(object? obj)
    {
        if (obj is User other)
            return Id.Equals(other.Id);
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}