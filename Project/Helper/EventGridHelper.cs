using Azure;
using Azure.Messaging.EventGrid;

namespace Project.Helper
{
    public class EventGridHelper
    {
        private readonly IConfiguration _configuration;

        public EventGridHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEventToEventGrid(string subject, string eventType, object payload)
        {
            string topicEndpoint = _configuration["EventGrid:TopicEndpoint"];
            string topicKey = _configuration["EventGrid:TopicKey"];

            var client = new EventGridPublisherClient(new Uri(topicEndpoint), new AzureKeyCredential(topicKey));

            var eventData = new EventGridEvent(
                subject: subject,
                eventType: eventType,
                dataVersion: "1.0",
                data: payload
            );

            try
            {
                await client.SendEventAsync(eventData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while sending event: {ex.Message}");
            }
        }
    }
}
