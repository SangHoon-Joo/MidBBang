using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class NetworkObjectSpawnManager : MonoBehaviourSingleton<NetworkObjectSpawnManager>
	{
		private List<NetworkObjectSpawner> _NetworkObjectSpawnerList = new List<NetworkObjectSpawner>();

		protected override void Awake()
		{
			base.Awake();

			foreach(var networkObjectSpawnerGameobject in GameObject.FindGameObjectsWithTag("NetworkObjectSpawner"))
			{
				NetworkObjectSpawner networkObjectSpawner = networkObjectSpawnerGameobject.GetComponent<NetworkObjectSpawner>();
				if(networkObjectSpawner == null)
					continue;
				_NetworkObjectSpawnerList.Add(networkObjectSpawner);
			}
		}

		public void SpawnAllNetworkObjects()
		{
			if(NetworkManager.Singleton == null)
				return;

			foreach(var networkObjectSpawner in _NetworkObjectSpawnerList)
			{
				if(NetworkManager.Singleton.IsServer)
				{
					if(networkObjectSpawner is NetworkNonPlayerSpawnerRepeat networkObjectSpawnerRepeat)
					{
						networkObjectSpawnerRepeat.SpawnNetworkObjectWaveRepeat();
						continue;
					}
					else
						networkObjectSpawner.SpawnNetworkObject();
				}
				
				Destroy(networkObjectSpawner.gameObject);
			}
		}
	}
}