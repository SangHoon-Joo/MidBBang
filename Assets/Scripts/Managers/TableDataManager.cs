using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class TableDataManager : MonoBehaviourSingleton<TableDataManager>, ITableData
	{
		private readonly List<ITableData> _TableData = new List<ITableData> ();
		
		public WorldTable World = new WorldTable();
		public ActorTable Actor = new ActorTable();
		public SkillTable Skill = new SkillTable();

		private TableDataManager()
		{
			_TableData.Add(World);
			_TableData.Add(Actor);
			_TableData.Add(Skill);
		}

		public override void Init()
		{
			base.Init();
		}
		
		public override void Reset()
		{
			base.Reset();

			Unload();
		}

		public void Load()
		{
			Unload();

			foreach(var tableData in _TableData)
			{
				tableData.Load();
			}
		}

		public async Task Wait()
		{
			foreach(var tableData in _TableData)
			{
				await tableData.Wait();
			}
		}

		public void Unload()
		{
			foreach(var tableData in _TableData)
			{
				tableData.Unload();
			}
		}

		public void Validate()
		{
			foreach(var tableData in _TableData)
			{
				tableData.Validate();
			}
		}
	}
}