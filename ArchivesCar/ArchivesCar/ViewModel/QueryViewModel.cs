using ArchivesCar.DAL;
using ArchivesCar.Model;
using ArchivesCar.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArchivesCar.ViewModel
{
    public class QueryViewModel : NotificationObject
    {
        public QueryViewModel(MainWindow window)
        {
            this.mainWindow = window;
            PIC = "../image/扫描.png";
            if (PublicData.ServerConfig.model.Equals("0"))
                Model = "离线模式";
            else
                Model = "普通模式";
            thread = new Thread(new ThreadStart(()=> {
                while (result)
                {
                    lock (PublicData.ServerConfig.EpcS)
                    {
                        ///获取2集合不重叠部分
                        var list = (from c in PublicData.ServerConfig.EpcS
                                   where!(from d in PublicData.ServerConfig.UEpcS select d).Contains(c)
                                   select c).ToList();
                        int i = 0;
                        GetSelArcByEpc getSel = new GetSelArcByEpc();
                        foreach (var temp in list)
                        {
                            object errorMsg = temp;
                            if (getSel.SelArcByEpc(ref errorMsg))
                            {
                                ReturnInfo info = errorMsg as ReturnInfo;
                                if (info.TrueOrFalse)
                                {
                                    InventoryInfo inventoryInfo = info.Result as InventoryInfo;
                                    inventoryInfo.order = i;
                                    i++;
                                    List.Add(inventoryInfo);
                                    PublicData.ServerConfig.UEpcS.Add(temp);
                                }
                                else
                                {
                                    setError(info.Result.ToString());
                                    break;
                                }
                            }
                            else
                            {
                                setError(errorMsg.ToString());
                                break;
                            }
                        }
                        Total = "已扫描到标签数量:" + PublicData.ServerConfig.UEpcS.Count;
                    }
                    Thread.Sleep(300);
                }
                thread.Abort();
                thread = null;
            }));
            thread.IsBackground = true;
            thread.Start();
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
                    if (PIC.Contains("停止"))
                        thread.Resume();
                    result = false;
                    HomeControl homeControl = new HomeControl(mainWindow);
                    mainWindow.grid.Children.Clear();
                    mainWindow.grid.Children.Add(homeControl);
                }));
            }
        }
        /// <summary>
        /// 扫描停止运行
        /// </summary>
        private ICommand changeCommond { get; set; }
        public ICommand ChangeCommond
        {
            get
            {
                return changeCommond ?? (changeCommond = new DelegateCommand(() =>
                {
                    if (PIC.Contains("扫描"))
                    {
                        thread.Suspend();
                        PIC = "../image/停止.png";
                    }
                    else
                    {
                        PIC = "../image/扫描.png";
                        thread.Resume();
                    }
                }));
            }
        }
        /// <summary>
        /// 模式显示
        /// </summary>
        private string model { get; set; }
        public string Model
        {
            get { return model; }
            set
            {
                model = value;
                this.RaisePropertyChanged(() => Model);
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        private string total { get; set; }
        public string Total
        {
            get { return total; }
            set
            {
                total = value;
                this.RaisePropertyChanged(() => Total);
            }
        }
        /// <summary>
        /// 转换图片
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
        /// 错误提示3秒呼吸
        /// </summary>
        private string error { get; set; }
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                this.RaisePropertyChanged(()=>Error);
            }
        }
        /// <summary>
        /// 列表
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
        public void setError(string data)
        {
            Task.Run(() =>
            {
                Error = data;
                Thread.Sleep(3000);
                Error = "";
            });
        }
    }
}
