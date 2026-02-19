using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;


namespace DataAccess.Models
{
    public enum DataType
    {
        String, Float32, Int32, Uint32, Int16, Uint16
    };
    [DataContract]
    public class Parameter :PropertyChangedBase, IDatabased
    {
        [DataMember]
        public int Id { get ; set; }

        #region Номер регистра
        private int _regNum;
        [DataMember]
        public int RegNum
        {
            get { return _regNum; }
            set => Set(ref _regNum, value);
        }
        #endregion

        #region Описание
        private string _description;
        [DataMember]
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
        #endregion

        #region Тип данных
        private DataType _type;
        [DataMember]
        public DataType Type { get => _type; set => Set(ref _type, value); }
        #endregion

        #region Длина (если строка)
        private int _length;
        [DataMember]
        public int Length { get => _length; set => Set(ref _length, value); } 
        #endregion

        #region Текущее значение
        private object _value;
        [NotMapped]
        [DataMember]
        public object Value
        {
            get => _value;
            set => Set(ref _value, Convert(value));
        }
        #endregion

        #region Примечание
        /// <summary>
        /// Примечание
        /// </summary>
        private string _notification;
        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        public string Notification
        {
            get => _notification;
            set => Set(ref _notification, value);
        }
        #endregion

        #region Циклический опрос
        /// <summary>
        /// Циклический опрос
        /// </summary>
        private bool _isCyclic;
        /// <summary>
        /// Циклический опрос
        /// </summary>
        [DataMember]
        public bool IsCyclic
        {
            get => _isCyclic;
            set => Set(ref _isCyclic, value);
        }
        #endregion

        #region Register Type
        /// <summary>
        /// Register Type
        /// </summary>
        private Registers _regType;
        /// <summary>
        /// Register Type
        /// </summary>
        [DataMember]
        public Registers RegType
        {
            get => _regType;
            set => Set(ref _regType, value);
        }
        #endregion

        #region Byte Order
        /// <summary>
        /// Byte Order
        /// </summary>
        private ByteOrder _order;
        /// <summary>
        /// Byte Order
        /// </summary>
        [DataMember]
        public ByteOrder Order
        {
            get => _order;
            set => Set(ref _order, value);
        }
        #endregion

        #region VisibleForAdmin
        /// <summary>
        /// Byte Order
        /// </summary>
        private bool _visibleForAdmin;
        /// <summary>
        /// Byte Order
        /// </summary>
        [DataMember]
        public bool VisibleForAdmin
        {
            get => _visibleForAdmin;
            set => Set(ref _visibleForAdmin, value);
        }
        #endregion


        object Convert(object writeValue)
        {
            if(writeValue == null)return null;
            switch (Type)
            {
                case DataType.String:
                    return writeValue.ToString();
                case DataType.Float32:
                    float tempFloat = 0;
                    float.TryParse(writeValue.ToString().Replace(",","."), NumberStyles.Any, CultureInfo.InvariantCulture, out tempFloat);
                    return tempFloat;                   
                case DataType.Int32:
                    int tempInt = 0;
                    int.TryParse(writeValue.ToString(), out tempInt);
                    return tempInt;
                case DataType.Uint32:
                    uint tempUInt = 0;
                    uint.TryParse(writeValue.ToString(), out tempUInt);
                    return tempUInt;
                case DataType.Int16:
                    short tempShort = 0;
                    short.TryParse(writeValue.ToString(), out tempShort);
                    return tempShort; ;
                case DataType.Uint16:
                    ushort tempUShort = 0;
                    ushort.TryParse(writeValue.ToString(), out tempUShort);
                    return tempUShort;                
                   
            }
            return writeValue;
        }

    }
}
