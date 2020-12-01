using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race2.Models
{
	/// <summary>
	/// Мотоцикл
	/// </summary>
	public class MotoVehicle : Vehicle
	{
		/// <summary>
		/// Есть ли коляска
		/// </summary>
		public virtual bool HasSidecar { get; set; }

		public MotoVehicle(int speed, double puncture, bool hasSidecar) : base(VehicleType.Moto, speed, puncture)
		{
			HasSidecar = hasSidecar;
		}

		/// <summary>
		/// Сформировать строку описания параметров
		/// </summary>
		/// <returns></returns>
		public override string ShowMyParameters()
		{			
			return HasSidecar ? "Есть коляска" : "Без коляски";
		}
	}
}
