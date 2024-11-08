using Microsoft.AspNetCore.Mvc;

namespace ProductSale.Api.Services.Interfaces
{
    public interface IChatService
    {
        Task<IActionResult> SendMessage(int id);
    }
}
