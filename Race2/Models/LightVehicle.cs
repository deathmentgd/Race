using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race2.Models
{
	/// <summary>
	/// Легковой автомобиль
	/// </summary>
	public class LightVehicle : Vehicle
	{
		/// <summary>
		/// Сколько человек внутри
		/// </summary>
		public virtual int PeopleInside { get; set; }

		public LightVehicle(int speed, double puncture, int peopleInside, int punctureTime) : base(VehicleType.Light, speed, puncture, punctureTime)
		{
			PeopleInside = peopleInside;
		}

		/// <summary>
		/// Сформировать строку описания параметров
		/// </summary>
		/// <returns></returns>
		public override string ShowMyParameters()
		{
			string msg;
			if (PeopleInside == 0)
			{
				msg = "В машине нет пассажиров";
			}
			else
			{
				msg = $"В машине {PeopleInside} пассажир(а,ов)";
			}
			return msg; 
		}
	}
}
