using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Krafton.SP2.X1.Lib;
using Newtonsoft.Json;

namespace ProjectWilson
{
	public class GameDataManager : MonoBehaviourSingleton<GameDataManager>, ISaveData
	{
		private const string _CryptoKey = "ZUrQ~u5U<~z?{V3h$*yzsBG~9r!aN>";
		private string _SaveDirectoryName;
		private string _SaveFileName;
		private readonly List<ISaveData> _GameData = new List<ISaveData>();
		public PlayerGameData Player = new PlayerGameData();
		
		public class SaveData
		{
			public PlayerGameData.SaveData Player = new PlayerGameData.SaveData();
		}

		private GameDataManager()
		{
			_GameData.Add(Player);
		}

		public override void Init()
		{
			_SaveDirectoryName = $"{Application.persistentDataPath}/Playtest/Standalone";
			_SaveFileName = $"{_SaveDirectoryName}/{Application.productName}Game.sav";

			foreach(var gameData in _GameData)
			{
				gameData.Init();
			}
		}

		public override void Reset()
		{
			foreach(var gameData in _GameData)
			{
				gameData.Reset();
			}
		}

		public object Serialize()
		{
			SaveData saveData = new SaveData();
			saveData.Player = Player.Serialize() as PlayerGameData.SaveData;
			return saveData;
		}

		public void Deserialize(object objectData)
		{	
			SaveData saveData = objectData as SaveData;
			if(saveData == null)
				return;

			Player.Deserialize(saveData.Player);
		}

		public void Save()
		{
			string json = JsonConvert.SerializeObject(Serialize());
			json = CryptoHelper.EncryptStringToString(json, _CryptoKey);
			if(!Directory.Exists(_SaveDirectoryName))
				Directory.CreateDirectory(_SaveDirectoryName);
			File.WriteAllText(_SaveFileName, json);
		}

		public void Load()
		{
			Init();

			if(!File.Exists(_SaveFileName))
				return;

			Reset();
				
			string json = File.ReadAllText(_SaveFileName);
			json = CryptoHelper.DecryptStringFromString(json, _CryptoKey);
			if(json == null)
				return;

			SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);
			Deserialize(saveData);
		}

		public void Delete()
		{
			if(!File.Exists(_SaveFileName))
				return;

			File.Delete(_SaveFileName);
		}

		public bool IsExist()
		{
			return File.Exists(_SaveFileName);
		}
	}
}