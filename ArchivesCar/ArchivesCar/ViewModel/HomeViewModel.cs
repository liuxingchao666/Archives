using ArchivesCar.BLL;
using ArchivesCar.DAL;
using ArchivesCar.Model;
using ArchivesCar.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ArchivesCar.ViewModel
{

    public class HomeViewModel : NotificationObject
    {
        public HomeViewModel(MainWindow mainWindow)
        {
            string error = GetServiceState();
            if (!string.IsNullOrEmpty(error))
            {
                ChangeModel = error;
            }
            if (PublicData.ServerConfig.model.Equals("0"))
                PIC = "../image/homeImage/离线.png";
            else
                PIC = "../image/homeimage/转换.png";
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            thread = new Thread(new ThreadStart(() =>
            {
                while (result)
                {
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Thread.Sleep(1000);
                }
                thread.Abort();
                thread = null;
            }));
            thread.IsBackground = true;
            thread.Start();
            window = mainWindow;
        }
        MainWindow window;
        bool result = true;
        Thread thread;
        /// <summary>
        /// 离线模式切换显示
        /// </summary>
        private string changeModel { get; set; }
        public string ChangeModel
        {
            get { return changeModel; }
            set
            {
                this.changeModel = value;
                this.RaisePropertyChanged(() => ChangeModel);
            }
        }
        /// <summary>
        /// 时间线程
        /// </summary>
        private string time { get; set; }
        public string Time
        {
            get { return time; }
            set
            {
                this.time = value;
                this.RaisePropertyChanged(() => Time);
            }
        }
        /// <summary>
        /// 模式显示图片
        /// </summary>
        private string pic { get; set; }
        public string PIC
        {
            get { return pic; }
            set
            {
                this.pic = value;
                this.RaisePropertyChanged(() => PIC);
            }
        }
        /// <summary>
        /// 盘点
        /// </summary>
        private ICommand inventoryCommond { get; set; }
        public ICommand InventoryCommond
        {
            get
            {
                return inventoryCommond ?? (inventoryCommond = new DelegateCommand(() =>
                {

                   
                    if (GetConn())
                    {
                        result = false;
                        inventoryControl inventoryControl = new inventoryControl(window);
                        window.grid.Children.Clear();
                        window.grid.Children.Add(inventoryControl);
                    }
                }));
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        private ICommand queryCommond { get; set; }
        public ICommand QueryCommond
        {
            get
            {
                return queryCommond ?? (queryCommond = new DelegateCommand(() =>
                {
                 
                    if (GetConn())
                    {
                        result = false;
                        QueryControl queryControl = new QueryControl(window);
                        window.grid.Children.Clear();
                        window.grid.Children.Add(queryControl);
                    }
                }));
            }
        }
        /// <summary>
        /// 档案绑定
        /// </summary>
        private ICommand bindCommond { get; set; }
        public ICommand BindCommond
        {
            get
            {
                return bindCommond ?? (bindCommond = new DelegateCommand(() =>
                {
                   
                    if (GetConn())
                    {
                        result = false;
                        BindControl control = new BindControl(window);
                        window.grid.Children.Clear();
                        window.grid.Children.Add(control);
                    }
                }));
            }
        }
        /// <summary>
        /// 上架
        /// </summary>
        private ICommand upperShelfCommond { get; set; }
        public ICommand UpperShelfCommond
        {
            get
            {
                return upperShelfCommond ?? (upperShelfCommond = new DelegateCommand(() =>
                {
                   
                    if (GetConn())
                    {
                        result = false;
                        UpperShelfControl control = new UpperShelfControl(window);
                        window.grid.Children.Clear();
                        window.grid.Children.Add(control);
                    }
                }));
            }
        }
        /// <summary>
        /// 模式切换
        /// </summary>
        private ICommand changeCommond { get; set; }
        public ICommand ChangeCommond
        {
            get
            {
                return changeCommond ?? (changeCommond = new DelegateCommand(() =>
                {
                    if (PublicData.ServerConfig.model.Equals("0"))
                    {
                        OnlineMode();
                    }
                    else
                    {
                        string error = GetServiceState();
                        if (!string.IsNullOrEmpty(error))
                        {
                            SetError(error);
                        }
                        else
                        {
                            OfflineMode();
                        }
                    }
                }));
            }
        }
        /// <summary>
        /// 切换为离线
        /// </summary>
        public void OfflineMode()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                object errorMsg = null;
                ChangeModel = "正在转换离线模式,请稍后....";
                GetDownLoadFileNamesDAL getDownLoad = new GetDownLoadFileNamesDAL();
                if (getDownLoad.GetDownLoadFileNames(ref errorMsg))
                {
                    ReturnInfo info = errorMsg as ReturnInfo;
                    if (info.TrueOrFalse)
                    {
                        if (info.Code == "")
                        {
                            ///无需下载直接切换离线
                            PIC = "../image/homeImage/离线.png";
                            ChangeSetting("0");
                        }
                        else
                        {
                            try
                            {
                                string path = getPath();
                                if (!string.IsNullOrEmpty(path))
                                {
                                    List<string> list = info.Result as List<string>;
                                    List<string> paths = new List<string>();
                                    foreach (var temp in list)
                                    {
                                        string url = string.Format(@"http://{0}:{1}/archivesmodule/TableSqlFile/download?fileUrl={2}", PublicData.ServerConfig.serverIP, PublicData.ServerConfig.serverPort, temp);
                                        WebClient webClient = new WebClient();
                                        if (File.Exists(path + "/" + temp))
                                            File.Delete(path + "/" + temp);
                                        webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                                        webClient.DownloadFile(new Uri(url), path + "/" + temp);
                                        webClient.DownloadFileAsync(new Uri(url), path);
                                        paths.Add(path);
                                    }
                                    if (DataFileWrite(paths))
                                    {
                                        PIC = "../image/homeImage/离线.png";
                                        ChangeSetting("0");
                                        SetError("成功切换离线模式");
                                    }
                                    else
                                    {
                                        SetError("导入数据库文件失败,请联系相关管理人员！！！");
                                    }
                                }
                                else
                                {
                                    SetError("已放弃切换操作");
                                }
                            }
                            catch (Exception ex)
                            {
                                SetError("切换离线模式失败,与服务器断开连接！！");
                            }
                        }
                    }
                    else
                    {
                        SetError(info.Result.ToString());
                    }
                }
                else
                {
                    SetError(errorMsg.ToString());
                }
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        /// <summary>
        /// 切换为普通模式
        /// </summary>
        public void OnlineMode()
        {
            PIC = "../image/HomeImage/转换.png";
            ChangeSetting("1");
            SetError("成功转换为普通模式");
        }
        /// <summary>
        /// 提示信息显示
        /// </summary>
        /// <param name="data"></param>
        public void SetError(string data)
        {
            Task.Run(() =>
            {
                ChangeModel = data;
                Thread.Sleep(2000);
                ChangeModel = "";
            });
        }
        /// <summary>
        /// 更新模式标识
        /// </summary>
        /// <param name="model"></param>
        public void ChangeSetting(string model)
        {
            PublicData.ServerConfig.model = model;
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["Model"].Value = model;
            cfa.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        /// <summary>
        /// 获取保存路径
        /// </summary>
        /// <returns></returns>
        static string getPath()
        {
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
            saveFileDialog.Description = "请选择您的文件保存路径";
            saveFileDialog.ShowDialog();
            return saveFileDialog.SelectedPath;
        }
        /// <summary>
        /// 数据库文件导入
        /// </summary>
        /// <returns></returns>
        bool DataFileWrite(List<string> paths)
        {
            try
            {
                //导入sql文件
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                // 接受来自调用程序的输入信息
                process.StartInfo.RedirectStandardInput = true;
                //输出信息
                process.StartInfo.RedirectStandardOutput = true;
                // 输出错误
                process.StartInfo.RedirectStandardError = true;
                //不显示程序窗口
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = @ConfigurationManager.AppSettings["Sql"];
                process.Start();
                process.StandardInput.WriteLine("mysql -uroot -p123456");
                //process.StandardInput.WriteLine("create database " + ConfigurationManager.AppSettings["Table"] + ";");
                process.StandardInput.WriteLine("use " + ConfigurationManager.AppSettings["Table"]);
                foreach (var path in paths)
                {
                    process.StandardInput.WriteLine("source " + path);
                }
                //MessageBox.Show(Output);
                //process.WaitForExit();
                if (process != null)
                {
                    process.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取MySql服务状态
        /// </summary>
        /// <returns></returns>
       // [STAThread]
        string GetServiceState()
        {
            var serviceControllers = ServiceController.GetServices();
            foreach (var temp in serviceControllers)
            {
                if (temp.ServiceName.Equals("MySQL"))
                {
                    if (temp.Status != ServiceControllerStatus.Running)
                        return "数据库服务未启动,请启动服务再重试";
                    else
                        return "";
                }
            }
            return "未安装MySql服务器,不支持离线模式";
        }
        public bool isCom = true;
        public bool  GetConn()
        {
            if (PublicData.ServerConfig.connState)
            {
                Task.Run(() => { 
                PublicData.ServerConfig.wirelessRfid.conset();
                });
                return true;
            }
            else
            {
                SetError("无线设备未连接,正在尝试连接....");
                Task.Run(() => {
                    if (PublicData.ServerConfig.wirelessRfid.conn())
                    {
                        SetError("无线设备连接成功，请重新点击");
                    }
                    else
                    {
                        SetError("尝试连接无线设备失败");
                    }
                });
                return false;
            }
        }
    }
}
