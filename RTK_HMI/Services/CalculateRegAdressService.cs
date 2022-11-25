using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace RTK_HMI.Services
{
    internal static class CalculateRegAdressService
    {
        public static (int, int) GetStartAndCount(IEnumerable<Parameter> parameters)
        { 
            var start = CalulateStartAddr(parameters);
            var finish = CalculateFinishAddr(parameters);
            return (start, finish-start+1);
        }

        static int CalulateStartAddr(IEnumerable<Parameter> parameters)
        {
            return parameters.Select(p => p.RegNum).Min();
        }

        static int CalculateFinishAddr(IEnumerable<Parameter> parameters)
        {
            return parameters.Select(p=>p.RegNum+GetLenght(p)-1).Max();
        }

        static int GetLenght(Parameter par)
        {
            switch (par.Type)
            {
                case DataType.String:
                    return par.Length%2==0?par.Length/2:par.Length/2+1;
                case DataType.Float32:                    
                case DataType.Int32:                    
                case DataType.Uint32:
                    return 2;
                case DataType.Int16:                    
                case DataType.Uint16:
                    return 1;
                default:
                    return 1;
            }
        }
    }
}
