using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WebApplication1.ModelBuilders;
using WebApplication1.Models;
using WebApplication1.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private readonly IDeviceListRepository deviceListRepository;
        private readonly IDeviceListModelBuilder deviceListModelBuilder;
        private readonly ISignalRepository signalRepository;
        private readonly ISignalModelBuilder signalModelBuilder;
        private readonly IBKIRepository BKIRepository;
        private readonly IBKIModelBuilder BKIModelBuilder;
        private readonly IWebHostEnvironment _WebhostingEnvironment;
        public HomeController(IDeviceListRepository deviceListRepository, IDeviceListModelBuilder deviceListModelBuilder, ISignalRepository signalRepository, ISignalModelBuilder signalModelBuilder, IBKIRepository BKIRepository, IBKIModelBuilder BKIModelBuilder, IWebHostEnvironment _WebhostingEnvironment)
        {
            this.deviceListRepository = deviceListRepository;
            this.deviceListModelBuilder = deviceListModelBuilder;
            this.signalRepository = signalRepository;
            this.signalModelBuilder = signalModelBuilder;
            this.BKIRepository = BKIRepository;
            this.BKIModelBuilder = BKIModelBuilder;
            this._WebhostingEnvironment = _WebhostingEnvironment;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(deviceListModelBuilder.GetDeviceListInfo());
        }

        [HttpGet]
        public IActionResult AddDeviceListRecord()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDeviceListRecord(DeviceListModel model)
        {
            deviceListRepository.SaveNewDeviceRecord(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeviceListRecordRemove(int id)
        {
            return View(deviceListModelBuilder.GetDeviceInfoById(id));
        }

        [HttpPost]
        public IActionResult DeviceListRecordRemove(DeviceListModel model)
        {
            var removeableModel = deviceListRepository.FindRecordById(model.DeviceID);

            deviceListRepository.RemoveDeviceRecord(removeableModel);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeviceRecordEdit(int id)
        {
            return View(deviceListRepository.FindRecordById(id));
        }

        [HttpPost]
        public IActionResult DeviceRecordEdit(DeviceListModel model)
        {
            deviceListRepository.EditDeviceRecord(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeviceRecordDetails(int id)
        {
            return View(deviceListRepository.FindRecordById(id));
        }

        [HttpPost]
        public IActionResult DeviceRecordDetails()
        {
            return RedirectToAction("Index");
        }

        public string[] AutocompleteSearch(string term)
        {
            var models = deviceListRepository.GetAllDeviceRecord().Where(a => a.DeviceName.Contains(term)).Select(a => a.DeviceName).ToArray();

            return models;
        }

        [HttpGet]
        public IActionResult LookSignals(int id)
        {
            return View(signalModelBuilder.GetSignalListInfo(id));  
        }

        [HttpGet]
        public IActionResult CreateSignals()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSignals(SignalModel model, int? id)
        {
            model.DeviceID = (int)id;
            signalRepository.SaveNewSignalRecord(model);
            return RedirectToAction("LookSignals", "Home", new { id = id });
        }

        [HttpGet]
        public IActionResult SignalsRecordEdit(int id)
        {
            return View(signalRepository.FindRecordById(id));
        }

        [HttpPost]
        public IActionResult SignalsRecordEdit(SignalModel model)
        {
            signalRepository.EditSignalRecord(model);
            return RedirectToAction("LookSignals", "Home", new { id = model.DeviceID });
        }

        [HttpGet]
        public IActionResult SignalRecordRemove(int id)
        {
            return View(signalModelBuilder.GetSignalInfoById(id));
        }

        [HttpPost]
        public IActionResult SignalRecordRemove(SignalModel model)
        {
            var removeableModel = signalRepository.FindRecordById(model.RecordID);

            signalRepository.RemoveSignalRecord(removeableModel);

            return RedirectToAction("LookSignals", "Home", new { id = removeableModel.DeviceID });
        }

        [HttpGet]
        public IActionResult LookBKI(int id)
        {
            return View(BKIModelBuilder.GetBKIListInfo(id));
        }

        [HttpGet]
        public IActionResult CreateBKI()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBKI(BKIModel model, int? id)
        {
            model.DeviceID = (int)id;
            BKIRepository.SaveNewBKIRecord(model);
            return RedirectToAction("LookBKI", "Home", new { id = id });
        }

        [HttpGet]
        public IActionResult BKIRecordEdit(int id)
        {
            return View(BKIRepository.FindRecordById(id));
        }

        [HttpPost]
        public IActionResult BKIRecordEdit(BKIModel model)
        {
            BKIRepository.EditBKIRecord(model);
            return RedirectToAction("LookBKI", "Home", new { id = model.DeviceID });
        }

        [HttpGet]
        public IActionResult BKIRecordRemove(int id)
        {
            return View(BKIModelBuilder.GetBKIInfoById(id));
        }

        [HttpPost]
        public IActionResult BKIRecordRemove(BKIModel model)
        {
            var removeableModel = BKIRepository.FindRecordById(model.RecordId);

            BKIRepository.RemoveBKIRecord(removeableModel);

            return RedirectToAction("LookBKI", "Home", new { id = removeableModel.DeviceID });
        }

        public async Task<IActionResult> OnPostExportDeviceList()
        {
            List<DeviceListModel> model = deviceListRepository.GetAllDeviceRecord();
            int i = 1;
            string sWebRootFolder = _WebhostingEnvironment.WebRootPath;
            string sFileName = @"Список устройств.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Список устройств");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Наименование устройства");
                row.CreateCell(1).SetCellValue("Сокращённое наименование устройства");
                row.CreateCell(2).SetCellValue("Тип устройства");

                foreach(var item in model)
                {
                    row = excelSheet.CreateRow(i);
                    row.CreateCell(0).SetCellValue(item.DeviceName);
                    row.CreateCell(1).SetCellValue(item.DeviceShortName);
                    row.CreateCell(2).SetCellValue(item.DeviceType);
                    
                    i++;
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostExportBKI(int id)
        {
            List<BKIModel> model = BKIModelBuilder.GetBKIListInfo(id);
            DeviceListModel deviceListModel = new DeviceListModel();
            deviceListModel = deviceListRepository.FindRecordById(id);

            int i = 1;

            string sWebRootFolder = _WebhostingEnvironment.WebRootPath;
            string sFileName = String.Format(@"{0}.xlsx", deviceListModel.DeviceName);
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName),FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                ISheet excelSheet = workbook.CreateSheet(deviceListModel.DeviceName);

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("№");
                row.CreateCell(1).SetCellValue("Объект");
                row.CreateCell(2).SetCellValue("Тип ОС");
                row.CreateCell(3).SetCellValue("Нормальное состояние");
                row.CreateCell(4).SetCellValue("Номер ППК");
                row.CreateCell(5).SetCellValue("Номер шлейфа");
                row.CreateCell(6).SetCellValue("Код взятия или Снятия");
                row.CreateCell(7).SetCellValue("Комментарий на БКИ");
                row.CreateCell(8).SetCellValue("1-я надпись");
                row.CreateCell(9).SetCellValue("2-я надпись");
                row.CreateCell(10).SetCellValue("Номер раздела");


                foreach (var item in model)
                {
                    row = excelSheet.CreateRow(i);
                    row.CreateCell(0).SetCellValue(item.Lamp);
                    row.CreateCell(1).SetCellValue(item.Object);
                    row.CreateCell(2).SetCellValue(item.OSType);
                    row.CreateCell(3).SetCellValue(item.CommonCondition);
                    row.CreateCell(4).SetCellValue(item.PKNumber);
                    row.CreateCell(5).SetCellValue(item.SHNumber);
                    row.CreateCell(6).SetCellValue(item.Deposit_Withdraw_code);
                    row.CreateCell(7).SetCellValue(item.BKI_Commentary);
                    row.CreateCell(8).SetCellValue(item.First_Inscription);
                    row.CreateCell(9).SetCellValue(item.Second_Incription);
                    row.CreateCell(10).SetCellValue(item.Section_Number);

                    i++;

                }
                
                workbook.Write(fs);

            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostExportAllBKI()
        {
            List<DeviceListModel> deviceListModel = deviceListRepository.GetAllDeviceRecord().Where(p => p.DeviceType.ToLower() == "бки").ToList();

            int i = 1;

            string sWebRootFolder = _WebhostingEnvironment.WebRootPath;
            string sFileName = @"Все БКИ.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                foreach(var bki in deviceListModel)
                {
                    List<BKIModel> model = BKIModelBuilder.GetBKIListInfo(bki.DeviceID);

                    ISheet excelSheet = workbook.CreateSheet(bki.DeviceName);

                    IRow row = excelSheet.CreateRow(0);

                    row.CreateCell(0).SetCellValue("№");
                    row.CreateCell(1).SetCellValue("Объект");
                    row.CreateCell(2).SetCellValue("Тип ОС");
                    row.CreateCell(3).SetCellValue("Нормальное состояние");
                    row.CreateCell(4).SetCellValue("Номер ППК");
                    row.CreateCell(5).SetCellValue("Номер шлейфа");
                    row.CreateCell(6).SetCellValue("Код взятия или Снятия");
                    row.CreateCell(7).SetCellValue("Комментарий на БКИ");
                    row.CreateCell(8).SetCellValue("1-я надпись");
                    row.CreateCell(9).SetCellValue("2-я надпись");
                    row.CreateCell(10).SetCellValue("Номер раздела");


                    foreach (var item in model)
                    {
                        row = excelSheet.CreateRow(i);
                        row.CreateCell(0).SetCellValue(item.Lamp);
                        row.CreateCell(1).SetCellValue(item.Object);
                        row.CreateCell(2).SetCellValue(item.OSType);
                        row.CreateCell(3).SetCellValue(item.CommonCondition);
                        row.CreateCell(4).SetCellValue(item.PKNumber);
                        row.CreateCell(5).SetCellValue(item.SHNumber);
                        row.CreateCell(6).SetCellValue(item.Deposit_Withdraw_code);
                        row.CreateCell(7).SetCellValue(item.BKI_Commentary);
                        row.CreateCell(8).SetCellValue(item.First_Inscription);
                        row.CreateCell(9).SetCellValue(item.Second_Incription);
                        row.CreateCell(10).SetCellValue(item.Section_Number);

                        i++;
                    }

                }
                
                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostExportSignal(int id)
        {
            List<SignalModel> model = signalModelBuilder.GetSignalListInfo(id);
            DeviceListModel deviceListModel = new DeviceListModel();
            deviceListModel = deviceListRepository.FindRecordById(id);

            int i = 1;

            string sWebRootFolder = _WebhostingEnvironment.WebRootPath;
            string sFileName = String.Format(@"{0}.xlsx", deviceListModel.DeviceName);
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                ISheet excelSheet = workbook.CreateSheet(deviceListModel.DeviceName);

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Номер шлейфа");
                row.CreateCell(1).SetCellValue("Кабинет");
                row.CreateCell(2).SetCellValue("Тип сигнализации");
                row.CreateCell(3).SetCellValue("Названия датчиков");
                row.CreateCell(4).SetCellValue("Раздел");
                row.CreateCell(5).SetCellValue("Кодовая комбинация");
                row.CreateCell(6).SetCellValue("БИ (номер или лампочка)");
                row.CreateCell(7).SetCellValue("Подпись на БИ");
                row.CreateCell(8).SetCellValue("Комментарий на БИ");
                row.CreateCell(9).SetCellValue("Подпись на табло охраны");
                row.CreateCell(10).SetCellValue("Примечание");


                foreach (var item in model)
                {
                    row = excelSheet.CreateRow(i);
                    row.CreateCell(0).SetCellValue(item.HS);
                    row.CreateCell(1).SetCellValue(item.Сabinet);
                    row.CreateCell(2).SetCellValue(item.SignalType);
                    row.CreateCell(3).SetCellValue(item.SensorName);
                    row.CreateCell(4).SetCellValue(item.Section);
                    row.CreateCell(5).SetCellValue(item.CodeCombination);
                    row.CreateCell(6).SetCellValue(item.BI_Number_Lamp);
                    row.CreateCell(7).SetCellValue(item.Signature_BI);
                    row.CreateCell(8).SetCellValue(item.Comment_BI);
                    row.CreateCell(9).SetCellValue(item.Signature_Security);
                    row.CreateCell(10).SetCellValue(item.Bonus);

                    i++;
                }

                workbook.Write(fs);

            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<IActionResult> OnPostExportAllSignal()
        {
            List<DeviceListModel> deviceListModel = deviceListRepository.GetAllDeviceRecord().Where(p => p.DeviceType.ToLower() == "сигнал").ToList();

            int i = 1;

            string sWebRootFolder = _WebhostingEnvironment.WebRootPath;
            string sFileName = @"Все Сигналы.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();

                foreach (var signal in deviceListModel)
                {
                    List<SignalModel> model = signalModelBuilder.GetSignalListInfo(signal.DeviceID);

                    ISheet excelSheet = workbook.CreateSheet(signal.DeviceName);

                    IRow row = excelSheet.CreateRow(0);

                    row.CreateCell(0).SetCellValue("Номер шлейфа");
                    row.CreateCell(1).SetCellValue("Кабинет");
                    row.CreateCell(2).SetCellValue("Тип сигнализации");
                    row.CreateCell(3).SetCellValue("Названия датчиков");
                    row.CreateCell(4).SetCellValue("Раздел");
                    row.CreateCell(5).SetCellValue("Кодовая комбинация");
                    row.CreateCell(6).SetCellValue("БИ (номер или лампочка)");
                    row.CreateCell(7).SetCellValue("Подпись на БИ");
                    row.CreateCell(8).SetCellValue("Комментарий на БИ");
                    row.CreateCell(9).SetCellValue("Подпись на табло охраны");
                    row.CreateCell(10).SetCellValue("Примечание");


                    foreach (var item in model)
                    {
                        row = excelSheet.CreateRow(i);
                        row.CreateCell(0).SetCellValue(item.HS);
                        row.CreateCell(1).SetCellValue(item.Сabinet);
                        row.CreateCell(2).SetCellValue(item.SignalType);
                        row.CreateCell(3).SetCellValue(item.SensorName);
                        row.CreateCell(4).SetCellValue(item.Section);
                        row.CreateCell(5).SetCellValue(item.CodeCombination);
                        row.CreateCell(6).SetCellValue(item.BI_Number_Lamp);
                        row.CreateCell(7).SetCellValue(item.Signature_BI);
                        row.CreateCell(8).SetCellValue(item.Comment_BI);
                        row.CreateCell(9).SetCellValue(item.Signature_Security);
                        row.CreateCell(10).SetCellValue(item.Bonus);

                        i++;
                    }
                }

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        [HttpPost]
        public IActionResult OnPostImportDeviceList()
        {
            List<DeviceListModel> models = new List<DeviceListModel>();
            DeviceListModel model = new DeviceListModel();
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _WebhostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        
                        models.Add(new DeviceListModel {
                            DeviceName = row.GetCell(0).ToString(),
                            DeviceShortName = row.GetCell(1).ToString(),
                            DeviceType = row.GetCell(2).ToString()
                        });
                    }
                }
            }

            deviceListRepository.SaveNewDeviceRecordRange(models);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult OnPostImportSignal(int id)
        {
            List<SignalModel> models = new List<SignalModel>();
            SignalModel model = new SignalModel();
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _WebhostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        model.DeviceID = id;
                        model.HS = Int32.Parse(row.GetCell(0).ToString());
                            models.Add(new SignalModel
                            {
                                DeviceID = id,
                                HS = Int32.Parse(row.GetCell(0).ToString()),
                                Сabinet = row.GetCell(1).ToString(),
                                SignalType = row.GetCell(2).ToString(),
                                SensorName = row.GetCell(3).ToString(),
                                Section = row.GetCell(4).ToString(),
                                CodeCombination = row.GetCell(5).ToString(),
                                BI_Number_Lamp = row.GetCell(6).ToString(),
                                Signature_BI = row.GetCell(7).ToString(),
                                Comment_BI = row.GetCell(8).ToString(),
                                Signature_Security = row.GetCell(9).ToString(),
                                Bonus = row.GetCell(10).ToString()
                            });
                    }
                }
            }

            signalRepository.SaveSignalRecordRange(models);

            return RedirectToAction("LookSignals", "Home", new { id = id });
        }

        [HttpPost]
        public IActionResult OnPostImportBKI(int id)
        {
            List<BKIModel> models = new List<BKIModel>();
            BKIModel model = new BKIModel();
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _WebhostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        
                        models.Add(new BKIModel {
                            DeviceID = id,
                            Lamp =  Int32.Parse(row.GetCell(0).ToString()),
                            Object = row.GetCell(1).ToString(),
                            OSType = row.GetCell(2).ToString(),
                            CommonCondition = row.GetCell(3).ToString(),
                            PKNumber = row.GetCell(4).ToString(),
                            SHNumber = row.GetCell(5).ToString(),
                            Deposit_Withdraw_code = row.GetCell(6).ToString(),
                            BKI_Commentary = row.GetCell(7).ToString(),
                            First_Inscription = row.GetCell(8).ToString(),
                            Second_Incription = row.GetCell(9).ToString(),
                            Section_Number = row.GetCell(10).ToString()
                        });
                    }
                }
            }

            BKIRepository.SaveNewBKIRecordRange(models);

            return RedirectToAction("LookBKI", "Home", new { id = id }); ;
        }
    }
}
