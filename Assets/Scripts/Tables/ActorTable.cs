using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	[Serializable]
	public class ActorTableData
	{
		public float ID;
		public string Name;
		public float Kind;
		public string Gender;
		public float HP;
		public float Sight;
		public float Speed;
		public float SummonOrder;
		public string FocusIconName;
		public string PrefabPath;
		public string UIPrefabPath;
		public float UIHUDStateHeight;
	}

	public class ActorData
	{
		public enum Kinds
		{
			PC	= 0,
			NPC,
			Prop,
			Max
		}
		
		public enum Genders
		{
			Male,
			Female,
			Genderlessness,

			Max
		}

		public int ID { private set; get; }
		public string Name { private set; get; }
		public Kinds Kind { private set; get; }
		public Genders Gender { private set; get; }
		public int HP { private set; get; }
		public float Sight { private set; get; }
		public float Speed { private set; get; }
		public int SummonOrder { private set; get; }
		public string FocusIconName { private set; get; }
		public string PrefabPath { private set; get; }
		public string UIPrefabPath { private set; get; }
		public float UIHUDStateHeight { private set; get; }

		public ActorData(ActorTableData characterTableData)
		{
			ID = (int)characterTableData.ID;
			Name = characterTableData.Name;
			Kind = (Kinds)characterTableData.Kind;
			Gender = Genders.Max;
			Genders gender;
			if(Enum.TryParse<Genders>(characterTableData.Gender, out gender))
				Gender = gender;
			HP = (int)characterTableData.HP;
			Sight = characterTableData.Sight;
			Speed = characterTableData.Speed;
			SummonOrder = (int)characterTableData.SummonOrder;
			FocusIconName = characterTableData.FocusIconName;
			PrefabPath = characterTableData.PrefabPath;
			UIPrefabPath = characterTableData.UIPrefabPath;
			UIHUDStateHeight = characterTableData.UIHUDStateHeight;
		}
	}

	public class ActorTable : JsonDataLoader<ActorTableData>, ITableData
	{
		public ReadOnlyDictionary<int, ActorData> Data
		{
			private set;
			get;
		}

		public ReadOnlyCollection<int> SummonOrderData
		{
			private set;
			get;
		}

		public int PlayerID
		{
			private set;
			get;
		}

		public ActorTable() : base("Assets/AddressableResources/Data/Actor.json") {}

		protected override void PostLoad(List<ActorTableData> rowData)
		{
			Dictionary<int, ActorData> data = new Dictionary<int, ActorData>();
			List<int> summonOrderData = new List<int>();
			for(int i = 0; i < rowData.Count; i++)
			{
				Assert.IsTrue(!data.ContainsKey((int)rowData[i].ID), rowData[i].ID.ToString());
				if(data.ContainsKey((int)rowData[i].ID))
					continue;

				ActorData characterData = new ActorData(rowData[i]);
				data.Add(characterData.ID, characterData);
				if(characterData.SummonOrder > 0)
					summonOrderData.Add(characterData.ID);

				if(characterData.Kind == ActorData.Kinds.PC)
					PlayerID = characterData.ID;
			}

			Data = new ReadOnlyDictionary<int, ActorData>(data);

			summonOrderData.Sort((int left, int right)=>
			{
				ActorData leftCharacterData = Data[left];
				ActorData rightCharacterData = Data[right];
				return leftCharacterData.SummonOrder.CompareTo(rightCharacterData.SummonOrder);
			});
			SummonOrderData = new ReadOnlyCollection<int>(summonOrderData);
		}

		public override void Unload()
		{
			Data = null;
		}

		public void Validate()
		{
			foreach(var pair in Data)
			{
				Assert.IsTrue(pair.Value.Gender != ActorData.Genders.Max, $"{pair.Value.ID} {pair.Key.ToString()}");
				Assert.IsTrue(pair.Value.SummonOrder >= 0);
			}
		}
	}
}