using CrimeGameBlazor_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoBL.Repository.IRepository
{
    public interface IGangsterRepository
    {
        public Task<Gangster?> GetGangster(int Id);
    }
}
