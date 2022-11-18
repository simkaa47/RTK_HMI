using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTK_HMI.Services
{
    internal static class RecognizeParameterFromArrService
    {
        public static void Recongnize(IEnumerable<Parameter> parameters,int[] buf, int offset)
        {
            var ushortArr = buf.Select(i => (ushort)i).ToArray();
            foreach (var param in parameters)
            {
                switch (param.Type)
                {
                    case DataType.String:
                        param.Value = GetStringFromUshorts(ushortArr, param.RegNum - offset, param.Length);
                        break;
                    case DataType.Float32:
                        param.Value = GetFloatFromUshorts(ushortArr, param.RegNum - offset);
                        break;
                    case DataType.Int32:
                        param.Value = GetInt32FromUshorts(ushortArr, param.RegNum - offset);
                        break;
                    case DataType.Uint32:
                        param.Value = GetUInt32FromUshorts(ushortArr, param.RegNum - offset);
                        break;
                    case DataType.Int16:
                        param.Value = GetShortFromUshorts(ushortArr, param.RegNum - offset);
                        break;
                    case DataType.Uint16:
                        param.Value = GetUshortFromUshorts(ushortArr, param.RegNum - offset);
                        break;
                    default:
                        break;
                }
            }




        }

        #region Получить float из 2х регистров
        static float GetFloatFromUshorts(ushort[] regs, int regNum)
        {
            ushort[] arr = new ushort[] { regs[regNum], regs[regNum+1] };
            var bytes = arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray();
            return BitConverter.ToSingle(bytes, 0);
        }
        #endregion

        #region Получить uint32 из 2х регистров
        static uint GetUInt32FromUshorts(ushort[] regs, int regNum)
        {
            try
            {
                ushort[] arr = new ushort[] { regs[regNum], regs[regNum+1] };
                return BitConverter.ToUInt32(arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray(), 0);
            }
            catch (Exception)
            {
                return 0;
            }


        }
        #endregion

        #region Получить int32 из 2х регистров
        static int GetInt32FromUshorts(ushort[] regs, int regNum)
        {
            try
            {
                ushort[] arr = new ushort[] { regs[regNum], regs[regNum + 1] };
                return BitConverter.ToInt32(arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray(), 0);
            }
            catch (Exception)
            {
                return 0;
            }


        }
        #endregion

        #region Получить строку из байт
        static string GetStringFromUshorts(ushort[] regs, int regNum, int lenght)
        {
            
            var bytes = regs.Skip(regNum)
                .SelectMany(num => BitConverter.GetBytes(num).Reverse())
                .Take(lenght)
                .ToArray();
            var name = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            return name;
        }
        #endregion

        #region Получить ushort из байт
        static ushort GetUshortFromUshorts(ushort[] regs, int regNum)
        {
            ushort temp = regs[regNum];
            return BitConverter.ToUInt16(BitConverter.GetBytes(temp).Reverse().ToArray(), 0);
        }
        #endregion

        #region Получить short из байт
        static short GetShortFromUshorts(ushort[] regs, int regNum)
        {
            ushort temp = regs[regNum];
            return BitConverter.ToInt16(BitConverter.GetBytes(temp).Reverse().ToArray(), 0);
        }
        #endregion

        #region Получить регистры
        public static ushort[] GetRegisters(Parameter parameter)
        {
            switch (parameter.Type)
            {
                case DataType.String:
                    if(parameter.Value is null)return new ushort[parameter.Length%2==0?parameter.Length/2:parameter.Length/2+1];
                    return GetRegistersFromString(parameter.Value.ToString(), parameter.Length);
                case DataType.Float32:
                    if (parameter.Value is null) return new ushort[2];
                    return GetRegsFromFloat((float)parameter.Value);
                case DataType.Int32:
                    if (parameter.Value is null) return new ushort[2];
                    return GetRegsFromInt32((int)parameter.Value);
                case DataType.Uint32:
                    if (parameter.Value is null) return new ushort[2];
                    return GetRegsFromUInt32((uint)parameter.Value);
                case DataType.Int16:
                    return new ushort[] { BitConverter.ToUInt16(BitConverter.GetBytes((short)parameter.Value).Reverse().ToArray()) };
                case DataType.Uint16:
                    return new ushort[] { BitConverter.ToUInt16(BitConverter.GetBytes((ushort)parameter.Value).Reverse().ToArray()) };
                default:
                    break;
            }
            return new ushort[2];
        }
        #endregion

        #region Получить регистры из строки
        static ushort[] GetRegistersFromString(string value, int length)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);             
            var bytes = Encoding.GetEncoding(1251).GetBytes(value).Take(length).ToArray();
            var arr = new byte[length];
            bytes.CopyTo(arr, 0);
            var nums = new ushort[length % 2 == 0 ? length / 2 : length / 2 + 1];
            for (int i = 0; i < bytes.Length; i += 2)
            {
                nums[i / 2] = BitConverter.ToUInt16(arr, i);
            }
            var regs = nums.Select(num => (ushort)((num / 256) + (ushort)(num << 8))).ToArray();
            return regs;
        }
        #endregion

        #region Получить регистры из float
        static ushort[] GetRegsFromFloat(float value)
        {
            var output = new ushort[2];
            var bytes = BitConverter.GetBytes(value).ToArray();
            for (int i = 0; i < 4; i+=2)
            {
                var reverse = new byte[] {bytes[i+1], bytes[i]};
                output[i/2] = BitConverter.ToUInt16(reverse, 0);
            }
            return output;
;        }
        #endregion

        #region Получить регистры из Int32
        static ushort[] GetRegsFromInt32(int value)
        {
            var output = new ushort[2];
            var bytes = BitConverter.GetBytes(value).ToArray();
            for (int i = 0; i < 4; i += 2)
            {
                var reverse = new byte[] { bytes[i + 1], bytes[i] };
                output[i / 2] = BitConverter.ToUInt16(reverse, 0);
            }
            return output;            
        }
        #endregion

        #region Получить регистры из UInt32
        static ushort[] GetRegsFromUInt32(uint value)
        {
            var output = new ushort[2];
            var bytes = BitConverter.GetBytes(value).ToArray();
            for (int i = 0; i < 4; i += 2)
            {
                var reverse = new byte[] { bytes[i + 1], bytes[i] };
                output[i / 2] = BitConverter.ToUInt16(reverse, 0);
            }
            return output;
        }
        #endregion


    }
}
