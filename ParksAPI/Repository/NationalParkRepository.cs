using ParksAPI.Data;
using ParksAPI.Models;
using ParksAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParksAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext db;

        public NationalParkRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            this.db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            this.db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return this.db.NationalParks.FirstOrDefault(a => a.Id == nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return this.db.NationalParks.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = this.db.NationalParks.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            bool value = this.db.NationalParks.Any(a => a.Id == id);
            return value;
        }

        public bool Save()
        {
            return this.db.SaveChanges() > 0;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            this.db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
