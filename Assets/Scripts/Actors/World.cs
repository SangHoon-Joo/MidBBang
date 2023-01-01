using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Unity​Engine.ResourceManagement.AsyncOperations;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class World : Actor
	{
		public enum TerrainKind
		{
			None,
			Ground,
			Ocean,
			Max
		}

		private WorldData _TableData;

		List<Actor> _Actors = new List<Actor>();
		Dictionary<int, Actor> _ActorDictionary = new Dictionary<int, Actor>();
		
		private int _GroundLayer;
		private int _GroundLayerMask;
		private int _OceanLayer;
		private int _OceanLayerMask;

		private void InitActorRegister()
		{
			_Actors.Clear();
			_ActorDictionary.Clear();
		}

		public void RegisterActor(Actor actor)
		{
			if(!_Actors.Contains(actor))
			{
				_Actors.Add(actor);
				
				Assert.IsTrue(!_ActorDictionary.ContainsKey(actor.ID));
				if(!_ActorDictionary.ContainsKey(actor.ID))
					_ActorDictionary.Add(actor.ID, actor);
			}
		}

		public void UnregisterActor(Actor actor)
		{
			if(_Actors.Remove(actor))
				_ActorDictionary.Remove(actor.ID);
		}

		public int GetTotalActorCount()
		{
			return _Actors.Count;
		}

		public IEnumerable<Actor> GetAllActors()
		{
			foreach(var actor in _Actors)
			{
				yield return actor;
			}
		}

		public Actor GetActor(int actorID)
		{
			if(_ActorDictionary.ContainsKey(actorID))
				return _ActorDictionary[actorID];

			return null;
		}

		public IEnumerable<Actor> FindActors(Actor instigator, Type targetType, int targetActorID)
		{
			IEnumerable<Actor> targetActors = GetAllActors();

			foreach(Actor actor in targetActors)
			{
				if(actor == instigator)
					continue;

				if(targetActorID == 0 || actor.ID == targetActorID)
					yield return actor;
			}
		}

		public IEnumerable<Actor> FindActorsAround(Actor instigator, Type targetType, int targetActorID, float distance)
		{
			float sqrDistance = distance * distance;

			IEnumerable<Actor> targetActors = GetAllActors();

			foreach(Actor actor in targetActors)
			{
				if(actor == instigator)
					continue;

				float sqrActorDistance = (instigator.Position - actor.Position).sqrMagnitude;
				if(sqrActorDistance > sqrDistance)
					continue;

				if(targetActorID == 0 || actor.ID == targetActorID)
					yield return actor;
			}
		}

		public bool FindRandomPosition(Actor target, Vector3 centerPosition, float radius, out Vector3 position, int loopCount = 10)
		{
			position = Vector3.zero;

			for(int i = 0; i < loopCount; i++)
			{
				Vector2 randomPosition = UnityEngine.Random.Range(0.0f, radius) * UnityEngine.Random.insideUnitCircle.normalized;
				position = centerPosition + new Vector3(randomPosition.x, 0, randomPosition.y);
				if(GetTerrainKind(position) != TerrainKind.Ground)
					continue;

				if(!IsPositionOverlappingWithActors(position, target))
					return true;
			}

			Debug.LogWarning("FindRandomPosition failed.");
			return false;
		}

		public bool FindRandomPositionNearby(Actor instigator, float minSurfaceDistance, float maxSurfaceDistance, Actor target, out Vector3 position, int loopCount = 10)
		{
			position = Vector3.zero;

			for(int i = 0; i < loopCount; i++)
			{
				float distance = UnityEngine.Random.Range(minSurfaceDistance, maxSurfaceDistance);
				Vector2 randomPosition = UnityEngine.Random.insideUnitCircle.normalized * distance;
				position = instigator.Position + new Vector3(randomPosition.x, 0.0f, randomPosition.y);

				if(GetTerrainKind(position) != TerrainKind.Ground)
					continue;

				if(!IsPositionOverlappingWithActors(position, target))
					return true;
			}

			Debug.LogWarning("FindRandomPositionNearby failed.");
			return false;
		}

		public Vector3 FindMoveTargetPosition(Actor instigator, Actor target, float space = 0.0f)
		{
			return target.Position + (instigator.Position - target.Position).normalized * (instigator.Radius + target.Radius + space);
		}

		public bool IsPositionOverlappingWithActors(Vector3 position, Actor excludedActor)
		{
			bool isOverlaped = false;
			foreach(Actor actor in _Actors)
			{
				if(actor == excludedActor)
					continue;
					
				float sqrDistance = (position - actor.Position).sqrMagnitude;
				float sqrOverlapDistance = (excludedActor.Radius + actor.Radius) * (excludedActor.Radius + actor.Radius);
				if(sqrDistance < sqrOverlapDistance)
				{
					isOverlaped = true;
					break;
				}
			}
			
			return isOverlaped;
		}

		public TerrainKind GetTerrainKind(Vector3 position, float maxTerrainHeight = 5.0f)
		{
			Vector3 rayDirection = Vector3.down;
			Ray ray = new Ray(position - rayDirection * maxTerrainHeight, rayDirection);
			float distance = maxTerrainHeight + 5.0f;

			RaycastHit raycastHit;
			if(Physics.Raycast(ray, out raycastHit, distance, _GroundLayerMask | _OceanLayerMask))
			{
				if(raycastHit.collider.gameObject.layer == _GroundLayer)
				{
					return TerrainKind.Ground;
				}
				return TerrainKind.Ocean;
			}
			return TerrainKind.None;
		}

		public Vector3 GetTerrainPosition(Vector3 position, TerrainKind terrainKind, float maxTerrainHeight = 5.0f)
		{
			Vector3 rayDirection = Vector3.down;
			Ray ray = new Ray(position - rayDirection * maxTerrainHeight, rayDirection);
			float distance = maxTerrainHeight + 5.0f;
			
			RaycastHit raycastHit;
			if(terrainKind == TerrainKind.Ground && Physics.Raycast(ray, out raycastHit, distance, _GroundLayerMask))
				return raycastHit.point;
			else if(terrainKind == TerrainKind.Ocean && Physics.Raycast(ray, out raycastHit, distance, _OceanLayerMask))
				return raycastHit.point;

			return position;
		}

		public bool IsTerrain(Ray ray, TerrainKind terrainKind, out Vector3 position, float distance = Mathf.Infinity)
		{
			position = Vector3.zero;

			int layerMask = 0;
			switch(terrainKind)
			{
				case TerrainKind.Ground:
					layerMask = _GroundLayerMask;
					break;
				case TerrainKind.Ocean:
					layerMask = _OceanLayerMask;
					break;
				case TerrainKind.None:
				default:
					return false;
			}

			RaycastHit raycastHit;
			if(Physics.Raycast(ray, out raycastHit, distance, layerMask))
			{
				position = raycastHit.point;
				return true;
			}

			return false;
		}

		public bool DoesPositionOverlapWithActors(Vector3 position, float radius)
		{
			foreach(Actor actor in _Actors)
			{
				float sqrDistance = (position - actor.Position).sqrMagnitude;
				float sqrOverlapDistance = (radius + actor.Radius) * (radius + actor.Radius);
				if(sqrDistance < sqrOverlapDistance)
					return true;
			}
			
			return false;
		}

		/*
		public void SpawnAllOpenedCharacters()
		{
			foreach(var pair in TableDataManager.Instance.Character.Data)
			{
				if(pair.Value.Kind == CharacterData.Kinds.PC)
					continue;

				if(GameDataManager.Instance.Characters.IsOpened(pair.Key))
					StartCoroutine(SpawnCharacterCoroutine(pair.Key, GameDataManager.Instance.Characters[pair.Key].Position, GameDataManager.Instance.Characters[pair.Key].LocalEulerAngles, null));
			}
		}

		private IEnumerator SpawnCharacterCoroutine(int characterID, Vector3 position, Vector3 localEulerAngles, System.Action<Character> callback)
		{
			Assert.IsTrue(TableDataManager.Instance.Character.Data.ContainsKey(characterID));
			if(!TableDataManager.Instance.Character.Data.ContainsKey(characterID))
				yield break;

			CharacterData characterData = TableDataManager.Instance.Character.Data[characterID];

			AsyncOperationHandle<GameObject> gameObjectHandle = Addressables.InstantiateAsync(characterData.PrefabPath, position, Quaternion.Euler(localEulerAngles.x, localEulerAngles.y, localEulerAngles.z));
			yield return gameObjectHandle;

			if(gameObjectHandle.Status == AsyncOperationStatus.Succeeded)
			{
				GameObject characterGameObject = gameObjectHandle.Result;
				Assert.IsNotNull<GameObject>(characterGameObject, $"{characterData.ID.ToString()} {characterData.PrefabPath}");
				if(_CharactersTransform != null)
					characterGameObject.transform.SetParent(_CharactersTransform);
				Character character = characterGameObject.GetComponent<Character>();
				Assert.IsNotNull<Character>(character, $"{characterData.ID.ToString()} {characterData.PrefabPath}");
				character.Init(characterData.ID);
				character.Warp(position);

				ScenePlay.Instance.UIGameMain.UIGameMainCharacterFocus.Refresh();

				if(callback != null)
					callback(character);
			}
		}

		public bool IsAllOpenedCharactersSpawnFinished()
		{
			foreach(var pair in TableDataManager.Instance.Character.Data)
			{
				if(pair.Value.Kind == CharacterData.Kinds.PC)
					continue;

				if(GameDataManager.Instance.Characters.IsOpened(pair.Key))
				{
					if(GetCharacter(pair.Key) == null)
						return false;
				}
			}

			return true;
		}


		public void DestroyCharacter(Character character)
		{
			character.TerminateActionSequence();

			GameDataManager.Instance.Player.CharacterInventoryManagerForTest.Add(character.ID);
			GameDataManager.Instance.Save();
			
			UnregisterActor(character);
			if(!Addressables.ReleaseInstance(character.gameObject))
				GameObject.Destroy(character.gameObject);
		}
		*/

		protected override void Awake()
		{
			base.Awake();
			InitActorRegister();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
		}

		protected override void Start()
		{
			if(!_IsInitialized)
				return;

			base.Start();

			_TableData = TableDataManager.Instance.World.Data[ID];
			
			Name = _TableData.Name;
			Sight = 0.0f;
			_GroundLayer = LayerMask.NameToLayer("Ground");
			_GroundLayerMask = 1 << _GroundLayer;
			_OceanLayer = LayerMask.NameToLayer("Ocean");
			_OceanLayerMask = 1 << _OceanLayer;
		}

		protected override void OnDestroy()
		{
			foreach(var actor in _Actors)
			{
				if(!Addressables.ReleaseInstance(actor.gameObject))
					GameObject.Destroy(actor.gameObject);
			}
			_Actors.Clear();
			_ActorDictionary.Clear();
		}
	}
}