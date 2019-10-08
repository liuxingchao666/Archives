using ArchivesCar.DAL;
using ArchivesCar.Model;
using ArchivesCar.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ArchivesCar.ViewModel
{
    public class UpperShelfViewModel : NotificationObject
    {
        public UpperShelfViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            PIC = "../image/扫描.png";
            PublicData.ServerConfig.userNow = "";
            thread = new Thread(new ThreadStart(() =>
            {
                while (result)
                {
                    if (!PublicData.ServerConfig.connState)
                    {
                        PublicData.ServerConfig.wirelessRfid.conn();
                        PublicData.ServerConfig.wirelessRfid.conset();
                    }
                    lock (PublicData.ServerConfig.EpcS)
                    {
                        var list = (from c in PublicData.ServerConfig.EpcS
                                    where !(from d in PublicData.ServerConfig.UEpcS select d).Contains(c)
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
                    }
                    lock (PublicData.ServerConfig.UserList)
                    {
                        if (isWrite)
                        {
                            if (PublicData.ServerConfig.UserList.Count > 0)
                            {
                                string user = PublicData.ServerConfig.UserList.Dequeue();
                                if (!PublicData.ServerConfig.userNow.Equals(user))
                                {
                                    LocalTion = GetPosition(user);
                                    Rfidstate = Visibility.Visible;
                                    isWrite = false;
                                }
                            }
                        }
                    }
                    Thread.Sleep(300);

                }
                thread.Abort();
                thread = null;
            }))
            {
                IsBackground = true
            };
            Rfidstate = Visibility.Hidden;
            thread.Start();

        }
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
                return fkRegionNum.TrimStart('0') + "区" + colNum.TrimStart('0') + "列" + divNum.TrimStart('0') + "节" + laysNum.TrimStart('0') + "层" + direction.TrimStart('0') + "侧";
            }
            else
            {
                return null;
            }
        }
        MainWindow mainWindow;
        Thread thread;
        bool result = true;
        bool isWrite = true;
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
        /// 启用扫描或者关闭
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
                        PIC = "../image/停止.png";
                    }
                    else
                    {
                        PIC = "../image/扫描.png";
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
                    object errorMsg = null;
                    UpperShelfDAL shelfDAL = new UpperShelfDAL();
                    if (shelfDAL.UpperShelf(ref errorMsg))
                    {
                        ReturnInfo info = errorMsg as ReturnInfo;
                        if (info.TrueOrFalse)
                        {
                            ///
                        }
                        setError(info.Result.ToString());
                    }
                    else
                    {
                        setError(errorMsg.ToString());
                    }
                }));
            }
        }
        /// <summary>
        /// 层架位置
        /// </summary>
        private string localTion { get; set; }
        public string LocalTion
        {
            get { return localTion; }
            set
            {
                localTion = value;
                this.RaisePropertyChanged(() => LocalTion);
            }
        }
        /// <summary>
        /// 恢复停止图标
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
        /// 全选图片显示（或叉）
        /// </summary>
        private string selectStatePIC { get; set; }
        public string SelectStatePIC
        {
            get { return selectStatePIC; }
            set
            {
                selectStatePIC = value;
                this.RaisePropertyChanged(() => SelectStatePIC);
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        private ICommand selectAllCommond { get; set; }
        public ICommand SelectAllCommond
        {
            get
            {
                return selectAllCommond ?? (selectAllCommond = new DelegateCommand(() =>
                {

                }));
            }
        }
        /// <summary>
        /// 单击
        /// </summary>
        private ICommand selectOneCommond { get; set; }
        public ICommand SelectOneCommond
        {
            get
            {
                return selectOneCommond ?? (selectOneCommond = new DelegateCommand(() =>
                {

                }));
            }
        }
        /// <summary>
        /// 单显
        /// </summary>
        private string selectOnePIC { get; set; }
        public string SelectOnePIC
        {
            get { return selectOnePIC; }
            set
            {
                selectOnePIC = value;
                this.RaisePropertyChanged(() => SelectOnePIC);
            }
        }
        void setError(string data)
        {
            Task.Run(() =>
            {
                LocalTion = data;
                Thread.Sleep(3000);
                LocalTion = "";
            });
        }
        private ICommand getRfidCommond { get; set; }
        public ICommand GetRfidCommond
        {
            get
            {
                return getRfidCommond ?? (getRfidCommond = new DelegateCommand(() =>
                {
                    Rfidstate = Visibility.Hidden;
                    isWrite = true;
                }));
            }
        }
        private Visibility rfidstate { get; set; }
        public Visibility Rfidstate
        {
            get { return rfidstate; }
            set
            {
                this.rfidstate = value;
                this.RaisePropertyChanged(() => Rfidstate);
            }
        }
    }
}
