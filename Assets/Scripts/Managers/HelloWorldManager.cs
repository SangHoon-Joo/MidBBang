using Krafton.SP2.X1.Lib;
using Unity.Netcode;
using UnityEngine;

namespace ProjectWilson
{
	public class HelloWorldManager : MonoBehaviourSingleton<HelloWorldManager>
	{
		void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 600, 600));
            if (!Unity.Netcode.NetworkManager.Singleton.IsClient && !Unity.Netcode.NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        static async void StartButtons()
        {
            if (GUILayout.Button("RelayCreate"))
            {
                await RelayManager.Instance.CreateRelay();
            }
            if (GUILayout.Button("RelayJoin"))
            {
                await RelayManager.Instance.JoinRelay("P6TkTP");
            }
            if (GUILayout.Button("Host"))
            {
                Unity.Netcode.NetworkManager.Singleton.StartHost();
            }
            
            if (GUILayout.Button("Client"))
            {
                if(Unity.Netcode.NetworkManager.Singleton.StartClient())
                    Debug.LogWarning($"[test] StartClient Success!");
                else
                    Debug.LogWarning($"[test] StartClient Fail!");
            }
            if (GUILayout.Button("Server")) Unity.Netcode.NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = Unity.Netcode.NetworkManager.Singleton.IsHost ? "Host" : Unity.Netcode.NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                Unity.Netcode.NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }
	}
}