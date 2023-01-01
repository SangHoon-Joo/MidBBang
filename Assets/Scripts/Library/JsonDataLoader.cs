using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity​Engine.ResourceManagement.AsyncOperations;

namespace Krafton.SP2.X1.Lib
{
	public abstract class JsonDataLoader<T> where T : class
	{
		private readonly string _JsonFilePath;
		private AsyncOperationHandle<TextAsset> _TextAssetHandle;
		
		public struct JsonData
		{
			public List<T> Rows;
		}

		public JsonDataLoader(string jsonFilePath)
		{
			_JsonFilePath = jsonFilePath;
		}

		public void Load()
		{
			if(string.IsNullOrEmpty(_JsonFilePath))
				return;

			_TextAssetHandle = Addressables.LoadAssetAsync<TextAsset>(_JsonFilePath);
		}

		public async Task Wait()
		{
			if(string.IsNullOrEmpty(_JsonFilePath))
			{
				PostLoad(new List<T>());
				Debug.Log($"[JsonDataLoad] {_JsonFilePath} Failed.");
				return;
			}

			await _TextAssetHandle.Task;
			
			List<T> rowData = null;
			if(_TextAssetHandle.Status == AsyncOperationStatus.Succeeded)
			{
				string jsonString = string.Concat("{\"Rows\":", _TextAssetHandle.Result.text, "}");
				JsonData jsonData = JsonUtility.FromJson<JsonData>(jsonString);
				rowData = jsonData.Rows;
			}
			Addressables.Release(_TextAssetHandle);

			if(rowData != null)
			{
				PostLoad(rowData);
				Debug.Log($"[JsonDataLoad] {_JsonFilePath} Success.");
				return;
			}
			
			PostLoad(new List<T>());
			Debug.Log($"[JsonDataLoad] {_JsonFilePath} Failed.");
		}

		protected abstract void PostLoad(List<T> rowData);
		public abstract void Unload();
	}
}
