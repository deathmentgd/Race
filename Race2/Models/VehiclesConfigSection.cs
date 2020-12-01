using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race2.Models
{
	public class VehiclesConfigSection : ConfigurationSection
	{
		[ConfigurationProperty("Racers")]
		public RacersCollection RacersItems
		{
			get { return ((RacersCollection)(base["Racers"])); }
		}
	}

	[ConfigurationCollection(typeof(RacerElement))]
	public class RacersCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new RacerElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RacerElement)(element)).NumberOrder;
		}

		public RacerElement this[int idx]
		{
			get { return (RacerElement)BaseGet(idx); }
		}
	}

	public class RacerElement : ConfigurationElement
	{
		[ConfigurationProperty("numberOrder", DefaultValue = "1", IsRequired = true, IsKey = true)]
		public int NumberOrder
		{
			get { return ((int)(base["numberOrder"])); }
		}

		[ConfigurationProperty("vehicleType", DefaultValue = "Light", IsRequired = true, IsKey =false)]
		public VehicleType VehicleType
		{
			get { return ((VehicleType)(base["vehicleType"])); }			
		}

		[ConfigurationProperty("speed", DefaultValue = "100", IsRequired = true, IsKey = false)]
		public int Speed
		{
			get { return ((int)(base["speed"])); }			
		}

		[ConfigurationProperty("puncture", DefaultValue = "0.05", IsRequired = true, IsKey = false)]
		public double Puncture
		{
			get { return ((double)(base["puncture"])); }
		}

		[ConfigurationProperty("people", IsRequired = false, IsKey = false)]
		public int? People
		{
			get { return ((int?)(base["people"])); }
		}

		[ConfigurationProperty("hasSidecar", IsRequired = false, IsKey = false)]
		public bool? HasSidecar
		{
			get { return ((bool?)(base["hasSidecar"])); }
		}

		[ConfigurationProperty("weight", IsRequired = false, IsKey = false)]
		public int? Weight
		{
			get { return ((int?)(base["weight"])); }
		}

		[ConfigurationProperty("punctureTime", DefaultValue ="3", IsRequired = false, IsKey = false)]
		public int PunctureTime
		{
			get { return ((int)(base["punctureTime"])); }
		}
	}
}
