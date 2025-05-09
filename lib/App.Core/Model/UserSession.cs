namespace App.Domain.Model
{
    public record UserSession(string UserId, string SessionId, DateTime IssuedAt);

}
