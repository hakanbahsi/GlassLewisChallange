namespace GlassLewisChallange.Infrastructure.Security
{
    public interface IIdProtector
    {
        string Protect(Guid id);
        Guid Unprotect(string protectedId);
        bool CanUnprotect(string encrypted);
    }
}
