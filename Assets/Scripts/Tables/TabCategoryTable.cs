using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	[Serializable]
	public class TabCategoryTableData
	{
		public string GroupName;
		public float Category;
		public string CategoryName;
		public string CategoryIconName;
	}

	public class TabCategoryData
	{
		public enum LobbyCategories
		{
			A	= 0,
			B,
			S,
			Max
		}

		public string GroupName { private set; get; }
		public int Category { private set; get; }
		public string CategoryName { private set; get; }
		public string CategoryIconName { private set; get; }

		public TabCategoryData(TabCategoryTableData tabCategoryTableData)
		{
			GroupName = tabCategoryTableData.GroupName;
			Category = (int)tabCategoryTableData.Category;
			CategoryName = tabCategoryTableData.CategoryName;
			CategoryIconName = tabCategoryTableData.CategoryIconName;
		}
	}

	public class TabCategoryTable : JsonDataLoader<TabCategoryTableData>, ITableData
	{
		public ReadOnlyDictionary<string, ReadOnlyDictionary<int, TabCategoryData>> Data
		{
			private set;
			get;
		}

		public TabCategoryTable() : base("Assets/AddressableResources/Data/TabCategory.json") {}

		protected override void PostLoad(List<TabCategoryTableData> rowData)
		{
			Dictionary<string, Dictionary<int, TabCategoryData>> data = new Dictionary<string, Dictionary<int, TabCategoryData>>();
			for(int i = 0; i < rowData.Count; i++)
			{
				if(data.ContainsKey(rowData[i].GroupName) && data[rowData[i].GroupName].ContainsKey((int)rowData[i].Category))
					continue;

				TabCategoryData tabCategoryData = new TabCategoryData(rowData[i]);
				if(!data.ContainsKey(tabCategoryData.GroupName))
					data.Add(tabCategoryData.GroupName, new Dictionary<int, TabCategoryData>());
				
				data[tabCategoryData.GroupName].Add(tabCategoryData.Category, tabCategoryData);
			}

			Dictionary<string, ReadOnlyDictionary<int, TabCategoryData>> data2 = new Dictionary<string, ReadOnlyDictionary<int, TabCategoryData>>();
			foreach(var pair in data)
			{
				data2.Add(pair.Key, new ReadOnlyDictionary<int, TabCategoryData>(pair.Value));
			}
			Data = new ReadOnlyDictionary<string, ReadOnlyDictionary<int, TabCategoryData>>(data2);
		}

		public override void Unload()
		{
			Data = null;
		}

		public void Validate()
		{
		
		}
	}
}