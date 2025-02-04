using Azure;
using Azure.AI.TextAnalytics;
using Newtonsoft.Json.Serialization;
using TeamFyraSidor.Data;

namespace TeamFyraSidor.Service
{
    public class AIService : IAIService
    {
        public readonly IConfiguration _configuration;

        public AIService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetSummary(Article article)
        {
            Uri endpoint = new(_configuration["LanguageAI:Endpoint"]);
            AzureKeyCredential credential = new(_configuration["LanguageAI:ApiKeyTwo"]);
            TextAnalyticsClient client = new(endpoint, credential);

            string content = article.Content;
            string contentSummary = "";
            List<string> batchedContent = new()
            {
                content
            };

            AbstractiveSummarizeOperation operation = client.AbstractiveSummarize(WaitUntil.Completed, batchedContent);

            await foreach (AbstractiveSummarizeResultCollection contentsInPage in operation.Value)
            {
                foreach (AbstractiveSummarizeResult contentResult in contentsInPage)
                {
                    foreach (AbstractiveSummary summary in contentResult.Summaries)
                    {
                        contentSummary = $"<strong>Summary</strong><br />{summary.Text.Replace("\n", "<br />")}";
                    }

                }
            }
            return contentSummary;
        }
    }
}
