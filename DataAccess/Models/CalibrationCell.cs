namespace DataAccess.Models
{
    public class CalibrationCell : PropertyChangedBase, IDatabased
    {
        public int Id { get ; set ; }

		#region Id параметра в расчетах
		/// <summary>
		/// Id параметра в расчетах
		/// </summary>
		private int _parameterId;
		/// <summary>
		/// Id параметра в расчетах
		/// </summary>
		public int ParameterId
		{
			get => _parameterId;
			set => Set(ref _parameterId, value);
		}
		#endregion

		#region Использование совей переменной в расчете
		/// <summary>
		/// Использование совей переменной в расчете
		/// </summary>
		private bool _useCastomValue;
		/// <summary>
		/// Использование совей переменной в расчете
		/// </summary>
		public bool UseCastomValue
		{
			get => _useCastomValue;
			set => Set(ref _useCastomValue, value);
		}
		#endregion

		#region Флаг использования в расчете
		/// <summary>
		/// Флаг использования в расчете
		/// </summary>
		private bool _isActive;
		/// <summary>
		/// Флаг использования в расчете
		/// </summary>
		public bool IsActive
		{
			get => _isActive;
			set => Set(ref _isActive, value);
		}
		#endregion

		#region Значение
		/// <summary>
		/// Значение
		/// </summary>
		private float _value;
		/// <summary>
		/// Значение
		/// </summary>
		public float Value
		{
			get => _value;
			set => Set(ref _value, value);
		}
        #endregion

        #region Id параметра в расчетах(Y)
        /// <summary>
        /// Id параметра в расчетах(Y)
        /// </summary>
        private int _parameterIdY;
        /// <summary>
        /// Id параметра в расчетах(Y)
        /// </summary>
        public int ParameterIdY
        {
            get => _parameterIdY;
            set => Set(ref _parameterIdY, value);
        }
        #endregion
        #region Использование совей переменной в расчете(Y)
        /// <summary>
        /// Использование совей переменной в расчете(Y)
        /// </summary>
        private bool _useCastomValueY;
        /// <summary>
        /// Использование совей переменной в расчете(Y)
        /// </summary>
        public bool UseCastomValueY
        {
            get => _useCastomValueY;
            set => Set(ref _useCastomValueY, value);
        }
        #endregion
        #region Значение Y
        /// <summary>
        /// Значение Y
        /// </summary>
        private float _valueY;
        /// <summary>
        /// Значение Y
        /// </summary>
        public float ValueY
        {
            get => _valueY;
            set => Set(ref _valueY, value);
        }
        #endregion
    }
}
