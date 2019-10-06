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
using System.Windows.Controls;
using System.Windows.Input;

namespace ArchivesCar.ViewModel
{
     public class LoginViewModel : NotificationObject
    {
        public LoginViewModel(MainWindow window)
        {
            mainWindow = window;
        }
        MainWindow mainWindow;
        /// <summary>
        /// 设置
        /// </summary>
        private ICommand setCommond { get; set; }
        public ICommand SetCommond
        {
            get
            {
                return setCommond ?? (setCommond = new DelegateCommand(() => {
                    SetControl setControl = new SetControl(mainWindow);
                    mainWindow.grid.Children.Clear();
                    mainWindow.grid.Children.Add(setControl);
                }));
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        private ICommand okCommond { get; set; }
        public ICommand OkCommond
        {
            get
            {
                return okCommond ?? (okCommond=new DelegateCommand<PasswordBox>((data)=> {
                    IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(data.SecurePassword);
                    string password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
                    if (string.IsNullOrEmpty(password))
                    {
                        setError("登录密码不能为空");
                        return;
                    }
                    if (string.IsNullOrEmpty(Account))
                    {
                        setError("登录账户不能为空");
                        return;
                    }
                    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                    keyValuePairs.Add("account",Account);
                    keyValuePairs.Add("password",password);
                    object errorMsg = keyValuePairs;
                    LoginDAL loginDAL = new LoginDAL();
                    if (loginDAL.login(ref errorMsg))
                    {
                        ReturnInfo info = errorMsg as ReturnInfo;
                        if (info.TrueOrFalse)
                        {
                            HomeControl homeControl = new HomeControl(mainWindow);
                            mainWindow.grid.Children.Clear();
                            mainWindow.grid.Children.Add(homeControl);
                        }
                        else
                        {
                            setError(info.Result.ToString());
                        }
                    }
                    else
                    {
                        setError(errorMsg.ToString());
                    }
                }));
            }
        }
        /// <summary>
        /// 用户
        /// </summary>
        private string account { get; set; }
        public string Account
        {
            get { return account; }
            set
            {
                this.account = value;
                this.RaisePropertyChanged(() => Account);
            }
        }
        /// <summary>
        /// 错误
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
