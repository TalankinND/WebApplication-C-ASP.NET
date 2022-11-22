using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class SignalRepository : ISignalRepository
    {
        public AppContext dbcontext;

        public SignalRepository(AppContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public void SaveSignalRecordRange(List<SignalModel> models)
        {
            dbcontext.SignalList.AddRange(models);

            dbcontext.SaveChanges();
        }

        public List<SignalModel> GetAllRecords()
        {
            return dbcontext.SignalList.ToList();
        }

        public void SaveNewSignalRecord(SignalModel model)
        {
            dbcontext.SignalList.Add(model);
            dbcontext.SaveChanges();
        }

        public void RemoveSignalRecord(SignalModel model)
        {
            dbcontext.SignalList.Remove(model);
            dbcontext.SaveChanges();
        }

        public void EditSignalRecord(SignalModel model)
        {
            dbcontext.Entry(model).State = EntityState.Modified;
            dbcontext.SaveChanges();
        }

        public SignalModel FindRecordById(int? id)
        {
            return dbcontext.SignalList.Find(id);
        }
    }
}
