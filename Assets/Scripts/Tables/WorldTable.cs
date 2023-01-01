using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	[Serializable]
	public class WorldTableData
	{
		public float ID;
		public string Name;
		public string ShortDescription;
	}

	public class WorldData
	{
		public int ID { private set; get; }
		public string Name { private set; get; }
		public string ShortDescription { private set; get; }

		public WorldData(WorldTableData worldTableData)
		{
			ID = (int)worldTableData.ID;
			Name = worldTableData.Name;
			ShortDescription = worldTableData.ShortDescription ?? string.Empty;
		}
	}

	public class WorldTable : JsonDataLoader<WorldTableData>, ITableData
	{
		public ReadOnlyDictionary<int, WorldData> Data
		{
			private set;
			get;
		}

		public WorldTable() : base("Assets/AddressableResources/Data/World.json") {}

		protected override void PostLoad(List<WorldTableData> rowData)
		{
			Dictionary<int, WorldData> data = new Dictionary<int, WorldData>();
			for(int i = 0; i < rowData.Count; i++)
			{
				Assert.IsTrue(!data.ContainsKey((int)rowData[i].ID), rowData[i].ID.ToString());
				if(data.ContainsKey((int)rowData[i].ID))
					continue;
				
				WorldData WorldData = new WorldData(rowData[i]);
				data.Add(WorldData.ID, WorldData);
			}

			Data = new ReadOnlyDictionary<int, WorldData>(data);
		}

		public override void Unload()
		{
			Data = null;
		}

		public void Validate()
		{
			foreach(var pair in Data)
			{
				Assert.IsTrue(!string.IsNullOrEmpty(pair.Value.ShortDescription), $"{pair.Value.ID.ToString()}");
			}
		}
	}
}