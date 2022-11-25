using System.Collections.Generic;

namespace DataAccess.Models
{
    public static class ParameterDataFactory
    {
        public static  IEnumerable<Parameter> Get()
        {
            return new List<Parameter>
            { 
                new Parameter{Description ="Модель", RegNum=65, Type=DataType.String, Length=10},
                new Parameter{Description ="Серийный номер", RegNum=70, Type=DataType.String, Length=10},
                new Parameter{Description ="Газ", RegNum=75, Type=DataType.String, Length=6},
                new Parameter{Description ="Единица измерения скорости", RegNum=78, Type=DataType.String, Length=6},
                new Parameter{Description ="Единица измерения потока", RegNum=81, Type=DataType.String, Length=6},
                new Parameter{Description ="Пароль", RegNum=84, Type=DataType.String, Length=6},
                new Parameter{Description ="Профиль трубы", RegNum=89, Type=DataType.Int16},
                new Parameter{Description ="Диаметр трубы", RegNum=90, Type=DataType.Float32},
                new Parameter{Description ="Высота, мм", RegNum=92, Type=DataType.Float32},
                new Parameter{Description ="Ширина, мм", RegNum=94, Type=DataType.Float32},
                new Parameter{Description ="Площадь потока, мм^2", RegNum=96, Type=DataType.Float32},
                new Parameter{Description ="Минимальный поток", RegNum=98, Type=DataType.Float32},
                new Parameter{Description ="К-т коррекции потока", RegNum=100, Type=DataType.Float32},
                new Parameter{Description ="Диаметр сенсора, мм", RegNum=102, Type=DataType.Float32},
                new Parameter{Description ="Длина сенсора, мм", RegNum=104, Type=DataType.Float32},
                new Parameter{Description ="К-т блокировки потока", RegNum=106, Type=DataType.Float32},
                new Parameter{Description ="Время усреднения, сек", RegNum=108, Type=DataType.Float32},
                new Parameter{Description ="Всего, расход", RegNum=110, Type=DataType.Float32},
                new Parameter{Description ="Полное время работы", RegNum=112, Type=DataType.Float32},
                new Parameter{Description ="Номер кривой", RegNum=114, Type=DataType.Int16},
                new Parameter{Description ="Тип ИОН", RegNum=115, Type=DataType.Int16},
                new Parameter{Description ="Тип выхода", RegNum=116, Type=DataType.Int16}
            };
        }
    }
}
