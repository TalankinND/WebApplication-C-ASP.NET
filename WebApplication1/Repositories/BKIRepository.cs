using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class BKIRepository:IBKIRepository
    {
        public AppContext dbcontext;

        public BKIRepository(AppContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public List<BKIModel> GetAllRecords()
        {
            return dbcontext.BKIList.ToList();
        }

        public void SaveNewBKIRecordRange(List<BKIModel> models)
        {
            dbcontext.BKIList.AddRange(models);
            dbcontext.SaveChanges();
        }

        public void SaveNewBKIRecord(BKIModel model)
        {
            dbcontext.BKIList.Add(model);
            dbcontext.SaveChanges();
        }

        public void RemoveBKIRecord(BKIModel model)
        {
            dbcontext.BKIList.Remove(model);
            dbcontext.SaveChanges();
        }

        public void EditBKIRecord(BKIModel model)
        {
            dbcontext.Entry(model).State = EntityState.Modified;
            dbcontext.SaveChanges();
        }

        public BKIModel FindRecordById(int? id)
        {
            return dbcontext.BKIList.Find(id);
        }
    }
}
