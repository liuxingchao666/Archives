using ArchivesCar.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
    public class wirelessRfid
    {
        UIntPtr hreader = (UIntPtr)0x0000000000000000;
        bool _shouldStop = false;
        byte onlyNewTag = 1;
        Thread thread;
        public bool conn()
        {
            int iret;
            UIntPtr InvenParamSpecList = UIntPtr.Zero;
            InvenParamSpecList = RfidBLL.RDR_CreateInvenParamSpecList();
            RfidBLL.RDR_SetInvenStopTrigger(InvenParamSpecList, 3, 1000, 0);
            UIntPtr AIPIso18000p6c = RfidBLL.ISO18000p6C_CreateInvenParam(InvenParamSpecList, 0,
              0, 0, 0x00,
              0xff);

            if (AIPIso18000p6c.ToUInt64() != 0)
            {
                ArrayList readerDriverInfoList = new ArrayList();

                RfidBLL.RDR_LoadReaderDrivers(AppDomain.CurrentDomain.BaseDirectory + "/Drivers");
                /* enum and show loaded reader driver */
                UInt32 nCount;
                nCount = RfidBLL.RDR_GetLoadedReaderDriverCount();
                uint i;
                for (i = 0; i < nCount; i++)
                {
                    UInt32 nSize;
                    CReaderDriverInf driver = new CReaderDriverInf();
                    StringBuilder strCatalog = new StringBuilder();
                    strCatalog.Append('\0', 64);

                    nSize = (UInt32)strCatalog.Capacity;
                    RfidBLL.RDR_GetLoadedReaderDriverOpt(i, "CATALOG", strCatalog, ref nSize);
                    driver.m_catalog = strCatalog.ToString();
                    if (driver.m_catalog == "Reader") // Only reader we need
                    {
                        StringBuilder strName = new StringBuilder();
                        strName.Append('\0', 64);
                        nSize = (UInt32)strName.Capacity;
                        RfidBLL.RDR_GetLoadedReaderDriverOpt(i, "NAME", strName, ref nSize);
                        driver.m_name = strName.ToString();

                        StringBuilder strProductType = new StringBuilder();
                        strProductType.Append('\0', 64);
                        nSize = (UInt32)strProductType.Capacity;
                        RfidBLL.RDR_GetLoadedReaderDriverOpt(i, "ID", strProductType, ref nSize);
                        driver.m_productType = strProductType.ToString();

                        StringBuilder strCommSupported = new StringBuilder();
                        strCommSupported.Append('\0', 64);
                        nSize = (UInt32)strCommSupported.Capacity;
                        RfidBLL.RDR_GetLoadedReaderDriverOpt(i, "COMM_TYPE_SUPPORTED", strCommSupported, ref nSize);
                        driver.m_commTypeSupported = (UInt32)int.Parse(strCommSupported.ToString());

                        readerDriverInfoList.Add(driver);
                    }
                }
                UInt32 nCOMCnt = RfidBLL.COMPort_Enum();
                for (i = 0; i < nCOMCnt; i++)
                {
                    StringBuilder comName = new StringBuilder();
                    comName.Append('\0', 64);
                    RfidBLL.COMPort_GetEnumItem(i, comName, (UInt32)comName.Capacity);

                }
                string connstr = "RDType=M60;CommType=COM;COMName=COM8;BaudRate=38400;Frame=8E1;BusAddr=255";
                byte[] data = new byte[16];
                int r = RfidBLL.RDR_Open(connstr, ref hreader);
                if (r == 0)
                {
                    thread = new Thread(DoInventory);
                    thread.Start();
                }
                return true;
            }
            return false;
        }

        public delegate void delegate_tag_report_handle(UInt32 AIPType, UInt32 tagType, UInt32 antID, UInt32 metaFlags, Byte[] tagData, UInt32 datlen, String writeOper, String lockOper);
        public void dele_tag_report_handler(UInt32 AIPType, UInt32 tagType, UInt32 antID, UInt32 metaFlags, Byte[] tagData, UInt32 datlen, String writeOper, String lockOper)
        {
            UInt16 epcBitsLen = 0;
            int idx = 0;
            List<Byte> epc;
            List<Byte> readData;
            int i;
            String strAntId;
            strAntId = antID.ToString();
            epc = new List<byte>();
            readData = new List<byte>();
            if (metaFlags == 0) metaFlags |= 0x01;
            if ((metaFlags & 0x01) > 0)
            {
                if (datlen < 2)
                {
                    return;
                }
                epcBitsLen = (UInt16)(tagData[idx] | (tagData[idx + 1] << 8));
                idx += 2;
                int epcBytes = ((epcBitsLen + 7) / 8);
                if ((datlen - idx) < epcBytes)
                {
                    return;
                }
                for (i = 0; i < epcBytes; i++) epc.Add(tagData[idx + i]);
                idx += epcBytes;
            }
            String EPC = BitConverter.ToString(epc.ToArray(), 0, epc.Count).Replace("-", string.Empty) + BitConverter.ToString(readData.ToArray(), 0, readData.Count).Replace("-", string.Empty);
            if (EPC.Contains("E200680A8AA8"))
                result = true;
            else
                result = false;

        }
        bool result = false;
        uint p = 0x01;
        public int tag_inventory(
                                Byte AIType,
                                 Byte AntennaSelCount,
                                 Byte[] AntennaSel,
                                delegate_tag_report_handle tagReportHandler,
                                 ref UInt32 nTagCount)
        {

            int iret;
            UIntPtr InvenParamSpecList = UIntPtr.Zero;
            InvenParamSpecList =RfidBLL.RDR_CreateInvenParamSpecList();
            if (InvenParamSpecList.ToUInt64() != 0)
            {
                /* set timeout */
                RfidBLL.RDR_SetInvenStopTrigger(InvenParamSpecList,3,100, 0);
                /* create ISO18000p6C air protocol inventory parameters */
                UIntPtr AIPIso18000p6c =RfidBLL.ISO18000p6C_CreateInvenParam(InvenParamSpecList, 0, 0, 0, 0x00, 0xff);
                if (AIPIso18000p6c.ToUInt64() != 0)
                {
                    /* set inventory read parameter for TID */
                    RfidBLL.ISO18000p6C_SetInvenReadParam(AIPIso18000p6c, (Byte)p, 0, 0);
                    /*only TID return */
                    UInt32 metaFlags = 0x20;
                    RfidBLL.ISO18000p6C_SetInvenMetaDataFlags(AIPIso18000p6c, 0x20);
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
                       // Invoke(tagReportHandler, pList);
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
            delegate_tag_report_handle cbTagReportHandle;

            cbTagReportHandle = new delegate_tag_report_handle(dele_tag_report_handler);

            int iret;
            Byte AIType =1;
            if (onlyNewTag == 1)
            {
                AIType = 2;  //only new tag inventory 
            }
            while (!_shouldStop)
            {
               byte[] AntennaSel = new byte[16];
                UInt32 nTagCount = 0;
                iret = tag_inventory(AIType, 0, AntennaSel, cbTagReportHandle, ref nTagCount);
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
            object[] pFinishList = { };
           // Invoke(new delegateInventoryFinishCallback(InventoryFinishCallback), pFinishList);
          
            RfidBLL.RDR_ResetCommuImmeTimeout(hreader);
        }
    }
}
