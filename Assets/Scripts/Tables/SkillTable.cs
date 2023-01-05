using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	[Serializable]
	public class SkillTableData
	{
		public float ID;
		public string Name;
		public float Latitude;
		public float Longitude;
		public float Kind;
		public float CoolTime;
		public float DurationTime;
		public float AttackRange;
		public float AttackDamage;
		public string Icon;
	}

	public class SkillData
	{
		public enum Kinds
		{
			Normal	= 0,
			Special,
			Max
		}

		public int ID { private set; get; }
		public string Name { private set; get; }
		public float Latitude { private set; get; }
		public float Longitude { private set; get; }
		public Kinds Kind { private set; get; }
		public float CoolTime { private set; get; }
		public float DurationTime { private set; get; }
		public float AttackRange { private set; get; }
		public int AttackDamage { private set; get; }
		public string Icon { private set; get; }

		public SkillData(SkillTableData skillTableData)
		{
			ID = (int)skillTableData.ID;
			Name = skillTableData.Name;
			Latitude = skillTableData.Latitude;
			Longitude = skillTableData.Longitude;
			Kind = (Kinds)skillTableData.Kind;
			CoolTime = skillTableData.CoolTime;
			DurationTime = skillTableData.DurationTime;
			AttackRange = skillTableData.AttackRange;
			AttackDamage = (int)skillTableData.AttackDamage;
			Icon = skillTableData.Icon;
		}
	}

	public class SkillTable : JsonDataLoader<SkillTableData>, ITableData
	{
		public ReadOnlyDictionary<int, SkillData> Data
		{
			private set;
			get;
		}

		public SkillTable() : base("Assets/AddressableResources/Data/Skill.json") {}

		protected override void PostLoad(List<SkillTableData> rowData)
		{
			Dictionary<int, SkillData> data = new Dictionary<int, SkillData>();
			for(int i = 0; i < rowData.Count; i++)
			{
				Assert.IsTrue(!data.ContainsKey((int)rowData[i].ID), rowData[i].ID.ToString());
				if(data.ContainsKey((int)rowData[i].ID))
					continue;

				SkillData skillData = new SkillData(rowData[i]);
				data.Add(skillData.ID, skillData);
			}

			Data = new ReadOnlyDictionary<int, SkillData>(data);
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