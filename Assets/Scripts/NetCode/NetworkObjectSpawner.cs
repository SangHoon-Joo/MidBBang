using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity​Engine.Assertions;

namespace ProjectWilson
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        protected NetworkObject _PrefabReference;

        public void Awake()
        {
            Assert.IsNotNull<NetworkObject>(_PrefabReference);
        }

        public virtual NetworkObject SpawnNetworkObject()
        {
            var instantiatedNetworkObject = Instantiate(_PrefabReference, transform.position, transform.rotation, null);
            SceneManager.MoveGameObjectToScene(instantiatedNetworkObject.gameObject,
                SceneManager.GetSceneByName(gameObject.scene.name));
                
            instantiatedNetworkObject.transform.localScale = transform.lossyScale;
            instantiatedNetworkObject.Spawn(destroyWithScene: true);

            return instantiatedNetworkObject;
        }
    }
}
