using ArchivesCar.DAL;
using ArchivesCar.Model;
using ArchivesCar.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ArchivesCar.ViewModel
{
    public class InventroyViewModel : NotificationObject
    {
        public InventroyViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            thread = new Thread(new ThreadStart(() =>
            {
                while (result)
                {
                    lock (PublicData.ServerConfig.UserList)
                    {
                        if (PublicData.ServerConfig.UserList.Count > 0)
                        {
                            string uid = PublicData.ServerConfig.UserList.Dequeue();
                            EPC ="当前层架标签位置:"+ GetPosition(uid);
                            GetDataByEPCDAL getDataByEPC = new GetDataByEPCDAL();
                            object errorMsg = null;
                            if (getDataByEPC.GetDataByEPC(ref errorMsg))
                            {
                                ReturnInfo info = errorMsg as ReturnInfo;
                                if (info.TrueOrFalse)
                                {
                                    List<InventoryInfo> infos = info.Result as List<InventoryInfo>;
                                    List = infos;
                                    ///刷新回显
                                    
                                }
                            }
                        }
                    }
                    Thread.Sleep(300);
                }
                thread.Abort();
                thread = null;
            }));
            thread.IsBackground = true;
            thread.Start();
            PIC = "../image/停止.png";
        }
        bool result = true;
        Thread thread;
        MainWindow mainWindow;
        /// <summary>
        /// 返回
        /// </summary>
        private ICommand backCommond { get; set; }
        public ICommand BackCommond
        {
            get
            {
                return backCommond ?? (backCommond = new DelegateCommand(() =>
                {
                    result = false;
                    HomeControl homeControl = new HomeControl(mainWindow);
                    mainWindow.grid.Children.Clear();
                    mainWindow.grid.Children.Add(homeControl);
                }));
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        private ICommand exportCommond { get; set; }
        public ICommand ExportCommond
        {
            get
            {
                return exportCommond ?? (exportCommond = new DelegateCommand(() =>
                {
                    if (List.Count > 0)
                    {
                        string path = getPath();
                        if (string.IsNullOrEmpty(path))
                            return;
                        path += "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                        if (File.Exists(path))
                            File.Delete(path);
                        HSSFWorkbook hSSF = new HSSFWorkbook();
                        HSSFSheet hSSFSheet = (HSSFSheet)hSSF.CreateSheet("Sheet1");
                        HSSFRow cells = (HSSFRow)hSSFSheet.CreateRow(0);
                        cells.CreateCell(0).SetCellValue("序号");
                        cells.CreateCell(1).SetCellValue("题名");
                        cells.CreateCell(2).SetCellValue("类别");
                        cells.CreateCell(3).SetCellValue("条码");
                        cells.CreateCell(4).SetCellValue("RFID");
                        cells.CreateCell(5).SetCellValue("存放位置");
                        cells.CreateCell(6).SetCellValue("状态");
                        for (int i = 0; i < List.Count; i++)
                        {
                            InventoryInfo checkModel = List[i];
                            HSSFRow hssfr = (HSSFRow)hSSFSheet.CreateRow(i + 1);
                            hssfr.CreateCell(0).SetCellValue(i + 1);
                            hssfr.CreateCell(1).SetCellValue(checkModel.fkFileName);
                            hssfr.CreateCell(2).SetCellValue(checkModel.isBox);
                            hssfr.CreateCell(3).SetCellValue(checkModel.barCode);
                            hssfr.CreateCell(4).SetCellValue(checkModel.rfid);
                            hssfr.CreateCell(5).SetCellValue(checkModel.locationName);
                            hssfr.CreateCell(6).SetCellValue(checkModel.state);

                        }
                        using (FileStream stream = new FileStream(path, FileMode.Create))
                        {
                            hSSF.Write(stream);
                        }
                        MessageBox.Show("导出成功");
                    }
                    else
                    {
                        MessageBox.Show("没有需要导出的数据");
                    }

                }));
            }
        }
        /// <summary>
        /// 停止线程刷新,恢复重新运行
        /// </summary>
        private ICommand changeCommond { get; set; }
        public ICommand ChangeCommond
        {
            get
            {
                return changeCommond ?? (changeCommond = new DelegateCommand(() =>
                {
                    if (PIC.Contains("扫描"))
                        PIC = "../image/停止.png";
                    else
                        PIC = "../image/扫描.png";
                }));
            }
        }
        /// <summary>
        /// 扫描或停止
        /// </summary>
        private string pic { get; set; }
        public string PIC
        {
            get { return pic; }
            set
            {
                pic = value;
                this.RaisePropertyChanged(() => PIC);
            }
        }
        /// <summary>
        /// 导出状态回显
        /// </summary>
        private string exportState { get; set; }
        public string ExportState
        {
            get { return exportState; }
            set
            {
                exportState = value;
                this.RaisePropertyChanged(() => ExportState);
            }
        }
        /// <summary>
        /// 当前处理的层架标签
        /// </summary>
        private string epc { get; set; }
        public string EPC
        {
            get { return epc; }
            set
            {
                epc = value;
                this.RaisePropertyChanged(() => EPC);
            }
        }
        /// <summary>
        /// 架上该拥有的总数
        /// </summary>
        private string numberTotal { get; set; }
        public string NumberTotal
        {
            get { return numberTotal; }
            set
            {
                numberTotal = value;
                this.RaisePropertyChanged(() => NumberTotal);
            }
        }
        /// <summary>
        /// 空架数量
        /// </summary>
        private string numberEmpty { get; set; }
        public string NumberEmpty
        {
            get { return numberEmpty; }
            set
            {
                numberEmpty = value;
                this.RaisePropertyChanged(() => NumberEmpty);
            }
        }
        /// <summary>
        /// 在架数量
        /// </summary>
        private string numberShelves { get; set; }
        public string NumberShelves
        {
            get { return numberShelves; }
            set
            {
                numberShelves = value;
                this.RaisePropertyChanged(() => NumberShelves);
            }
        }
        /// <summary>
        /// 顺架数量
        /// </summary>
        private string numberCages { get; set; }
        public string NumberCages
        {
            get { return numberCages; }
            set
            {
                numberCages = value;
                this.RaisePropertyChanged(() => NumberCages);
            }
        }
        /// <summary>
        /// 错架数量
        /// </summary>
        private string numberWrong { get; set; }
        public string NumberWrong
        {
            get { return numberWrong; }
            set
            {
                numberWrong = value;
                this.RaisePropertyChanged(() => NumberWrong);
            }
        }
        /// <summary>
        /// 数据列
        /// </summary>
        private List<InventoryInfo> list { get; set; } = new List<InventoryInfo>();
        public List<InventoryInfo> List
        {
            get { return list; }
            set
            {
                list = value;
                this.RaisePropertyChanged(() => List);
            }
        }
        /// <summary>
        /// 根据uid获取标签位置
        /// </summary>
        string GetPosition(string user)
        {
            string USER = user.Replace(" ", "");
            if (USER.Substring(0, 8) == "AA0CFFA5")
            {
                string direction = "";
                string fkRegionNum = USER.Substring(8, 4);
                string colNum = USER.Substring(12, 4);
                string divNum = USER.Substring(16, 4);
                string laysNum = USER.Substring(20, 4);
                if (USER.Substring(24, 4) == "0000") { direction = "左"; }
                else { direction = "右"; }
                return  fkRegionNum.TrimStart('0') + "区" + colNum.TrimStart('0') + "列" + divNum.TrimStart('0') + "节" + laysNum.TrimStart('0') + "层" + direction.TrimStart('0') + "侧";
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取文件保存路径
        /// </summary>
        /// <returns></returns>
        [STAThread]
        string getPath()
        {
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
            saveFileDialog.Description = "请选择您的文件保存路径";
            saveFileDialog.ShowDialog();
            return saveFileDialog.SelectedPath;
        }
    }
}
