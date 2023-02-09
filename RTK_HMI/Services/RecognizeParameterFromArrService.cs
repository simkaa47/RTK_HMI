using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

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
                        param.Value = GetFloatFromUshorts(ushortArr, param.RegNum - offset, param.Order);
                        break;
                    case DataType.Int32:
                        param.Value = GetInt32FromUshorts(ushortArr, param.RegNum - offset, param.Order);
                        break;
                    case DataType.Uint32:
                        param.Value = GetUInt32FromUshorts(ushortArr, param.RegNum - offset, param.Order);
                        break;
                    case DataType.Int16:
                        param.Value = GetShortFromUshorts(ushortArr, param.RegNum - offset, param.Order);
                        break;
                    case DataType.Uint16:
                        param.Value = GetUshortFromUshorts(ushortArr, param.RegNum - offset, param.Order);
                        break;
                    default:
                        break;
                }
            }

        }

        #region Получить float из 2х регистров
        static float GetFloatFromUshorts(ushort[] regs, int regNum, ByteOrder order)
        {
            ushort[] arr = null;
            byte[] bytes = null;
            switch (order)
            {
                case ByteOrder.ABCD:
                    arr = new ushort[] { regs[regNum], regs[regNum+1] };
                    bytes = arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray();
                    break;
                case ByteOrder.CDAB:
                    arr = new ushort[] { regs[regNum + 1], regs[regNum] };
                    bytes = arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray();
                    break;
                case ByteOrder.BADC:
                    arr = new ushort[] { regs[regNum], regs[regNum + 1] };
                    bytes = arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray();
                    break;
                case ByteOrder.DCBA:
                    arr = new ushort[] { regs[regNum + 1], regs[regNum] };
                    bytes = arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray();
                    break;
                default:
                    break;
            }

            
            return BitConverter.ToSingle(bytes, 0);
        }
        #endregion

        #region Получить uint32 из 2х регистров
        static uint GetUInt32FromUshorts(ushort[] regs, int regNum, ByteOrder order)
        {
            ushort[] arr = null;
            try
            {
                switch (order)
                {
                    case ByteOrder.ABCD:
                        arr = new ushort[] { regs[regNum], regs[regNum + 1] };
                        return BitConverter.ToUInt32(arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray(), 0);
                    case ByteOrder.CDAB:
                        arr = new ushort[] { regs[regNum+1], regs[regNum] };
                        return BitConverter.ToUInt32(arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray(), 0);
                    case ByteOrder.BADC:
                        arr = new ushort[] { regs[regNum], regs[regNum + 1] };
                        return BitConverter.ToUInt32(arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray(), 0);
                    case ByteOrder.DCBA:
                        arr = new ushort[] { regs[regNum + 1], regs[regNum] };
                        return BitConverter.ToUInt32(arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray(), 0);
                }
                return 0;
                
            }
            catch (Exception)
            {
                return 0;
            }


        }
        #endregion

        #region Получить int32 из 2х регистров
        static int GetInt32FromUshorts(ushort[] regs, int regNum, ByteOrder order)
        {
            ushort[] arr = null;
            try
            {
                switch (order)
                {
                    case ByteOrder.ABCD:
                        arr = new ushort[] { regs[regNum], regs[regNum + 1] };
                        return BitConverter.ToInt32(arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray(), 0);
                    case ByteOrder.CDAB:
                        arr = new ushort[] { regs[regNum + 1], regs[regNum] };
                        return BitConverter.ToInt32(arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray(), 0);
                    case ByteOrder.BADC:
                        arr = new ushort[] { regs[regNum], regs[regNum + 1] };
                        return BitConverter.ToInt32(arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray(), 0);
                    case ByteOrder.DCBA:
                        arr = new ushort[] { regs[regNum + 1], regs[regNum] };
                        return BitConverter.ToInt32(arr.SelectMany(num => BitConverter.GetBytes(num).Reverse()).ToArray(), 0);
                }
                return 0;
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
        static ushort GetUshortFromUshorts(ushort[] regs, int regNum, ByteOrder order)
        {
            ushort temp = regs[regNum];
            switch (order)
            {
                case ByteOrder.ABCD:
                case ByteOrder.CDAB:
                    return BitConverter.ToUInt16(BitConverter.GetBytes(temp).ToArray(), 0);
                case ByteOrder.BADC:
                case ByteOrder.DCBA:
                    return BitConverter.ToUInt16(BitConverter.GetBytes(temp).Reverse().ToArray(), 0);
                default:
                    break;
            }
            return 0;
        }
        #endregion

        #region Получить short из байт
        static short GetShortFromUshorts(ushort[] regs, int regNum, ByteOrder order)
        {
            ushort temp = regs[regNum];
            switch (order)
            {
                case ByteOrder.ABCD:                    
                case ByteOrder.CDAB:
                    return BitConverter.ToInt16(BitConverter.GetBytes(temp).ToArray(), 0);
                case ByteOrder.BADC:                   
                case ByteOrder.DCBA:
                    return BitConverter.ToInt16(BitConverter.GetBytes(temp).Reverse().ToArray(), 0);
                default:
                    break;
            }
            return 0;
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
                    return GetRegsFromFloat((float)parameter.Value, parameter.Order);
                case DataType.Int32:
                    if (parameter.Value is null) return new ushort[2];
                    return GetRegsFromInt32((int)parameter.Value, parameter.Order);
                case DataType.Uint32:
                    if (parameter.Value is null) return new ushort[2];
                    return GetRegsFromUInt32((uint)parameter.Value, parameter.Order);
                case DataType.Int16:
                    if (parameter.Value is null) return new ushort[2];
                    return GetRegsFromShort((short)parameter.Value, parameter.Order);
                case DataType.Uint16:
                    if (parameter.Value is null) return new ushort[2];
                    return GetRegsFromUshort((ushort)parameter.Value, parameter.Order);
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
        static ushort[] GetRegsFromFloat(float value, ByteOrder order)
        {
            var bytes = BitConverter.GetBytes(value).ToArray();
            return GetRegistersFromBytes(bytes, order);                  
        }
        #endregion

        #region Получить регистры из Int32
        static ushort[] GetRegsFromInt32(int value, ByteOrder order)
        {            
            var bytes = BitConverter.GetBytes(value).ToArray();
            return GetRegistersFromBytes(bytes, order);            
        }
        #endregion

        #region Получить регистры из UInt32
        static ushort[] GetRegsFromUInt32(uint value, ByteOrder order)
        {
            var bytes = BitConverter.GetBytes(value).ToArray();
            return GetRegistersFromBytes(bytes, order);
        }
        #endregion

        #region Получить регистры из short
        static ushort[] GetRegsFromShort(short value, ByteOrder order)
        {
            var bytes = BitConverter.GetBytes(value).ToArray();
            return GetRegistersFromBytes(bytes, order);            
        }
        #endregion

        #region Получить регистры из ushort
        static ushort[] GetRegsFromUshort(ushort value, ByteOrder order)
        {
            var bytes = BitConverter.GetBytes(value).ToArray();
            return GetRegistersFromBytes(bytes, order);
        }
        #endregion

        #region Получить регистры из байт
        private static ushort[] GetRegistersFromBytes(byte[] arr, ByteOrder order)
        {
            var output = new ushort[arr.Length/2];
            if(arr.Length<2 || arr.Length>4 || arr.Length %2 !=0) return output;
            byte[] couple = null;
            for (int i = 0; i < arr.Length; i += 2)
            {
                switch (order)
                {
                    case ByteOrder.ABCD:
                        couple = new byte[] { arr[i], arr[i + 1] };
                        output[i / 2] = BitConverter.ToUInt16(couple, 0);
                        break;
                    case ByteOrder.CDAB:
                        couple = new byte[] { arr[i], arr[i + 1] };
                        output[output.Length - 1 - (i / 2)] = BitConverter.ToUInt16(couple, 0);
                        break;
                    case ByteOrder.BADC:
                        couple = new byte[] { arr[i + 1], arr[i] };
                        output[i / 2] = BitConverter.ToUInt16(couple, 0);
                        break;
                    case ByteOrder.DCBA:
                        couple = new byte[] { arr[i + 1], arr[i] };
                        output[output.Length - 1 - (i / 2)] = BitConverter.ToUInt16(couple, 0);
                        break;
                    default:
                        break;
                }               
                
            }
            return output;
        }
        #endregion


    }
}
