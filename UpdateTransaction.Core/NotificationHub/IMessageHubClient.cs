using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateTransaction.Core.NotificationHub
{
    public interface IMessageHubClient
    {
        Task NewUserAdded(List<string> message);
        Task WeatherReport(WeatherForecast[] weatherForecasts);
    }
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
