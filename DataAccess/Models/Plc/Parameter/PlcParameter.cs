using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Timers;

namespace DataAccess.Models.Plc.Parameter
{
    public partial class PlcParameter<T>: PropertyChangedBase, IDataErrorInfo where T: IComparable
    {
        #region Конструктор
        public PlcParameter()
        {

        }
        public PlcParameter(string id, string description, T minValue, T maxValue, int dbNum, int dbOffset, bool isReadOnly)
        {
            Id = id;
            Description = description;
            MinValue = minValue; 
            MaxValue = maxValue;
            DbNum = dbNum;
            DbOffset = dbOffset;
            OnlyRead = isReadOnly;
            timer = new Timer(5000);
            timer.Elapsed += OnTimerElapsed;
        }

        #endregion

        #region IDataError
        Dictionary<string, string> errorsDict = new Dictionary<string, string>();
        public string this[string columnName] => errorsDict.ContainsKey(columnName) ? errorsDict[columnName] : null;

        public string Error { get; }

        #endregion

        #region Идентификатор
        public string Id { get; }

        #endregion

        #region Db номер
        int _dbNum;
        public int DbNum { get => _dbNum; set => Set(ref _dbNum, value); }

        #endregion

        #region Db Offset
        int _dbOffset;
        public int DbOffset { get => _dbOffset; set => Set(ref _dbOffset, value); }

        #endregion

        #region Валидация
        bool _validationOk = true;
        public bool ValidationOk { get => _validationOk; set => Set(ref _validationOk, value); }
        #endregion

        #region Описание
        string _description = "";
        public string Description { get => _description; set => Set(ref _description, value); }
        #endregion

        #region Минимальная величина
        T _minValue;
        public T MinValue { get => _minValue; set => Set(ref _minValue, value); }
        #endregion

        #region Максмальная величина
        T _maxValue;
        public T MaxValue { get => _maxValue; set => Set(ref _maxValue, value); }
        #endregion

        #region Только чтение
        bool _onlyRead;
        public bool OnlyRead { get => _onlyRead; set => Set(ref _onlyRead, value); }
        #endregion

        #region Величина

        T _value;

        [DataMember]

        public T Value
        {

            get => _value;
            set
            {
                IsWriting = false;
                if (Set(ref _value, value))
                {
                    WriteValue = value;
                }
            }

        }

        #endregion

        #region Значение записи
        T _writeValue;
        [DataMember]
        public T WriteValue
        {
            get => _writeValue;
            set
            {
                if (value != null)
                {

                    if (value.CompareTo(Value) != 0)
                    {
                        RestartTimer();
                    }
                    if (errorsDict != null)
                    {
                        if (value.CompareTo(MinValue) < 0 || value.CompareTo(MaxValue) > 0)
                        {
                            errorsDict["WriteValue"] = $"Значение параметра записи \"{Description}\" больше максимального или меньше минимального предела!";
                            ValidationOk = false;
                        }
                        else
                        {
                            errorsDict["WriteValue"] = null;
                            ValidationOk = true;
                        }
                    }
                    Set(ref _writeValue, value);
                }

            }

        }

        #endregion


        #region Writing Process Indicator

        /// <summary>
        /// Writing Process Indicator
        /// </summary>
        private bool _isWriting = true;
        /// <summary>
        /// Writing Process Indicator
        /// </summary>
        public bool IsWriting
        {
            get => _isWriting;
            set => Set(ref _isWriting, value);
        }
        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }

        #region Таймер
        Timer timer;
        void RestartTimer()
        {
            if (timer.Enabled) timer.Stop();
            timer.Start();
        }

        void OnTimerElapsed(object source, ElapsedEventArgs e)
        {
            timer.Stop();
            WriteValue = Value;
        }

        #endregion

    }
}
