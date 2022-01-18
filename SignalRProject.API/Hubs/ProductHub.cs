using Microsoft.AspNetCore.SignalR;
using SignalRProject.API.Models;
using System.Threading.Tasks;

namespace SignalRProject.API.Hubs
{
    public class ProductHub : Hub<IProductHub>
    {
        public async Task SendProduct(Product p)
        {
            await Clients.All.ReceiveProduct(p);
        }
    }
}
