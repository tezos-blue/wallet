using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;

namespace SLD.Tezos.Client
{
	public static class Telemetry
	{
		private static ITelemetry Client;

		public static void Initialize(ITelemetry telemetry)
		{
			Client = telemetry;
		}

		public static void TrackEvent(string name, params string[] namevalues)
		{
			var finalName = $"Wallet | {name}";
			var properties = MakeProperties(namevalues);

			Client?.TrackEvent(finalName, properties);

			Analytics.TrackEvent(finalName, properties);
		}

		public static void TrackException(string text, Exception e, params string[] namevalues)
		{
			var properties = MakeProperties(namevalues);

			Client?.TrackException(text, e, properties);
		}

		private static Dictionary<string, string> MakeProperties(string[] namevalues)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();

			if (namevalues != null)
			{
				for (int i = 0; i < namevalues.Length; i += 2)
				{
					data.Add(namevalues[i], namevalues[i + 1]);
				}
			}

			data.Add("Module", "Client");

			return data;
		}
	}
}