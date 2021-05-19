using System.Collections.Generic;
using System.Text;
using AwwScrap2.Common.BaseClasses;
using AwwScrap2.Common.Enums;
using AwwScrap2.Support;
using Sandbox.Definitions;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace AwwScrap2
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, priority: int.MinValue + 1)]
	public class AwwScrap2Core : BaseSessionComp
	{
		protected override string CompName { get; } = "AwwScrap2Core";
		protected override CompType Type { get; } = CompType.Server;
		protected override MyUpdateOrder Schedule { get; } = MyUpdateOrder.BeforeSimulation;

		private readonly HashSet<MyBlueprintDefinition> _blueprints = new HashSet<MyBlueprintDefinition>(EqualityComparer<MyBlueprintDefinition>.Default);
		private readonly HashSet<BlueprintClassEntry> _blueprintClasses = new HashSet<BlueprintClassEntry>(EqualityComparer<BlueprintClassEntry>.Default);
		private readonly HashSet<MyPhysicalItemDefinition> _components = new HashSet<MyPhysicalItemDefinition>(EqualityComparer<MyPhysicalItemDefinition>.Default);
		private readonly HashSet<MyPhysicalItemDefinition> _ores = new HashSet<MyPhysicalItemDefinition>(EqualityComparer<MyPhysicalItemDefinition>.Default);
		private readonly HashSet<MyCubeBlockDefinition> _cubes = new HashSet<MyCubeBlockDefinition>(EqualityComparer<MyCubeBlockDefinition>.Default);

		public override void LoadData()
		{
			base.LoadData();
			SetupPhysicalItem();
			//SetupTestItem2();
		}

		public override void BeforeStart()
		{
			base.BeforeStart();

			//MyDefinitionManager.Static.GetBlueprintClass()

			//SetupCollections();

			//GrabProductionBlockDefinitions();
			//Playground();

			//DissectDefinitionManager();
			//DissectBlueprints();

			//PrintProductionBlockDefinitions();
			//ReportDefinitions();

			//SetupCollections();
			//ReportDefinitionManager();
			//WriteToLog("", ToString(), LogType.General);
			ReportDefinitions();

			//MyPhysicalItemDefinition siliconOreDef = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Silicon"));
			//foreach (var def in MyDefinitionManager.Static.GetDefinitionsOfType<MyPhysicalItemDefinition>())
			//{
			//	WriteToLog("", $"{def.Id}", LogType.General);
			//}

		}

		private void Playground()
		{
			//IMyRefinery iMyRef;
			//MyRefinery myRef;
			//IMyProductionBlock x;

			//MyProductionBlockDefinition x;


			//ListReader<MyProductionBlockDefinition> myProductionBlockDefinitions = MyDefinitionManager.Static.GetDefinitionsOfType<MyProductionBlockDefinition>();

			//WriteToLog("", $"{myProductionBlockDefinitions.Count}", LogType.General);
			//WriteToLog("", "", LogType.General);

			//foreach (var def in MyDefinitionManager.Static.GetDefinitionsOfType<MyProductionBlockDefinition>())
			//{
			//	WriteToLog("",$"{def} | {def.BlueprintClasses?.Count} | {def.SubBlockDefinitions?.Count} | {def.ResourceSinkGroup}", LogType.General);
			//}
		}

		private void SetupTestItem()
		{

			// 1) Physical Item
			// 2) Blueprint
			// 3) Blueprint Class
			// 4) Blueprint Class Entry

			// 1) Physical Item: 
			MyPhysicalItemDefinition newDef = new MyPhysicalItemDefinition
			{
				Id = new MyDefinitionId(typeof(MyObjectBuilder_Ore), "BulletproofGlassScrap"),
				DisplayNameString = "Bulletproof Glass Scrap",
				Icons = new[] { Constants.IconLocation + Constants.BulletproofGlassIcon },
				Size = new Vector3(0.2, 0.2, 0.1),
				Mass = 1,
				Volume = 0.254f,
				Model = Constants.KnownScrapModelLocation + Constants.Scrap2,
				PhysicalMaterial = MyStringHash.GetOrCompute("Metal")
			};

			MyDefinitionManager.Static.Definitions.AddDefinition(newDef);

			// 2) Blueprint
			MyPhysicalItemDefinition siliconIngotDef = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Ingot), "Silicon"));
			MyPhysicalItemDefinition siliconOreDef = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Silicon"));
			MyBlueprintDefinition newBp = new MyBlueprintDefinition
			{
				Id = new MyDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), "BulletproofGlassToIngot"),
				DisplayNameString = "Bulletproof Glass Scrap",
				Icons = new[]
				{
					Constants.IconLocation + Constants.BulletproofGlassIcon
				},
				Prerequisites = new[]
				{
					new MyBlueprintDefinitionBase.Item
					{
						Amount = 1,
						Id = siliconOreDef.Id
					}
				},
				Results = new[]
				{
					new MyBlueprintDefinitionBase.Item
					{
						Amount = (MyFixedPoint) 6.75,
						Id = siliconIngotDef.Id
					}
				},
				BaseProductionTimeInSeconds = 0.04f
			};

			MyDefinitionManager.Static.Definitions.AddDefinition(newBp);

			// 3) Blueprint Class
			MyBlueprintClassDefinition newBpClass = new MyBlueprintClassDefinition
			{
				Id = new MyDefinitionId(typeof(MyBlueprintClassDefinition), Constants.AwwScrapBlueprintClassSubtypeId),
				DisplayNameString = "Scrap Recycling",
				DescriptionString = "Scrap Recycling",
				Icons = new[]
				{
					"Textures\\GUI\\Icons\\component\\ScrapMetalComponent.dds"
				},
				HighlightIcon = "Textures\\GUI\\Icons\\component\\ScrapMetalComponent.dds",
				InputConstraintIcon = "Textures\\GUI\\Icons\\filter_ore.dds",
				OutputConstraintIcon = "Textures\\GUI\\Icons\\filter_ingot.dds"
			};

			MyDefinitionManager.Static.Definitions.AddDefinition(newBpClass);

			// 4) Blueprint Class Entry
			BlueprintClassEntry newBpClassEntry = new BlueprintClassEntry
			{
				TypeId = newBp.Id.TypeId,
				BlueprintSubtypeId = "BulletproofGlassToIngot",
				Class = "Ingots",
				Enabled = true,
				BlueprintTypeId = newBp.Id.TypeId.ToString()
			};

			//MyDefinitionManager.Static.Definitions.Definitions.
			//MyDefinitionManager.Static.Definitions.Definitions.Add(typeof(BlueprintClassEntry));
		}

		private bool ManualAddToDefinitions(MyDefinitionBase def)
		{
			//Dictionary<MyStringHash, MyDefinitionBase> tmpVal;
			//if (!MyDefinitionManager.Static.Definitions.Definitions.TryGetValue(def.Id.TypeId, out tmpVal))
			//{
			//	WriteToLog("","Def not found...",LogType.General);
			//	tmpVal = new Dictionary<MyStringHash, MyDefinitionBase>();
			//	MyDefinitionManager.Static.Definitions.Definitions[def.Id.TypeId] = tmpVal;
			//}

			//MyDefinitionSet y = new MyDefinitionSet();
			//y.

			//MyDefinitionManagerBase x = MyDefinitionManagerBase.Static;
			//x.GetLoadingSet().

			//ListReader<MyPhysicalItemDefinition> defs = MyDefinitionManager.Static.GetDefinitionsOfType<MyPhysicalItemDefinition>();

			if (MyDefinitionManager.Static.Definitions.Definitions.ContainsKey(def.Id.TypeId))
				MyDefinitionManager.Static.Definitions.Definitions[def.Id.TypeId].Add(def.Id.SubtypeId, def);
			else MyDefinitionManager.Static.Definitions.Definitions.Add(def.Id.TypeId, new Dictionary<MyStringHash, MyDefinitionBase>
			{
				{ def.Id.SubtypeId, def }
			});

			return MyDefinitionManager.Static.Definitions.Definitions.ContainsKey(def.Id.TypeId) && MyDefinitionManager.Static.Definitions.Definitions[def.Id.TypeId].ContainsKey(def.Id.SubtypeId);
		}

		private void SetupPhysicalItem()
		{
			// 1) Physical Item
			MyObjectBuilder_PhysicalItemDefinition newPhysDefOb = new MyObjectBuilder_PhysicalItemDefinition
			{
				Id = new SerializableDefinitionId
				{
					TypeId = typeof(MyObjectBuilder_Ore),
					SubtypeId = "BulletproofGlassScrap2"
				},
				DisplayName = "Bulletproof Glass Scrap 2",
				Icons = new[] { Constants.IconLocation + Constants.BulletproofGlassIcon },
				Size = new Vector3(0.2, 0.2, 0.1),
				Mass = 1,
				Volume = 0.254f,
				Model = Constants.KnownScrapModelLocation + Constants.Scrap2,
				PhysicalMaterial = "Metal"
			};

			// 1b) Physical Item Def -- May not be needed... 
			MyPhysicalItemDefinition newPhysItemDef = new MyPhysicalItemDefinition
			{
				Id = newPhysDefOb.Id,
				AvailableInSurvival = true
			};

			WriteToLog("Added", $"{ManualAddToDefinitions(newPhysItemDef)}", LogType.General);
			//WriteToLog("Added",$"{MyDefinitionManager.Static.Definitions.AddOrRelaceDefinition(newPhysItemDef)}",LogType.General);
		}

		private void SetupTestItem2()
		{
			// 1a) Physical Item Ob
			// 1b) Physical Item Def?
			// 1c) TODO: Add this... somewhere... 
			// 2) Blueprint (add 1 to 2)
			// 3a) Blueprint Class OB
			// 3b) Blueprint Class Def
			// 3a) Add 3a to 3b
			// 4a) Add 3b to Refineries
			// 4a) Add 3b to Skits [if applicable]

			// 1) Physical Item
			MyObjectBuilder_PhysicalItemDefinition newPhysDefOb = new MyObjectBuilder_PhysicalItemDefinition
			{
				Id = new SerializableDefinitionId
				{
					TypeId = typeof(MyObjectBuilder_Ore),
					SubtypeId = "BulletproofGlassScrap2"
				},
				DisplayName = "Bulletproof Glass Scrap 2",
				Icons = new[] { Constants.IconLocation + Constants.BulletproofGlassIcon },
				Size = new Vector3(0.2, 0.2, 0.1),
				Mass = 1,
				Volume = 0.254f,
				Model = Constants.KnownScrapModelLocation + Constants.Scrap2,
				PhysicalMaterial = "Metal"
			};

			// 1b) Physical Item Def -- May not be needed... 
			MyPhysicalItemDefinition newPhysItemDef = new MyPhysicalItemDefinition
			{
				Id = newPhysDefOb.Id
			};


			foreach (var test in MyDefinitionManager.Static.Definitions.Definitions[typeof(MyObjectBuilder_Ore)].Keys)
			{
				//test this	
			}

			// 2) Blueprint
			MyPhysicalItemDefinition siliconIngotDef = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Ingot), "Silicon"));
			MyPhysicalItemDefinition siliconOreDef = MyDefinitionManager.Static.GetPhysicalItemDefinition(new MyDefinitionId(typeof(MyObjectBuilder_Ore), "Silicon"));

			BlueprintItem preReq = new BlueprintItem
			{
				Id = newPhysItemDef.Id,
				Amount = "1"
			};

			BlueprintItem results = new BlueprintItem
			{
				Id = siliconIngotDef.Id,
				Amount = "1"
			};

			MyObjectBuilder_BlueprintDefinition newBpDefOb = new MyObjectBuilder_BlueprintDefinition
			{
				Id = new MyDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), "BulletproofGlassToIngot"),
				DisplayName = "Bulletproof Glass Scrap",
				Icons = new[]
				{
					Constants.IconLocation + Constants.BulletproofGlassIcon
				},
				Prerequisites = new[]
				{
					preReq
				},
				Results = new[]
				{
					results
				},

				BaseProductionTimeInSeconds = 0.04f
			};

			MyBlueprintDefinition newBpDef = new MyBlueprintDefinition
			{
				Id = newBpDefOb.Id
			};

			//MyBlueprintDefinition newBp = new MyBlueprintDefinition
			//{
			//	Id = new MyDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), "BulletproofGlassToIngot"),
			//	DisplayNameString = "Bulletproof Glass Scrap",
			//	Icons = new[]
			//	{
			//		Constants.IconLocation + Constants.BulletproofGlassIcon
			//	},
			//	Prerequisites = new[]
			//	{
			//		new MyBlueprintDefinitionBase.Item
			//		{
			//			Amount = 1,
			//			Id = siliconOreDef.Id
			//		}
			//	},
			//	Results = new[]
			//	{
			//		new MyBlueprintDefinitionBase.Item
			//		{
			//			Amount = (MyFixedPoint) 6.75,
			//			Id = siliconIngotDef.Id
			//		}
			//	},
			//	BaseProductionTimeInSeconds = 0.04f
			//};

			// 3a) Blueprint Class OB
			MyObjectBuilder_BlueprintClassDefinition newBpClassDefOb = new MyObjectBuilder_BlueprintClassDefinition
			{
				Id = new SerializableDefinitionId
				{
					TypeId = typeof(MyObjectBuilder_BlueprintClassDefinition),
					SubtypeId = "AwwScrap2"
				},
				DisplayName = "Scrap Recycling",
				Description = "Scrap Recycling",
				Icons = new[]
				{
					"Textures\\GUI\\Icons\\component\\ScrapMetalComponent.dds"
				},
				HighlightIcon = "Textures\\GUI\\Icons\\component\\ScrapMetalComponent.dds",
				InputConstraintIcon = "Textures\\GUI\\Icons\\filter_ore.dds",
				OutputConstraintIcon = "Textures\\GUI\\Icons\\filter_ingot.dds"
			};

			// 3b) Blueprint Class Definition
			MyBlueprintClassDefinition newBpClassDef = new MyBlueprintClassDefinition
			{
				Id = newBpClassDefOb.Id
			};

			// 3c) Add Blueprint to Blueprint Class (creates an entry)
			//x.AddBlueprint(newBpDef);
		}

		private void AddBlueprintClassToProductionBlocks(MyBlueprintClassDefinition newBpClassDef)
		{

			// 4a) Add Blueprint Class to MyRefineryDefinitions (large refinery, arc furnace, modded?)
			foreach (var def in MyDefinitionManager.Static.GetDefinitionsOfType<MyRefineryDefinition>())
			{
				WriteToLog("", $"{def}", LogType.General);
				def.BlueprintClasses.Add(newBpClassDef);

				foreach (var cls in def.BlueprintClasses)
				{
					WriteToLog("", $"{cls}", LogType.General);
				}
			}

			// 4b) If applicable, add Blueprint Class to MySurvivalKitDefinition (Skit, modded Skit?)
			foreach (var def in MyDefinitionManager.Static.GetDefinitionsOfType<MySurvivalKitDefinition>())
			{
				WriteToLog("", $"{def}", LogType.General);
				foreach (var cls in def.BlueprintClasses)
				{
					WriteToLog("", $"{cls}", LogType.General);
				}
			}
		}

		//private void PrintProductionBlocksDefinitions()
		//{
		//	foreach (var def in MyDefinitionManager.Static.GetDefinitionsOfType<MyRefineryDefinition>())
		//	{
		//		WriteToLog("", $"{def}", LogType.General);

		//		foreach (var cls in def.BlueprintClasses)
		//		{
		//			WriteToLog("", $"{cls}", LogType.General);
		//		}
		//	}

		//	foreach (var def in MyDefinitionManager.Static.GetDefinitionsOfType<MySurvivalKitDefinition>())
		//	{
		//		WriteToLog("", $"{def}", LogType.General);
		//		foreach (var cls in def.BlueprintClasses)
		//		{
		//			WriteToLog("", $"{cls}", LogType.General);
		//		}
		//	}
		//}

		private void SetupCollections()
		{
			foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
			{
				MyPhysicalItemDefinition physDef = def as MyPhysicalItemDefinition;
				if (physDef != null && physDef.Public)
				{
					if (physDef.Id.TypeId == typeof(MyObjectBuilder_Component))
						_components.Add(physDef);

					if (def.Id.TypeId == typeof(MyObjectBuilder_Ore))
						_ores.Add(physDef);
					continue;
				}

				MyCubeBlockDefinition blockDef = def as MyCubeBlockDefinition;
				if (blockDef != null && blockDef.Public)
					_cubes.Add(blockDef);
			}

			foreach (var bp in MyDefinitionManager.Static.GetBlueprintDefinitions())
			{
				MyBlueprintDefinition bpDef = bp as MyBlueprintDefinition;
				if (bpDef != null && bpDef.Public)
					_blueprints.Add(bpDef);
			}
		}

		private void ReportCollections()
		{
			WriteToLog(CompName, ToString(), LogType.General);
		}

		private void PrintProductionBlockDefinitions()
		{
			_report.Clear();

			_report.AppendLine("\n");
			_report.AppendLine("** MyProductionBlockDefinition Rundown **");
			_report.AppendLine();

			foreach (var def in MyDefinitionManager.Static.GetDefinitionsOfType<MyProductionBlockDefinition>())
			{
				_report.AppendLine();
				_report.AppendFormat("{0,-4}Definition Type: {1}", " ", def);
				_report.AppendLine();

				_report.AppendLine();
				_report.AppendFormat("{0,-8}Blueprint Class: {1}", " ", def);
				foreach (var bpc in def.BlueprintClasses)
				{
					_report.AppendFormat("{0,-12}Definition Key: {1}", " ", bpc);
					_report.AppendLine();
					_report.AppendFormat("{0,-12}TypeId: {1,-30} SubtypeId: {2} ", " ", bpc.Id, bpc.Id.SubtypeId);
					_report.AppendLine("\n");
				}
			}

			WriteToLog("", _report.ToString(), LogType.General);
		}

		private void ReportDefinitions()
		{
			_report.Clear();

			_report.AppendLine("\n");
			_report.AppendLine("** Definitions Rundown **");
			_report.AppendFormat("** Count: {0} **", MyDefinitionManager.Static.Definitions.Definitions.Count);
			_report.AppendLine("\n");

			foreach (var defDic in MyDefinitionManager.Static.Definitions.Definitions)
			{
				_report.AppendLine();
				_report.AppendFormat("{0,-4}Definition Type: {1}", " ", defDic.Key);
				//_report.AppendLine();
				//WriteToLog("Def", $"{defDic.Key}", LogType.General);
				//foreach (var defDicVal in defDic.Value)
				//{
				//	_report.AppendFormat("{0,-8}Definition Key: {1}", " ", defDicVal.Key);
				//	_report.AppendLine();
				//	_report.AppendFormat("{0,-12}Definition Value: {1}", " ", defDicVal.Value);
				//	_report.AppendLine("\n");
				//}
				//_report.AppendLine();
			}
			_report.AppendLine();
			WriteToLog("", _report.ToString(), LogType.General);
		}

		private void ReportDefinitionManager()
		{
			_report.Clear();
			_report.AppendLine("\n");
			_report.AppendLine("** Definition Manager Rundown **");
			_report.AppendLine();
			foreach (var def in MyDefinitionManager.Static.GetAllDefinitions())
			{
				_report.AppendFormat("{0,-4}Definition TypeId: {1}", " ", def.Id.TypeId);
				_report.AppendLine();
				_report.AppendFormat("{0,-4}Definition SubtypeId: {1}", " ", def.Id.SubtypeId);
				_report.AppendLine();
				_report.AppendFormat("{0,-4}Definition: {1}", " ", def);
				_report.AppendLine("\n");
			}
			_report.AppendLine();
			WriteToLog("", _report.ToString(), LogType.General);
		}

		private void DissectBlueprints()
		{
			_report.Clear();

			foreach (var bp in _blueprints)
			{
				_report.AppendLine("\n");
				_report.AppendLine("** Blueprint Rundown **");
				_report.AppendFormat("{0,-4} TypeId: {1,-20} SubtypeId: {2,-20}", " ", bp.Id.TypeId, bp.Id.SubtypeId);
				_report.AppendLine();

				_report.AppendLine();
				_report.AppendFormat("{0,4}** Results **", " ");
				_report.AppendLine();
				foreach (var re in bp.Results)
				{
					_report.AppendFormat("{0,-8} TypeId: {1,-20} SubtypeId: {2,-20} Amount: {3}", " ", re.Id.TypeId, re.Id.SubtypeId, re.Amount);
					_report.AppendLine();
				}

				_report.AppendLine();
				_report.AppendFormat("{0,4}** Prerequisites **", " ");
				_report.AppendLine();
				foreach (var pre in bp.Prerequisites)
				{
					_report.AppendFormat("{0,-8} TypeId: {1,-20} SubtypeId: {2,-20} Amount: {3}", " ", pre.Id.TypeId, pre.Id.SubtypeId, pre.Amount);
					_report.AppendLine();
				}
			}
			_report.AppendLine();
			WriteToLog("", _report.ToString(), LogType.General);
		}

		private readonly StringBuilder _report = new StringBuilder();
		private const string Indent = " ";

		public override string ToString()
		{
			_report.Clear();
			_report.AppendLine("\n");
			_report.AppendLine("AwwScrap2 Hashset Report");
			_report.AppendLine();

			_report.AppendLine();
			_report.AppendLine("** Blueprints **");
			_report.AppendFormat("{0,-4} Total: {1,-3}", Indent, _blueprints.Count);
			_report.AppendLine();
			foreach (var bp in _blueprints)
			{
				_report.AppendFormat("{0,-4} TypeId: {1} SubtypeId: {2}", Indent, bp.Id.TypeId, bp.Id.SubtypeId);
				_report.AppendLine();
			}

			//_report.AppendLine();
			//_report.AppendLine("** Components **");
			//_report.AppendFormat("{0,-4} Total: {1,-3}", Indent, _components.Count);
			//_report.AppendLine();
			//foreach (var comp in _components)
			//{
			//	_report.AppendFormat("{0,-4} TypeId: {1} SubtypeId: {2}", Indent, comp.Id.TypeId, comp.Id.SubtypeId);
			//	_report.AppendLine();
			//}

			_report.AppendLine();
			_report.AppendLine("** Ores **");
			_report.AppendFormat("{0,-4} Total: {1,-3}", Indent, _ores.Count);
			_report.AppendLine();
			foreach (var ore in _ores)
			{
				_report.AppendFormat("{0,-4} TypeId: {1} SubtypeId: {2}", Indent, ore.Id.TypeId, ore.Id.SubtypeId);
				_report.AppendLine();
			}

			//_report.AppendLine();
			//_report.AppendLine("** Cubes **");
			//_report.AppendFormat("{0,-4} Total: {1,-3}", Indent, _cubes.Count);
			//_report.AppendLine();
			//foreach (var cube in _cubes)
			//{
			//	_report.AppendFormat("{0,-4} TypeId: {1,-45} SubtypeId: {2}", Indent, cube.Id.TypeId, cube.Id.SubtypeId);
			//	_report.AppendLine();
			//}

			return _report.ToString();
		}
	}
}
