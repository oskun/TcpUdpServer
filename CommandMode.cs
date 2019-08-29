using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    /// <summary>
    /// 命令模式
    /// </summary>
    public enum CommandMode
    {

        /// <summary>
        /// 开机
        /// </summary>
        TurnOn,
        /// <summary>
        /// 关机
        /// </summary>
        TurnOff,
        /// <summary>
        /// 调高温度
        /// </summary>
        AdjustUpTemperature,
        /// <summary>
        /// 降低温度
        /// </summary>
        AdjustDownTemperature,
        /// <summary>
        /// 设置温度
        /// </summary>
        SetTemperature,
        /// <summary>
        /// 设置模式
        /// </summary>
        SetMode,
        /// <summary>
        /// 选择频道
        /// </summary>
        SelectChannel,
        /// <summary>
        /// 上一台
        /// </summary>
        AdjustUpChannel,
        /// <summary>
        /// 下一台
        /// </summary>
        AdjustDownChannel,
        /// <summary>
        /// 调小音量
        /// </summary>
        AdjustUpVolume,
        /// <summary>
        /// 增大音量
        /// </summary>
        AdjustDownVolume,
        /// <summary>
        /// 设置静音
        /// </summary>
        SetMute,
        /// <summary>
        /// 暂停
        /// </summary>
        Pause,
        /// <summary>
        /// 继续
        /// </summary>
        Continue,
        /// <summary>
        /// 调低亮度
        /// </summary>
        AdjustUpBrightness,
        /// <summary>
        /// 降高亮度
        /// </summary>
        AdjustDownBrightness,

        /// <summary>
        /// 设置风速
        /// </summary>
        SetWindSpeed

    }
}
