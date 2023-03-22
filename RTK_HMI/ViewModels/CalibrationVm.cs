using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using RTK_HMI.Infrastructure.Commands;
using RTK_HMI.Views.DialogWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;

namespace RTK_HMI.ViewModels
{
    public class CalibrationVm:PropertyChangedBase
    {
		private readonly IRepository<CalibrationCell> _calibrationRepository;
        public CalibrationVm(MainViewModel mainVm)
        {
            MainVm = mainVm;
            _calibrationRepository = new EfRepository<CalibrationCell>();
            Init();

        }

        public MainViewModel MainVm { get; }
        private void Init()
        {
			Points = _calibrationRepository.Init(new List<CalibrationCell>
			{
				new CalibrationCell{ParameterId=23},
				new CalibrationCell{ParameterId = 54,Value=17},
				new CalibrationCell { ParameterId = 54, Value = 345.678f, UseCastomValue = true }
			}); ;
        }

		#region Точки для расчета
		/// <summary>
		/// Точки для расчета
		/// </summary>
		private IEnumerable<CalibrationCell> _points;
		/// <summary>
		/// Точки для расчета
		/// </summary>
		public IEnumerable<CalibrationCell> Points
		{
			get => _points;
			set => Set(ref _points, value);
		}
		#endregion

		#region Выбранная точка
		/// <summary>
		/// Выбранная точка
		/// </summary>
		private CalibrationCell _selectedPoint;
		/// <summary>
		/// Выбранная точка
		/// </summary>
		public CalibrationCell SelectedPoint
		{
			get => _selectedPoint;
			set => Set(ref _selectedPoint, value);
		}
		#endregion

		#region Степень кривой
		/// <summary>
		/// Степень кривой
		/// </summary>
		private int _degree = 1;
		/// <summary>
		/// Степень кривой
		/// </summary>
		public int Degree
		{
			get => _degree;
			set 
			{
				if(value>0 && value<6)
				{
                    Set(ref _degree, value);
                }                
            } 
		}
		#endregion

		#region Рассчитанные к-ты калибровки
		/// <summary>
		/// Рассчитанные к-ты калибровки
		/// </summary>
		private List<double> _coeffs;
		/// <summary>
		/// Рассчитанные к-ты калибровки
		/// </summary>
		public List<double> Coeffs
		{
			get => _coeffs;
			set => Set(ref _coeffs, value);
		}
        #endregion

        #region Коллекция измеренных значений для тренда
        private List<Point> _measuredPointsCollection;

        public List<Point> MeasuredPointsCollection
        {
            get { return _measuredPointsCollection; }
            set { Set(ref _measuredPointsCollection, value); }
        }
        #endregion

        #region Коллекция рассичтанных значений для тренда
        private List<Point> _сalculatedMeasCollection;

        public List<Point> CalculatedMeasCollection
        {
            get { return _сalculatedMeasCollection; }
            set { Set(ref _сalculatedMeasCollection, value); }
        }
        #endregion

        #region Команды

        #region Редактировать точку
        /// <summary>
        /// Редактировать точку
        /// </summary>
        RelayCommand _editCommand;
		/// <summary>
		/// Редактировать точку
		/// </summary>
		public RelayCommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand(execPar => 
		{
            if (SelectedPoint is null) return;
            SafetyAction(() =>
            {
				_calibrationRepository.Update(SelectedPoint);
            });
        }, canExecPar => true));
		#endregion

