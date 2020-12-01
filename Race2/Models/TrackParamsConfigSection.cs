using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race2.Models
{
	public class TrackParamsConfigSection : ConfigurationSection
	{
		[ConfigurationProperty("length", DefaultValue = "2")]
		public int Length
		{
			get { return ((int)(base["length"])); }
		}

		[ConfigurationProperty("timerInterval", DefaultValue = "100")]
		public int TimerInterval
		{
			get { return ((int)(base["timerInterval"])); }
		}
	}	
}
