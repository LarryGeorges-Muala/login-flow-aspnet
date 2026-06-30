namespace Flow.Models;

public class ClientItem
{
    public long Id { get; set; }
    public string? UserName { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public string? PasswordStateChange { get; set; } = string.Empty;
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? EmailStateChange { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public bool Active { get; set; } = false;
    public string? Token { get; set; } = string.Empty;
    public string? QrCode { get; set; } = string.Empty;
    public string? QrCodeUrl { get; set; } = string.Empty;
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public DateTime? LastUpdated { get; set; } = DateTime.Now;
}