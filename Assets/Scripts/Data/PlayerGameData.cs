using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class PlayerGameData : ISaveData
	{
		private readonly List<ISaveData> _GameData = new List<ISaveData> ();
		public string Name { set; get; }

		public class SaveData
		{
			public string Name;
		}

		public PlayerGameData()
		{
			
		}

		public void Init()
		{
			Name = string.Empty;
		}

		public void Reset()
		{
			Name = string.Empty;
		}

		public object Serialize()
		{
			SaveData saveData = new SaveData();
			saveData.Name = Name;
			return saveData;
		}

		public void Deserialize(object objectData)
		{
			SaveData saveData = objectData as SaveData;
			if(saveData == null)
				return;

			Name = saveData.Name;
		}
	}
}