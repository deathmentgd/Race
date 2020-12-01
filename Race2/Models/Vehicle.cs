using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Race2.Models
{
	public enum VehicleType
	{
		Light = 1,
		Moto = 2,
		Heavy = 3
	}

	/// <summary>
	/// ТС
	/// </summary>
	public abstract class Vehicle : BindableBase
	{
		/// <summary>
		/// Место на финише		
		/// </summary>
		public virtual int Place { get; set; }
		/// <summary>
		/// Тип ТС
		/// </summary>
		public virtual VehicleType VehicleType { get; }
		/// <summary>
		/// Скорость ТС
		/// </summary>
		public virtual int Speed { get; set; }
		/// <summary>
		/// Пройденное расстояние
		/// </summary>
		public virtual double DistancePass
		{
			get { return GetProperty(() => DistancePass); }
			set { SetProperty(() => DistancePass, value); }
		}
		/// <summary>
		/// Затраченное время
		/// </summary>
		public virtual string ElapsedTime
		{
			get { return GetProperty(() => ElapsedTime); }
			set { SetProperty(() => ElapsedTime, value); }
		}

		/// <summary>
		/// Вероятность прокола
		/// </summary>
		public virtual double Puncture { get; set; }
		/// <summary>
		/// Флаг что прокол активен
		/// </summary>
		public virtual bool IsPuncture
		{
			get { return GetProperty(() => IsPuncture); }
			set { SetProperty(() => IsPuncture, value); }
		}
		/// <summary>
		/// Флаг что доехал до финиша
		/// </summary>
		public virtual bool IsFinished
		{
			get { return GetProperty(() => IsFinished); }
			set { SetProperty(() => IsFinished, value); }
		}
		/// <summary>
		/// Текущая вероятность (для контроля)
		/// </summary>
		public virtual double RndValue
		{
			get { return GetProperty(() => RndValue); }
			set { SetProperty(() => RndValue, value); }
		}

		/// <summary>
		/// Время починки прокола
		/// </summary>
		private long _punctureTimeSeconds { get; set; } = 3;

		/// <summary>
		/// Секундомер
		/// </summary>
		Stopwatch _stopWatch;
		/// <summary>
		/// Таймер для отсчета починки прокола
		/// </summary>
		System.Threading.Timer _timer;
		/// <summary>
		/// Интервал отслеживания расстояния в мс
		/// </summary>
		int _timerInterval;

		/// <summary>
		/// Рандомайзер для уникализации шанса прокола
		/// </summary>
		private static readonly Random random = new Random();
		private static readonly object syncLock = new object();

		/// <summary>
		/// Мои параметры
		/// </summary>
		public string MyParameters => ShowMyParameters();

		/// <summary>
		/// Сформировать строку описания параметров
		/// </summary>
		/// <returns></returns>
		public abstract string ShowMyParameters();

		//------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Создать ТС
		/// </summary>
		/// <param name="speed">скорость</param>
		/// <param name="puncture">шанс прокола</param>
		public Vehicle(VehicleType vehicleType, int speed, double puncture, int punctureTime)
		{
			IsFinished = true;
			Speed = speed;
			Puncture = puncture;
			_punctureTimeSeconds = punctureTime;
			VehicleType = vehicleType;
			IsPuncture = false;
		}

		//------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Стартовать
		/// </summary>
		/// <param name="lengthTrack"></param>
		public void Run(double lengthTrack, int timerInterval)
		{
			_timerInterval = timerInterval;
			IsFinished = false;
			DistancePass = 0;
			ElapsedTime = "00:00:00.000";
			_stopWatch = new Stopwatch();
			_stopWatch.Start();
			_timer = new System.Threading.Timer(new TimerCallback(GoGoGo), lengthTrack, 0, _timerInterval);
		}

		/// <summary>
		/// На каждый инкремент отслеживания чекаем прокол и считаем расстояние и время
		/// </summary>
		/// <param name="obj"></param>
		private void GoGoGo(object obj)
		{
			var lengthTrack = obj as double?;

			//Если не прокол - проверим шансы
			if (!IsPuncture)
			{
				CheckForPuncture();
				if (IsPuncture)
				{
					StopForPuncture();
				}
			}

			//Если все еще не прокол - то посчитаем расстояние
			if (!IsPuncture)
			{
				DistancePass += Math.Truncate(((Speed / (double)3600) * (_timerInterval / (double)1000))  * 1000000) / 1000000;
			}

			//Останавливаем все, когда доехалии
			if (DistancePass >= lengthTrack)
			{
				_stopWatch.Stop();
				_timer.Dispose();
				IsFinished = true;
			}
			//Отмечаем прошедшее время
			TimeSpan ts = _stopWatch.Elapsed;			
			ElapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
		}

		/// <summary>
		/// Проверяем не проколол ли
		/// </summary>
		private void CheckForPuncture()
		{
			lock (syncLock)
			{
				RndValue = random.NextDouble();
			}
			IsPuncture = RndValue >= 0 && RndValue <= Puncture;
		}

		/// <summary>
		/// Запускаем таймер после прокола, для возобновления движения
		/// </summary>
		private void StopForPuncture()
		{
			var timer = new System.Timers.Timer(_punctureTimeSeconds * 1000);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = false;
			timer.Enabled = true;
		}

		/// <summary>
		/// По истечении таймера - стартуем дальше
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			IsPuncture = false;
		}
	}
}
