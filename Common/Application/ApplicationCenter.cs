using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SLD.Tezos.Client
{
	using Documents;
	using OS;
	using Serialization;

	public static class ApplicationCenter
	{
		public static List<IBackupIdentity> BackupProviders = new List<IBackupIdentity>();

		public static SyncEvent UIReady = new SyncEvent();

		public static event Action<object> DocumentReceived;

		public static object WaitingDocument;

		public static void Receive(string json)
		{
			Trace.WriteLine($"Receive json: {json}");

			object document = null;

			try
			{
				document = Document.FromJson(json);
			}
			catch
			{
				try
				{
					// Alpha faucet document?
					document = FaucetDocument.FromJson(json);
				}
				catch (Exception e)
				{
					Telemetry.TrackException("Received document caused failure", e,
						"Content", json);
				}
			}

			if (document != null)
			{
				// Known document type
				Trace.WriteLine($"Received document: {document}");

				if (DocumentReceived != null)
				{
					DocumentReceived(document);
				}
				else
				{
					WaitingDocument = document;
				}
			}
		}
	}
}