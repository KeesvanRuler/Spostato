using AutoMapper;
using CrimeGameBlazor_Business.Repository.IRepository;
using CrimeGameBlazor_DataAccess;
using CrimeGameBlazor_DataAccess.Data;
using CrimeGameBlazor_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoBL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<int> DeleteById(string Id)
        {
            var obj = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == Id);
            if (obj != null)
            {
                _db.ApplicationUsers.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserDTO>>(_db.ApplicationUsers);
        }

        public async Task<UserDTO> GetById(string Id)
        {
            var obj = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == Id);
            if (obj != null)
            {
                return _mapper.Map<ApplicationUser, UserDTO>(obj);
            }
            return new UserDTO();
        }
    }
}
