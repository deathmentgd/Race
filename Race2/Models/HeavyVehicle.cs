using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race2.Models
{
	/// <summary>
	/// Грузовик
	/// </summary>
	public class HeavyVehicle : Vehicle
	{
		/// <summary>
		/// Вес груза
		/// </summary>
		public virtual double Weight { get; set; }

		public HeavyVehicle(int speed, double puncture, double weight, int punctureTime) : base(VehicleType.Heavy, speed, puncture, punctureTime)
		{
			Weight = weight;
		}

		/// <summary>
		/// Сформировать строку описания параметров
		/// </summary>
		/// <returns></returns>
		public override string ShowMyParameters()
		{
			return $"Груз массой {Weight} кг";
		}
	}
}
