using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Race2.Models
{
	/// <summary>
	/// Трасса 
	/// </summary>
	public class Track : BindableBase
	{
		public delegate void FinishHandler();
		/// <summary>
		/// Ивент о завершении гонки
		/// </summary>
		public event FinishHandler OnFinishHandler;

		/// <summary>
		/// Длинна трассы
		/// </summary>
		public virtual double Length
		{
			get { return GetProperty(() => Length); }
			set { SetProperty(() => Length, value); }
		}
		public virtual string LengthString
		{
			get { return GetProperty(() => LengthString); }
			set { SetProperty(() => LengthString, value); }
		}
		public virtual int TimerInterval { get; set; }
		public virtual string IntervalString
		{
			get { return GetProperty(() => IntervalString); }
			set { SetProperty(() => IntervalString, value); }
		}
		/// <summary>
		/// Участники
		/// </summary>
		public virtual ObservableCollection<Vehicle> Racers { get; set; } = new ObservableCollection<Vehicle>();
		/// <summary>
		/// Итоговые результаты
		/// </summary>
		public virtual ObservableCollection<Vehicle> FinishedRacers { get; set; } = new ObservableCollection<Vehicle>();

		/// <summary>
		/// Флаг что все доехали
		/// </summary>
		public virtual bool IsAllFinished
		{
			get { return GetProperty(() => IsAllFinished); }
			set { SetProperty(() => IsAllFinished, value); }
		}

		/// <summary>
		/// Внутренний таймер для проверки окончания
		/// </summary>
		System.Threading.Timer _timer;

		//------------------------------------------------------------------------

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="length">длина трассы в км</param>
		public Track(double length, int timerInterval)
		{
			Length = length;
			TimerInterval = timerInterval;
			IsAllFinished = true;
			LengthString = $"{Length} км";
			IntervalString = $"{TimerInterval} мс";
		}

		/// <summary>
		/// Начать гонку
		/// </summary>
		public void StartRace()
		{
			IsAllFinished = false;
			FinishedRacers.Clear();
			Racers.ForEach(veh =>
			{
				veh.Run(Length, TimerInterval);
			});
			CheckForFinish();
		}

		/// <summary>
		/// Запустить таймер на проверку окончания
		/// </summary>
		private void CheckForFinish()
		{							
			_timer = new System.Threading.Timer(new TimerCallback(CheckDo), 0, 0, 1000);			
		}

		/// <summary>
		/// Сама проверка окончания
		/// </summary>
		/// <param name="obj"></param>
		private void CheckDo(object obj)
		{
			if (Racers.Count(x=>x.IsFinished) == Racers.Count)
			{
				_timer.Dispose();
				IsAllFinished = true;
				OnFinishHandler?.Invoke();
				//Отсортируем по времени и выведем таблицу
				var list = Racers.OrderBy(x => x.ElapsedTime).ToList();
				for (var i = 0; i<list.Count;i++)
				{
					list[i].Place = i + 1;
					FinishedRacers.Add(list[i]);
				}
			}
		}
	}
}
