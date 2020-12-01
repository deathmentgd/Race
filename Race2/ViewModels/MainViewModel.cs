using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Race2.Properties;
using System.Windows.Threading;
using Race2.Models;

namespace Race2.ViewModels
{
	[POCOViewModel]
	public class MainViewModel
	{
		public virtual IMessageBoxService MBS => null;
		public virtual ICurrentWindowService CWS => null;

		/// <summary>
		/// Экземпляр трэка
		/// </summary>
		public virtual Track Track { get; set; }

		public bool IsMainLoaded = false;

		protected MainViewModel()
		{
			DevExpress.Xpf.Core.DXGridDataController.DisableThreadingProblemsDetection = true;
		}

		public static MainViewModel Create()
		{
			return ViewModelSource.Create(() => new MainViewModel());
		}

		//-------------------------------------------------------------------------------------------------------------------

		public void MainViewLoaded()
		{
			//Инициализация
			Track = new Track(1);
			Track.Racers.Add(new LightVehicle(90, 0.0023, 2));
			Track.Racers.Add(new LightVehicle(80, 0.0007, 0));
			Track.Racers.Add(new MotoVehicle(110, 0.0040, true));
			Track.Racers.Add(new MotoVehicle(100, 0.0011, false));
			Track.Racers.Add(new HeavyVehicle(160, 0.004, 2000));
			Track.Racers.Add(new HeavyVehicle(150, 0.003, 2300));
			//Подпишемся на окончание гонки
			Track.OnFinishHandler += OnFinish;
		}		

		/// <summary>
		/// Запустить гонку
		/// </summary>
		public void RunRace()
		{
			Track.StartRace();
		}

		/// <summary>
		/// Спросим повтор по окончании гонки
		/// </summary>
		public void OnFinish()
		{
			System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
								 new System.Action(delegate ()
								 {
									 if (MBS.ShowMessage("Хотите повторить гонку?", "Заезд завершен", MessageButton.YesNo, MessageIcon.Information) == MessageResult.Yes)
									 {
										 RunRace();
									 }
								 })
							  );
		}

		public void ShowMessageError(string msg, string title)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
								 new System.Action(delegate ()
								 {
									 MBS.ShowMessage(msg, title, MessageButton.OK, MessageIcon.Error);
								 })
							  );
		}
	}
}