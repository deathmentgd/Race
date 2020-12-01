using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Race2.Properties;
using System.Windows.Threading;
using Race2.Models;
using System.Configuration;

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
			TrackParamsConfigSection sectionTrack = (TrackParamsConfigSection)ConfigurationManager.GetSection("TrackParams");
			if (sectionTrack != null)
			{
				Track = new Track(sectionTrack.Length, sectionTrack.TimerInterval);
			}
			else
			{
				//Инициализация
				Track = new Track(1, 100);
			}

			VehiclesConfigSection section = (VehiclesConfigSection)ConfigurationManager.GetSection("Vehicles");

			if (section != null)
			{
				foreach (RacerElement racer in section.RacersItems)
				{
					switch (racer.VehicleType)
					{
						case VehicleType.Light:
							Track.Racers.Add(new LightVehicle(racer.Speed, racer.Puncture, (int)racer.People, racer.PunctureTime));
							break;
						case VehicleType.Moto:
							Track.Racers.Add(new MotoVehicle(racer.Speed, racer.Puncture, (bool)racer.HasSidecar, racer.PunctureTime));
							break;
						case VehicleType.Heavy:
							Track.Racers.Add(new HeavyVehicle(racer.Speed, racer.Puncture, (int)racer.Weight, racer.PunctureTime));
							break;
					}
				}
			}

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
	}
}