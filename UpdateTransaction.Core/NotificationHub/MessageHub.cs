using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateTransaction.Core.NotificationHub
{
    public class MessageHub:Hub<IMessageHubClient>
    {
        public async Task NewUserAdded(List<string> message)
        {
            await Clients.All.NewUserAdded(message);
        }
        public async Task WeatherReport(WeatherForecast[] weatherForecasts)
        {
            await Clients.All.WeatherReport(weatherForecasts);
        }
    }
}
