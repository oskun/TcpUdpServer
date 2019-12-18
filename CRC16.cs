using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpUdpServer
{
    public class CRC16
    {

        /*
         public static int CRC_XModem(byte[] bytes){
        int crc = 0x00;          // initial value
        int polynomial = 0x1021;
        for (int index = 0 ; index< bytes.length; index++) {
            byte b = bytes[index];
            for (int i = 0; i < 8; i++) {
                boolean bit = ((b   >> (7-i) & 1) == 1);
                boolean c15 = ((crc >> 15    & 1) == 1);
                crc <<= 1;
                if (c15 ^ bit) crc ^= polynomial;
            }
        }
        crc &= 0xffff;
        return crc;
    }


             */



        /// <summary>
        /// 生成CRC码
        /// </summary>
        /// <param name="message">发送或返回的命令，CRC码除外</param>
        /// <param name="CRC">生成的CRC码</param>
        private static void GetCRC(byte[] message, ref byte[] CRC)
        {
            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;
            for (int i = 0; i < message.Length - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);
                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    //下面两句所得结果一样
                    //CRCFull = (ushort)(CRCFull >> 1);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);
                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }



        public static byte[] CRC_RTU(string hexstr)
        {
            var bytes = StrHelper.strToHexByte(hexstr);
            byte[] crcBytes = new byte[2];
            byte[] sendResponse = new byte[bytes.Length + crcBytes.Length];
            bytes.CopyTo(sendResponse, 0);
            CRC16.GetCRC(sendResponse, ref crcBytes);
            sendResponse[sendResponse.Length - 1] = crcBytes[1];//高8位
            sendResponse[sendResponse.Length - 2] = crcBytes[0];//低8位
            return sendResponse;


        }



        public static string CRC16Data(string hexstr)
        {
            return string.Empty;
        }



        public static byte[] CRC_XModem(string hexstr)
        {

            byte[] bytes = CRC16.strToHexByte(hexstr);
            int crc = 0x00;
            int polynomial = 0x1021;
            for (int index = 0; index < bytes.Length; index++)
            {
                byte b = bytes[index];
                for (int i = 0; i < 8; i++)
                {
                    bool bit = ((b >> (7 - i) & 1) == 1);
                    bool c15 = ((crc >> 15 & 1) == 1);
                    crc <<= 1;
                    if (c15 ^ bit) crc ^= polynomial;
                }
            }
            var bs = intTo2Bytes(crc);
            var list = bytes.ToList();
            list.AddRange(bs);
            return list.ToArray();

        }


        private static byte[] intTo2Bytes(int value)
        {
            byte[] src = new byte[2];
            src[0] = (byte)((value >> 8) & 0xFF);
            src[1] = (byte)(value & 0xFF);
            return src;
        }


        #region 计算CRC校验码
        /// <summary>
        /// 计算CRC校验码
        /// Cyclic Redundancy Check 循环冗余校验码
        /// 是数据通信领域中最常用的一种差错校验码
        /// 特征是信息字段和校验字段的长度可以任意选定
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] CRC16_C(byte[] data)
        {
            byte num = 0xff;
            byte num2 = 0xff;

            byte num3 = 1;
            byte num4 = 160;
            byte[] buffer = data;

            for (int i = 0; i < buffer.Length; i++)
            {
                //位异或运算
                num = (byte)(num ^ buffer[i]);

                for (int j = 0; j <= 7; j++)
                {
                    byte num5 = num2;
                    byte num6 = num;

                    //位右移运算
                    num2 = (byte)(num2 >> 1);
                    num = (byte)(num >> 1);

                    //位与运算
                    if ((num5 & 1) == 1)
                    {
                        //位或运算
                        num = (byte)(num | 0x80);
                    }
                    if ((num6 & 1) == 1)
                    {
                        num2 = (byte)(num2 ^ num4);
                        num = (byte)(num ^ num3);
                    }
                }
            }
            return new byte[] { num, num2 };
        }
        /// <summary>
        /// 转换为16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToHexByte(string hexString)
        {

            hexString = hexString.Replace(" ", "");
            //判断奇偶位数
            if ((hexString.Length % 2) != 0)
            {
                hexString = hexString + "0";
            }
            //声明字符串变量
            byte[] buffer = new byte[hexString.Length / 2];

            for (int i = 0; i < buffer.Length; i++)
            {
                //将指定基的数字的字符串表示形式转换为等效的8位无符号整数
                //即将字符串转换为16进制形式的字节
                var c = hexString.Substring(i * 2, 2);
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 0x10);
            }
            return buffer;
        }



        /// <summary>
        /// 得到发送的数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] getSendByte(string hexString)
        {
            var bytes = strToHexByte(hexString);
            var crc = new CRC16();
            var crccode = crc.CRC16_C(bytes);
            var finaldata = bytes.Concat(crccode).ToArray();
            return finaldata;

        }
        #endregion

    }
}
