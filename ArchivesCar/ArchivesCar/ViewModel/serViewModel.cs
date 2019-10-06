using ArchivesCar.BLL;
using ArchivesCar.DAL;
using ArchivesCar.Model;
using ArchivesCar.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArchivesCar.ViewModel
{
     public class serViewModel : NotificationObject
    {
        public serViewModel(MainWindow window)
        {
            mainWindow = window;
            ServerIP = ConfigurationManager.AppSettings["ServerIP"];
            ServerPort = ConfigurationManager.AppSettings["ServerPort"];
            string roomId= ConfigurationManager.AppSettings["RoomId"];
            Task.Run(()=> {
                object errorMsg = null;
                GetStorageRoomsDAL roomsDAL = new GetStorageRoomsDAL();
                if (roomsDAL.GetStorageRoom(ref errorMsg))
                {
                    ReturnInfo returnInfo = errorMsg as ReturnInfo;
                    if (returnInfo.TrueOrFalse)
                    {
                        List = returnInfo.Result as List<StorageRoomInfo>;
                        foreach (var temp in List)
                        {
                            if (temp.Id == roomId)
                            {
                                SelectRoom = temp;
                                selectName = temp.Name;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Error = returnInfo.Result.ToString();
                    }
                }
                else
                {
                    Error = errorMsg.ToString();
                }
            });
        }
        MainWindow mainWindow;
        /// <summary>
        /// 服务器ip
        /// </summary>
        private string serverIP { get; set; }
        public string ServerIP
        {
            get { return serverIP; }
            set
            {
                this.serverIP = value;
                this.RaisePropertyChanged(()=>ServerIP);
            }
        }
        /// <summary>
        /// 端口
        /// </summary>
        private string serverPort { get; set; }
        public string ServerPort
        {
            get { return serverPort; }
            set
            {
                this.serverPort = value;
                this.RaisePropertyChanged(() => ServerPort);
            }
        }
        /// <summary>
        /// 库房获取列表
        /// </summary>
        private List<StorageRoomInfo> list { get; set; } = new List<StorageRoomInfo>();
        public List<StorageRoomInfo> List
        {
            get { return list; }
            set
            {
                this.list = value;
                this.RaisePropertyChanged(()=>List);
            }
        }
        /// <summary>
        /// 选中库房（默认）
        /// </summary>
        private StorageRoomInfo selectRoom { get; set; } = new StorageRoomInfo();
        public StorageRoomInfo SelectRoom
        {
            get { return selectRoom; }
            set
            {
                this.selectRoom = value;
                this.RaisePropertyChanged(()=>SelectRoom);
            }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        private string error { get; set; }
        public string Error
        {
            get { return error; }
            set
            {
                this.error = value;
                this.RaisePropertyChanged(()=>Error);
            }
        }
        /// <summary>
        /// 当前库房名称
        /// </summary>
        private string SelectName { get; set; }
        public string selectName
        {
            get { return SelectName; }
            set
            {
                this.SelectName = value;
                this.RaisePropertyChanged(() => selectName);
            }
        }
        /// <summary>
        /// 返回上一页
        /// </summary>
        private ICommand backCommond { get; set; }
        public ICommand BackCommond
        {
            get
            {
                return backCommond ?? (backCommond=new DelegateCommand(()=> {
                    LoginControl loginControl = new LoginControl(mainWindow);
                    mainWindow.grid.Children.Clear();
                    mainWindow.grid.Children.Add(loginControl);
                }));
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        private ICommand okCommond { get; set; }
        public ICommand OkCommond
        {
            get
            {
                return okCommond ?? (okCommond=new DelegateCommand(()=> {
                    if (string.IsNullOrEmpty(ServerIP))
                    {
                        setError("服务器ip,不能为空");
                        return;
                    }
                    if (!Regex.IsMatch(ServerIP, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$"))
                    {
                        setError("服务器ip地址设置格式有误");
                        return;
                    }
                    if (string.IsNullOrEmpty(ServerPort))
                    {
                        setError("端口不可为空");
                        return;
                    }
                    if (SelectRoom == null && string.IsNullOrEmpty(SelectRoom.Id))
                    {
                        setError("未选择库房");
                        return;
                    }
                    string url = string.Format(@"http://{0}:{1}/storeroom/countingcar/selAllStore",ServerIP,ServerPort);
                    Http http = new Http(url,null);
                    try
                    {
                        var result = JToken.Parse(http.HttpGet(url));
                    
                        if (result["state"].ToString().ToLower().Equals("true"))
                        {
                            PublicData.ServerConfig.serverIP = ServerIP;
                            PublicData.ServerConfig.serverPort = ServerPort;
                            PublicData.ServerConfig.roomId = SelectRoom.Id;
                            Task.Run(()=> {
                                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                                cfa.AppSettings.Settings["ServerIP"].Value = ServerIP;
                                cfa.AppSettings.Settings["ServerPort"].Value = ServerPort;
                                cfa.AppSettings.Settings["RoomId"].Value = SelectRoom.Id;
                                cfa.AppSettings.Settings["RoomName"].Value = SelectRoom.Name;
                                cfa.Save(ConfigurationSaveMode.Modified);
                                ConfigurationManager.RefreshSection("appSettings");
                            });
                            ///
                            LoginControl loginControl = new LoginControl(mainWindow);
                            mainWindow.grid.Children.Clear();
                            mainWindow.grid.Children.Add(loginControl);
                        }
                        else
                        {
                            setError("连接服务器失败");
                        }
                    }
                    catch
                    {
                        setError("连接服务器失败");
                    }
                }));
            }
        }
        public void setError(string data)
        {
            Task.Run(() => {
                Error = data;
                Thread.Sleep(3000);
                Error = "";
            });
        }
    }
}
