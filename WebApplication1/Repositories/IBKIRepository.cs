using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IBKIRepository
    {
        List<BKIModel> GetAllRecords();

        void SaveNewBKIRecord(BKIModel model);

        void SaveNewBKIRecordRange(List<BKIModel> models);

        void RemoveBKIRecord(BKIModel model);

        void EditBKIRecord(BKIModel model);

        BKIModel FindRecordById(int? id);
    }
}
