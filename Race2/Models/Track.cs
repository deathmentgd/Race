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
		public event FinishHandler OnFinishHandler;

		/// <summary>
		/// Длинна трассы
		/// </summary>
		public virtual double Length { get; set; }
		/// <summary>
		/// Участники
		/// </summary>
		public virtual ObservableCollection<Vehicle> Racers { get; set; } = new ObservableCollection<Vehicle>();
		public virtual ObservableCollection<Vehicle> FinishedRacers { get; set; } = new ObservableCollection<Vehicle>();

		public virtual bool IsAllFinished
		{
			get { return GetProperty(() => IsAllFinished); }
			set { SetProperty(() => IsAllFinished, value); }
		}

		System.Threading.Timer _timer;

		public Track(double length)
		{
			Length = length;
			IsAllFinished = true;
		}

		public void StartRace()
		{
			IsAllFinished = false;
			FinishedRacers.Clear();
			Racers.ForEach(veh =>
			{
				veh.Run(Length);
			});
			CheckForFinish();
		}

		private void CheckForFinish()
		{							
			_timer = new System.Threading.Timer(new TimerCallback(CheckDo), 0, 0, 1000);			
		}

		private void CheckDo(object obj)
		{
			if (Racers.Count(x=>x.IsFinished) == Racers.Count)
			{
				_timer.Dispose();
				IsAllFinished = true;
				OnFinishHandler?.Invoke();
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
