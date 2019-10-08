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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArchivesCar.ViewModel
{
    public class BindViewModel : NotificationObject
    {
        public BindViewModel(MainWindow mainWindow)
        {
            PublicData.ServerConfig.EpcList = new Queue<string>();
            this.mainWindow = mainWindow;
            thread = new Thread(new ThreadStart(() =>
            {
                while (result)
                {
                    if (!PublicData.ServerConfig.connState)
                    {
                        PublicData.ServerConfig.wirelessRfid.conn();
                        PublicData.ServerConfig.wirelessRfid.conset();
                    }
                    lock (PublicData.ServerConfig.EpcList)
                    {
                        if (PublicData.ServerConfig.EpcList.Count > 0)
                        {
                            EPC = PublicData.ServerConfig.EpcList.Dequeue();
                            Rfidstate = Visibility.Visible;
                            thread.Suspend();
                        }
                    }
                    Thread.Sleep(300);
                }
                thread.Abort();
                thread = null;
            }));
            thread.IsBackground = true;
            thread.Start();
            State = Visibility.Visible;
            Rfidstate = Visibility.Hidden;
        }
        Thread thread;
        bool result = true;
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
                    if (PublicData.ServerConfig.connState && !PublicData.ServerConfig.wirelessRfid._shouldStop)
                        PublicData.ServerConfig.wirelessRfid.stop();
                    HomeControl homeControl = new HomeControl(mainWindow);
                    mainWindow.grid.Children.Clear();
                    mainWindow.grid.Children.Add(homeControl);
                }));
            }
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        private string queryText { get; set; }
        public string QueryText
        {
            get { return queryText; }
            set
            {
                queryText = value;
                this.RaisePropertyChanged(() => QueryText);
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
                    if (string.IsNullOrEmpty(QueryText))
                    {
                        return;
                    }
                    query(QueryText);
                }));
            }
        }
        /// <summary>
        /// 绑定
        /// </summary>
        private ICommand bindCommond { get; set; }
        public ICommand BindCommond
        {
            get
            {
                return bindCommond ?? (bindCommond = new DelegateCommand<DataGrid>((data) =>
                {
                    if (data.SelectedIndex < 0) {
                        Color = "Red";
                        setMsg("未选中档案");
                        return;
                    }
                    if (string.IsNullOrEmpty(EPC)) {
                        Color = "Red";
                        setMsg("没有可用RFID");
                        return;
                    }
                    InventoryInfo info = data.SelectedItem as InventoryInfo;
                    Dictionary<string, object> valuePairs = new Dictionary<string, object>();
                    valuePairs.Add("rfid",EPC);
                    valuePairs.Add("fileId", info.fkFileId);
                    object errorMsg = valuePairs;
                    BindRFIDDAL bindRFIDDAL = new BindRFIDDAL();
                    if (bindRFIDDAL.BindRFID(ref errorMsg))
                    {
                        ReturnInfo returnInfo = errorMsg as ReturnInfo;
                        if (returnInfo.TrueOrFalse)
                        {
                            foreach (var temp in List)
                            {
                                if (temp.fkFileId == info.fkFileId)
                                {
                                    temp.rfid = EPC;
                                }
                            }
                            setMsg("绑定成功");
                           // query(QueryText);
                            EPC = "";
                        }
                        else
                        {
                            Color = "Red";
                            setMsg(returnInfo.Result.ToString());
                        }
                    }
                    else
                    {
                        Color = "Red";
                        setMsg(errorMsg.ToString());
                    }
                }));
            }
        }
        /// <summary>
        /// 读取RFID
        /// </summary>
        private ICommand bindRfid { get; set; }
        public ICommand QueryRfid
        {
            get
            {
                return bindRfid ?? (bindRfid = new DelegateCommand(() =>
                {
                    EPC = "";
                    Rfidstate = Visibility.Hidden;
                    thread.Resume();
                }));
            }
        }
        /// <summary>
        /// epc
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
        /// 数据集
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
        /// 操作提示
        /// </summary>
        private string msg { get; set; }
        public string Msg
        {
            get { return msg; }
            set
            {
                msg = value;
                this.RaisePropertyChanged(()=>Msg);
            }
        }
        /// <summary>
        /// 颜色提示
        /// </summary>
        private string color { get; set; }
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                this.RaisePropertyChanged(()=>Color);
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        private Visibility state { get; set; }
        public Visibility State
        {
            get { return state; }
            set
            {
                state = value;
                this.RaisePropertyChanged(()=>State);
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        private Visibility rfidstate { get; set; }
        public Visibility Rfidstate
        {
            get { return rfidstate; }
            set
            {
                rfidstate = value;
                this.RaisePropertyChanged(() => Rfidstate);
            }
        }
        void setMsg(string data)
        {
            Task.Run(()=> {
                Msg = data;
                Thread.Sleep(3000);
                Msg = "";
            });
        }
        public void query(string queryText)
        {
            object errorMsg = queryText;
            GetSelArcByFileNameDAL byFileNameDAL = new GetSelArcByFileNameDAL();
            if (byFileNameDAL.GetSelArcByFileName(ref errorMsg))
            {
                ReturnInfo returnInfo = errorMsg as ReturnInfo;
                if (returnInfo.TrueOrFalse)
                {
                    List = returnInfo.Result as List<InventoryInfo>;
                }
                else
                {
                    setMsg(returnInfo.Result.ToString());
                    Color = "Red";
                }
            }
            else
            {
                setMsg(errorMsg.ToString());
                Color = "Red";
            }
        }
    }
}
