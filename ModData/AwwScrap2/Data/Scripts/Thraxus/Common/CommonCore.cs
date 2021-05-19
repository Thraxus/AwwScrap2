﻿using AwwScrap2.Common.BaseClasses;
using AwwScrap2.Common.Enums;
using AwwScrap2.Common.Factions.Models;
using AwwScrap2.Common.Reporting;
using VRage.Game.Components;

namespace AwwScrap2.Common
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, priority: int.MinValue + 1)]
	public class CommonCore : BaseSessionComp
	{
		protected override string CompName { get; } = "CommonCore";
		protected override CompType Type { get; } = CompType.Server;
		protected override MyUpdateOrder Schedule { get; } = MyUpdateOrder.NoUpdate;

		protected override void SuperEarlySetup()
		{
			base.SuperEarlySetup();
		}

		protected override void LateSetup()
		{
			base.LateSetup();
			FactionDictionaries.Initialize();
			WriteToLog($"{CompName} - Game Settings", $"{GameSettings.Report()}", LogType.General);
			WriteToLog($"{CompName} - Factions", $"{FactionDictionaries.Report()}", LogType.General);
		}
	}
}
