using ArchivesCar.Model;
using ArchivesCar.PublicData;
using ArchivesCar.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ArchivesCar.BLL
{
    public static class RfidBLL
    {
        /// <summary>
        /// 自动以当前检验连接串连接设备
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <param name="hrOut">指针返回请求头 header</param>
        /// <returns>0 成功</returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_Open(string connStr, ref UIntPtr hrOut);
        /// <summary>
        /// 获取连接设备信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 RDR_GetLibVersion(StringBuilder buf, UInt32 nSize);
        /// <summary>
        /// 创建中间衔接
        /// </summary>
        /// <returns>返回衔接协议 InvenParamSpecList</returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UIntPtr RDR_CreateInvenParamSpecList();
        /// <summary>
        /// 寻找EPC
        /// </summary>
        /// <param name="hr">请求头（读卡器句柄）</param>
        /// <param name="AIType">1 恢复扫描 或者 2 重新</param>
        /// <param name="AntennaCoun">0</param>
        /// <param name="AntennaIDs">byte[16]</param>
        /// <param name="InvenParamSpecList">衔接协议</param>
        /// <returns>0 成功</returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_TagInventory(UIntPtr hr, Byte AIType, Byte AntennaCoun, Byte[] AntennaIDs, UIntPtr InvenParamSpecList);
        /// <summary>
        /// 获取Inventory的标签数据报告数据数量
        /// </summary>
        /// <param name="hr">请求头</param>
        /// <returns></returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 RDR_GetTagDataReportCount(UIntPtr hr);
        /// <summary>
        /// 获取Inventory的标签数据报告数据节点。
        /// </summary>
        /// <param name="hr">请求头</param>
        /// <param name="seek">0 不，1 上，2 下，3 最后</param>
        /// <returns></returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UIntPtr RDR_GetTagDataReport(UIntPtr hr, Byte seek);
        /// <summary>
        /// 打开射频输出
        /// </summary>
        /// <param name="hr">请求头</param>
        /// <returns></returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_OpenRFTransmitter(UIntPtr hr);
        /// <summary>
        /// 获取输出端口名称
        /// </summary>
        /// <param name="hr">请求头</param>
        /// <param name="idxOut"></param>
        /// <param name="bufName"></param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_GetOutputName(UIntPtr hr, byte idxOut, StringBuilder bufName, ref UInt32 nSize);
        /// <summary>
        /// 主动断开连接处理
        /// </summary>
        /// <param name="hr">请求头</param>
        /// <returns></returns>
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_ResetCommuImmeTimeout(UIntPtr hr);
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_LoadReaderDrivers(string drvpath);
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_Close(UIntPtr hr);
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 RDR_GetLoadedReaderDriverCount();
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int COMPort_GetEnumItem(UInt32 idx, StringBuilder nameBuf, UInt32 nSize);
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 COMPort_Enum();
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_SetInvenStopTrigger(UIntPtr hInvenParams, Byte stopTriggerType, UInt32 maxTimeout, UInt32 triggerValue);
        [DllImport("rfidlib_aip_iso18000p6C.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UIntPtr ISO18000p6C_CreateInvenParam(UIntPtr hInvenParamSpecList,
                                                        Byte AntennaID /* By default set to 0,apply to all antenna */,
                                                        Byte Sel,
                                                        Byte Session,
                                                        Byte Target,
                                                        Byte Q);
        [DllImport("rfidlib_aip_iso18000p6C.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int ISO18000p6C_SetInvenSelectParam(UIntPtr hIso18000p6CInvenParam,
                                                   Byte target,
                                                    Byte action,
                                                     Byte memBank,
                                                      UInt32 dwPointer,
                                                       Byte[] maskBits,
                                                         Byte maskBitLen,
                                                         Byte truncate);
        [DllImport("rfidlib_aip_iso18000p6C.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int ISO18000p6C_SetInvenReadParam(UIntPtr hIso18000p6CInvenParam, Byte MemBank, UInt32 WordPtr, Byte WordCount);
        [DllImport("rfidlib_aip_iso18000p6C.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern UIntPtr ISO18000p6C_CreateTAWrite(UIntPtr hIso18000p6CInvenParam,
                                                    Byte memBank,
                                                    UInt32 wordPtr,
                                                    UInt32 wordCnt,
                                                    Byte[] pdatas,
                                                    UInt32 nSize);
        [DllImport("rfidlib_aip_iso18000p6C.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int ISO18000p6C_SetInvenMetaDataFlags(UIntPtr hIso18000p6CInvenParam, UInt32 flags);
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_GetLoadedReaderDriverOpt(UInt32 idx, string option, StringBuilder valueBuffer, ref UInt32 nSize);
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_SetCommuImmeTimeout(UIntPtr hr);

        [DllImport("rfidlib_aip_iso18000p6C.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int ISO18000p6C_ParseTagReport(UIntPtr hTagReport,
                                            ref UInt32 aip_id,
                                          ref UInt32 tag_id,
                                          ref UInt32 ant_id,
                                          ref UInt32 metaFlags,
                                          Byte[] tagdata,
                                          ref UInt32 tdLen /* IN:max size of the tagdata buffer ,OUT:bytes written into tagdata buffer */);
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int DNODE_Destroy(UIntPtr dn);
        [DllImport("rfidlib_reader.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int RDR_GetReaderInfor(UIntPtr hr,
                                  Byte Type,
                                  StringBuilder buffer,
                                 ref UInt32 nSize);

    }
    public class wirelessRfid
    {
        public wirelessRfid()
        {
            RfidBLL.RDR_LoadReaderDrivers(AppDomain.CurrentDomain.BaseDirectory + "/Drivers");
        }
        UIntPtr hreader = (UIntPtr)0x00;
        bool _shouldStop = false;
        byte onlyNewTag = 1;
        Thread thread;
        public bool conn()
        {
            hreader = (UIntPtr)0x00;
            int iret;
            UIntPtr InvenParamSpecList = UIntPtr.Zero;
            InvenParamSpecList = RfidBLL.RDR_CreateInvenParamSpecList();
            RfidBLL.RDR_SetInvenStopTrigger(InvenParamSpecList, 3, 1000, 0);
            UIntPtr AIPIso18000p6c = RfidBLL.ISO18000p6C_CreateInvenParam(InvenParamSpecList, 0,
              0, 0, 0x00,
              0xff);

            if (AIPIso18000p6c.ToUInt64() != 0)
            {
                string connstr = "RDType=M60;CommType=COM;COMName=COM8;BaudRate=38400;Frame=8E1;BusAddr=255";
                UInt32 nCOMCnt = RfidBLL.COMPort_Enum();
                for (uint i = 0; i < nCOMCnt; i++)
                {
                    StringBuilder comName = new StringBuilder();
                    comName.Append('\0', 64);
                    RfidBLL.COMPort_GetEnumItem(i, comName, (UInt32)comName.Capacity);
                    connstr = "RDType=M60;CommType=COM;COMName=" + comName + ";BaudRate=38400;Frame=8E1;BusAddr=255";
                    int r = RfidBLL.RDR_Open(connstr, ref hreader);
                    if (r == 0)
                    {
                        StringBuilder devInfor = new StringBuilder();
                        devInfor.Append('\0', 128);
                        UInt32 nSize;
                        nSize = (UInt32)devInfor.Capacity;
                        r = RfidBLL.RDR_GetReaderInfor(hreader, 0, devInfor, ref nSize);
                        if (r == 0)
                        {
                            ServerConfig.connState = true;
                            _shouldStop = false;
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
            return false;
        }
        bool result = true;//user epc 切换判断
        uint p = 0x01;//切换码
        public LoginControl control;
        public int tag_inventory(
                                Byte AIType,
                                 Byte AntennaSelCount,
                                 Byte[] AntennaSel,
                                 ref UInt32 nTagCount)
        {

            int iret;
            UIntPtr InvenParamSpecList = UIntPtr.Zero;
            InvenParamSpecList = RfidBLL.RDR_CreateInvenParamSpecList();
            if (InvenParamSpecList.ToUInt64() != 0)
            {
                /* set timeout */
                RfidBLL.RDR_SetInvenStopTrigger(InvenParamSpecList, 3, 100, 0);
                /* create ISO18000p6C air protocol inventory parameters */
                UIntPtr AIPIso18000p6c = RfidBLL.ISO18000p6C_CreateInvenParam(InvenParamSpecList, 0, 0, 0, 0x00, 0xff);
                if (AIPIso18000p6c.ToUInt64() != 0)
                {
                    if (result)
                        p = 0x01;//epc
                    else
                        p = 0x03;//uid 0x02 tid
                    /* set inventory read parameter for TID */
                    RfidBLL.ISO18000p6C_SetInvenReadParam(AIPIso18000p6c, (Byte)p, 0, 0);
                    /*only TID return */
                    UInt32 metaFlags = 0x20;
                    RfidBLL.ISO18000p6C_SetInvenMetaDataFlags(AIPIso18000p6c, metaFlags);
                }
            }
            nTagCount = 0;
        LABEL_TAG_INVENTORY:
            iret = RfidBLL.RDR_TagInventory(hreader, AIType, AntennaSelCount, AntennaSel, InvenParamSpecList);
            if (iret == 0 || iret == -21)
            {
                nTagCount += RfidBLL.RDR_GetTagDataReportCount(hreader);
                UIntPtr TagDataReport;
                TagDataReport = (UIntPtr)0;
                TagDataReport = RfidBLL.RDR_GetTagDataReport(hreader, 0); //first
                while (TagDataReport.ToUInt64() > 0)
                {
                    UInt32 aip_id = 0;
                    UInt32 tag_id = 0;
                    UInt32 ant_id = 0;
                    Byte[] tagData = new Byte[256];
                    UInt32 nSize = (UInt32)tagData.Length;
                    UInt32 metaFlags = 0;

                    iret = RfidBLL.ISO18000p6C_ParseTagReport(TagDataReport, ref aip_id, ref tag_id, ref ant_id, ref metaFlags, tagData, ref nSize);
                    if (iret == 0)
                    {
                        object[] pList = { aip_id, tag_id, ant_id, metaFlags, tagData, nSize, "", "" };

                        byte[] epc = new byte[12];
                        Array.Copy(tagData, 4, epc, 0, 12);
                        StringBuilder @string = new StringBuilder();
                        foreach (var temp in epc)
                        {
                            @string.Append(temp.ToString("X2"));
                        }
                        if (!ServerConfig.EpcS.Contains(@string.ToString()))
                            ServerConfig.EpcS.Add(@string.ToString());
                        if (@string.ToString().Contains("E200680A8AA8"))//层架判断
                            result = false;
                        else
                            result = true;
                        ///user判断
                        ServerConfig.EpcS.Add(@string.ToString());
                        byte[] user = new byte[28];
                        Array.Copy(tagData, 0, user, 0, 28);
                        @string = new StringBuilder();
                        foreach (var temp in user)
                        {
                            @string.Append(temp.ToString("X2"));
                        }
                        if (@string.ToString().Contains("AA0CFFA5"))
                        {
                            //可用user
                            if (!ServerConfig.UserList.Contains(@string.ToString()))
                                ServerConfig.UserList.Enqueue(@string.ToString());
                            ServerConfig.EpcS.Remove(@string.ToString());
                        }
                    }


                    TagDataReport = RfidBLL.RDR_GetTagDataReport(hreader, 2); //next

                }
                if (iret == -21) // stop trigger occur,need to inventory left tags
                {
                    AIType = 2;//use only-new-tag inventory 
                    goto LABEL_TAG_INVENTORY;
                }
                iret = 0;
            }
            if (InvenParamSpecList.ToUInt64() != 0) RfidBLL.DNODE_Destroy(InvenParamSpecList);
            return iret;
        }
        public void DoInventory()
        {
            int iret;
            Byte AIType = 1;
            if (onlyNewTag == 1)
            {
                AIType = 2;  //only new tag inventory 
            }
            while (!_shouldStop)
            {
                StringBuilder devInfor = new StringBuilder();
                devInfor.Append('\0', 128);
                UInt32 nSize;
                nSize = (UInt32)devInfor.Capacity;
                iret = RfidBLL.RDR_GetReaderInfor(hreader, 0, devInfor, ref nSize);
                if (iret != 0)
                {
                    close();
                    break;
                }
                byte[] AntennaSel = new byte[16];
                UInt32 nTagCount = 0;
                iret = tag_inventory(AIType, 0, AntennaSel, ref nTagCount);
                if (iret == 0)
                {
                    // inventory ok

                }
                else
                {
                    // inventory error 
                }
                AIType = 1;
                if (onlyNewTag == 1)
                {
                    AIType = 2;
                }
            }

            RfidBLL.RDR_ResetCommuImmeTimeout(hreader);
        }
        /// <summary>
        /// 停止扫描
        /// </summary>
        public void stop()
        {
            _shouldStop = true;
            thread.Abort();
            thread = null;
            RfidBLL.RDR_SetCommuImmeTimeout(hreader);
        }
        /// <summary>
        /// 关闭通讯
        /// </summary>
        public void close()
        {
            int i = RfidBLL.RDR_Close(hreader);

            RfidBLL.RDR_ResetCommuImmeTimeout(hreader);
            ServerConfig.connState = false;
            _shouldStop = true;
        }
        /// <summary>
        /// 恢复扫描
        /// </summary>
        public void conset()
        {
            _shouldStop = false;
            thread = new Thread(this.DoInventory);
            thread.Start();
        }
    }
}
