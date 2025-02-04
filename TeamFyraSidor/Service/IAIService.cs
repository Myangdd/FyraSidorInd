using TeamFyraSidor.Data;

namespace TeamFyraSidor.Service
{
    public interface IAIService
    {
        Task<string> GetSummary(Article article);
    }
}
