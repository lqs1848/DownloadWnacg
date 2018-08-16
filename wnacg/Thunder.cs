using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace wnacg
{
    class Thunder
    {
        //迅雷下载开放引擎  
        [DllImport("XLDownload.dll", EntryPoint = "XLInitDownloadEngine")]
        public static extern bool XLInitDownloadEngine();
        [DllImport("XLDownload.dll", EntryPoint = "XLUninitDownloadEngine")]
        public static extern bool XLUninitDownloadEngine();
        [DllImport("XLDownload.dll", EntryPoint = "XLURLDownloadToFile", CharSet = CharSet.Auto)]
        public static extern int XLURLDownloadToFile(string pszFileName, string pszUrl, string pszRefUrl, ref Int32 lTaskId);
        [DllImport("XLDownload.dll", EntryPoint = "XLQueryTaskInfo", CharSet = CharSet.Auto)]
        public static extern int XLQueryTaskInfo(int lTaskId, ref int plStatus, ref UInt64 pullFileSize, ref UInt64 pullRecvSize);
        [DllImport("XLDownload.dll", EntryPoint = "XLGetErrorMsg", CharSet = CharSet.Auto)]
        public static extern int XLGetErrorMsg(int dwErrorId, string pszBuffer, ref int dwSize);
        [DllImport("XLDownload.dll", EntryPoint = "XLContinueTask", CharSet = CharSet.Auto)]
        public static extern int XLContinueTask(int lTaskId);
        [DllImport("XLDownload.dll", EntryPoint = "XLContinueTaskFromTdFile", CharSet = CharSet.Auto)]
        public static extern int XLContinueTaskFromTdFile(string pszTdFileFullPath, ref int lTaskId);
        [DllImport("XLDownload.dll", EntryPoint = "XLPauseTask", CharSet = CharSet.Auto)]
        public static extern int XLPauseTask(int lTaskId, ref int lNewTaskId);
        [DllImport("XLDownload.dll", EntryPoint = "XLStopTask", CharSet = CharSet.Auto)]
        public static extern int XLStopTask(int lTaskId);

        public const int TaskStatus_Connect = 0;                // 已经建立连接  
        public const int TaskStatus_Download = 2;                // 开始下载   
        public const int TaskStatus_Pause = 10;                  // 暂停  
        public const int TaskStatus_Success = 11;                // 成功下载  
        public const int TaskStatus_Fail = 12;                  // 下载失败  


        public const int XL_SUCCESS = 0;
        public const int XL_ERROR_FAIL = 0x10000000;

        // 尚未进行初始化  
        public const int XL_ERROR_UNINITAILIZE = XL_ERROR_FAIL + 1;

        // 不支持的协议，目前只支持HTTP  
        public const int XL_ERROR_UNSPORTED_PROTOCOL = XL_ERROR_FAIL + 2;

        // 初始化托盘图标失败  
        public const int XL_ERROR_INIT_TASK_TRAY_ICON_FAIL = XL_ERROR_FAIL + 3;

        // 添加托盘图标失败  
        public const int XL_ERROR_ADD_TASK_TRAY_ICON_FAIL = XL_ERROR_FAIL + 4;

        // 指针为空  
        public const int XL_ERROR_POINTER_IS_NULL = XL_ERROR_FAIL + 5;

        // 字符串是空串  
        public const int XL_ERROR_STRING_IS_EMPTY = XL_ERROR_FAIL + 6;

        // 传入的路径没有包含文件名  
        public const int XL_ERROR_PATH_DONT_INCLUDE_FILENAME = XL_ERROR_FAIL + 7;

        // 创建目录失败  
        public const int XL_ERROR_CREATE_DIRECTORY_FAIL = XL_ERROR_FAIL + 8;

        // 内存不足  
        public const int XL_ERROR_MEMORY_ISNT_ENOUGH = XL_ERROR_FAIL + 9;

        // 参数不合法  
        public const int XL_ERROR_INVALID_ARG = XL_ERROR_FAIL + 10;

        // 任务不存在  
        public const int XL_ERROR_TASK_DONT_EXIST = XL_ERROR_FAIL + 11;

        // 文件名不合法  
        public const int XL_ERROR_FILE_NAME_INVALID = XL_ERROR_FAIL + 12;

        // 没有实现  
        public const int XL_ERROR_NOTIMPL = XL_ERROR_FAIL + 13;

        // 已经创建的任务数达到最大任务数，无法继续创建任务  
        public const int XL_ERROR_TASKNUM_EXCEED_MAXNUM = XL_ERROR_FAIL + 14;

        // 任务类型未知  
        public const int XL_ERROR_INVALID_TASK_TYPE = XL_ERROR_FAIL + 15;

        // 文件已经存在  
        public const int XL_ERROR_FILE_ALREADY_EXIST = XL_ERROR_FAIL + 16;

        // 文件不存在  
        public const int XL_ERROR_FILE_DONT_EXIST = XL_ERROR_FAIL + 17;

        // 读取cfg文件失败  
        public const int XL_ERROR_READ_CFG_FILE_FAIL = XL_ERROR_FAIL + 18;

        // 写入cfg文件失败  
        public const int XL_ERROR_WRITE_CFG_FILE_FAIL = XL_ERROR_FAIL + 19;

        // 无法继续任务，可能是不支持断点续传，也有可能是任务已经失败  
        // 通过查询任务状态，确定错误原因。  
        public const int XL_ERROR_CANNOT_CONTINUE_TASK = XL_ERROR_FAIL + 20;

        // 无法暂停任务，可能是不支持断点续传，也有可能是任务已经失败  
        // 通过查询任务状态，确定错误原因。  
        public const int XL_ERROR_CANNOT_PAUSE_TASK = XL_ERROR_FAIL + 21;

        // 缓冲区太小  
        public const int XL_ERROR_BUFFER_TOO_SMALL = XL_ERROR_FAIL + 22;

        // 调用XLInitDownloadEngine的线程，在调用XLUninitDownloadEngine之前已经结束。  
        // 初始化下载引擎线程，在调用XLUninitDownloadEngine之前，必须保持执行状态。  
        public const int XL_ERROR_INIT_THREAD_EXIT_TOO_EARLY = XL_ERROR_FAIL + 23;

        // TP崩溃  
        public const int XL_ERROR_TP_CRASH = XL_ERROR_FAIL + 24;

        // 任务不合法，调用XLContinueTaskFromTdFile继续任务。内部任务切换失败时，会产生这个错误。  
        public const int XL_ERROR_TASK_INVALID = XL_ERROR_FAIL + 25;
    }
} 

