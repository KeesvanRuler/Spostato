using SpostatoDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWebApp.Services
{
    public interface IGangsterService
    {
        Task<Gangster> GetCurrentAliveGangsterAsync();
        Task UpdateGangsterAsync(Gangster gangster);
    }
}
