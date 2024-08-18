using CrimeGameBlazor_Business.Repository.IRepository;
using CrimeGameBlazor_DataAccess;
using CrimeGameBlazor_DataAccess.Data;
using CrimeGameBlazor_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoBL.Repository
{
    public class GangsterRepository : IGangsterRepository
    {
        private readonly ApplicationDbContext _db;
        public GangsterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Gangster?> GetGangster(int Id)
        {
            return await _db.Gangsters.FindAsync(Id);
        }
    }
}