		#region Добавить точку
		/// <summary>
		/// Добавить точку
		/// </summary>
		RelayCommand _addCommand;
		/// <summary>
		/// Добавить точку
		/// </summary>
		public RelayCommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(execPar => 
		{
            var point = new CalibrationCell();
            SafetyAction(() =>
            {
                _calibrationRepository.Add(point);
            });
        }, canExecPar => true));
		#endregion

		#region Удалить точку
		/// <summary>
		/// Удалить точку
		/// </summary>
		RelayCommand _deleteCommand;
		/// <summary>
		/// Удалить точку
		/// </summary>
		public RelayCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(execPar => 
		{
            if (SelectedPoint is null) return;
            SafetyAction(() =>
            {
                _calibrationRepository.Delete(SelectedPoint);
            });
        }, canExecPar => true));
		#endregion

		#region Рассчитать 
		/// <summary>
		/// Рассчитать 
		/// </summary>
		RelayCommand _calculateCommand;
		/// <summary>
		/// Рассчитать 
		/// </summary>
		public RelayCommand CalculateCommand => _calculateCommand ?? (_calculateCommand = new RelayCommand(execPar => 
		{
			SafetyAction(() => 
			{
				Coeffs = Calculate();
			});
		}, canExecPar => true));
        #endregion

        #region Команда посчитать график для проверки полинома
        RelayCommand _showPolinomTrend;
        public RelayCommand ShowPolinomTrendCommand => _showPolinomTrend ?? (_showPolinomTrend = new RelayCommand(par =>
        {
           
            var measList = GetPoints().
            OrderBy(p => p.Item1).Select(p => new Point(p.Item1, p.Item2)).ToList();
            if (measList.Count >= 2)
            {
                var startWeak = measList[0].X;
                var finishWeak = measList[measList.Count - 1].X;
                if (startWeak != finishWeak)
                {
                    int cnt = 50;
                    double diff = (finishWeak - startWeak) / cnt;
                    var calcList = Enumerable.Range(0, cnt).
                    Select(i => new Point(startWeak + i * diff, GetPhysvalueByWeak(startWeak + i * diff))).ToList();
                    MeasuredPointsCollection = measList;
                    CalculatedMeasCollection = calcList;
                }
            }
        }, canExecPar => true));

        double GetPhysvalueByWeak(double weak)
        {
            double result = 0;
            for (int i = 0; i < Coeffs.Count; i++)
            {
                result += (Math.Pow(weak, i) * Coeffs[i]);
            }
            return result;
        }

        #endregion

        #endregion

        void SafetyAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



		List<double> Calculate()
		{
			var points = GetPoints().ToList();
			if(points.Count<2)
			{
				throw new Exception("Количество валидных точек меньше 2!");
                
            }
            var className = "Calibration";
            var asmName = "VeryImportantAlgortim.dll";
            Assembly asm = Assembly.LoadFrom(asmName);
            Type calibration = asm.GetTypes().Where(t => t.Name == className).FirstOrDefault();
            if (calibration != null)
            {

            }
            else throw new Exception($"Не удалось найти или некорректен файл {asmName}.dll");
            MethodInfo getCoeffs = calibration.GetMethod("GetCoeffs", BindingFlags.Public | BindingFlags.Static);
            object result = getCoeffs?.Invoke(null, new object[] { points, _degree });
            if (result is List<double> list) return list;
            else throw new Exception($"Не удалось найти или некорректен файл {asmName}.dll");
        }

		IEnumerable<(double,double)> GetPoints()
		{ 
			var pars = MainVm.ParameterVm.Parameters;
			float temp = 0;
			foreach (var p in Points)
			{
				(double x, double y) retPoint = (0, 0);
				if (!p.IsActive) continue;
				if (!p.UseCastomValue)
				{
					if (pars.Count() <= p.ParameterId) continue;
					if (!float.TryParse(pars.ElementAt(p.ParameterId).Value.ToString(), out temp)) continue;
                    retPoint.x = temp;
                }
				else retPoint.x = p.Value;
                if (!p.UseCastomValueY)
                {
                    if (pars.Count() <= p.ParameterIdY) continue;
                    if (!float.TryParse(pars.ElementAt(p.ParameterIdY).Value.ToString(), out temp)) continue;
                    retPoint.y = temp;
                }
                else retPoint.y = p.ValueY;
				yield return retPoint;
            }
		}



    }
}
