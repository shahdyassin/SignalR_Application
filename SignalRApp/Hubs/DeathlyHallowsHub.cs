using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.Hubs
{
    public class DeathlyHallowsHub : Hub
    {
        public Dictionary<string , int> GetRaceStatus()
        {
            return SD.DeathlyHallowRace;
        }
    }
}
