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
                        param.Value = GetStringFromUshorts(ushortArr, param.RegNum - offset);
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
                        param.Value = (short)ushortArr[param.RegNum - offset];
                        break;
                    case DataType.Uint16:
                        param.Value = ushortArr[param.RegNum - offset];
                        break;
                    default:
                        break;
                }
            }




        }

        #region Получить float из 2х регистров
        static float GetFloatFromUshorts(ushort[] regs, int regNum)
        {
            ushort[] arr = new ushort[] { regs[regNum], regs[regNum + 1] };
            return BitConverter.ToSingle(arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray(), 0);
        }
        #endregion

        #region Получить uint32 из 2х регистров
        static uint GetUInt32FromUshorts(ushort[] regs, int regNum)
        {
            try
            {
                ushort[] arr = new ushort[] { regs[regNum], regs[regNum + 1] };
                return BitConverter.ToUInt32(arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray(), 0);
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
                return BitConverter.ToInt32(arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray(), 0);
            }
            catch (Exception)
            {
                return 0;
            }


        }
        #endregion

        #region Получить строку из байт
        static string GetStringFromUshorts(ushort[] regs, int regNum)
        {
            ushort[] arr = new ushort[] { regs[regNum], regs[regNum + 1] };
            return Encoding.GetEncoding(1251).GetString(arr.SelectMany(num => BitConverter.GetBytes(num)).ToArray());            
        }
        #endregion
    }
}
