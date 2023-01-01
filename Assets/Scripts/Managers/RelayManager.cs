using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Krafton.SP2.X1.Lib;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using ProjectWilson;

public class RelayManager : MonoBehaviourSingleton<RelayManager>
{
    private bool _CreatedRelay = false;
    public bool CreatedRelay => _CreatedRelay;
    private Allocation _HostAllocation;
    private JoinAllocation _JoinAllocation;

    public async Task Authenticate()
    {    
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(GameDataManager.Instance.Player.Name);

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            // do nothing
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> CreateRelay()
    {
        try
        {
            //await Authenticate();

            _HostAllocation = await RelayService.Instance.CreateAllocationAsync(1);
            //SetHostRelayData();

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(_HostAllocation.AllocationId);

            //RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            //NetworkManager.Singleton.StartHost();
            
            _CreatedRelay = true;
            //Debug.LogError($"[test] JoinCode : {joinCode}");
            return joinCode;
        }
        catch(RelayServiceException e)
        {
            Debug.LogError(e);
            return string.Empty;
        }
    }

    public void SetHostRelayData()
    {
        if(_HostAllocation == null)
        {
            Debug.LogError($"[test] _HostAllcoation is null!!!");
            return;
        }

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
            _HostAllocation.RelayServer.IpV4,
            (ushort)_HostAllocation.RelayServer.Port,
            _HostAllocation.AllocationIdBytes,
            _HostAllocation.Key,
            _HostAllocation.ConnectionData
        );
    }

    public void SetClientRelayData()
    {
        if(_JoinAllocation == null)
        {
            Debug.LogError($"[test] _JoinAllocation is null!!!");
            return;
        }

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
            _JoinAllocation.RelayServer.IpV4,
            (ushort)_JoinAllocation.RelayServer.Port,
            _JoinAllocation.AllocationIdBytes,
            _JoinAllocation.Key,
            _JoinAllocation.ConnectionData,
            _JoinAllocation.HostConnectionData
        );
    }

    public async Task JoinRelay(string joinCode)
    {
        try
        {
            //await Authenticate();
            Debug.LogWarning($"[test] JoinRelay : joinCode = {joinCode}");
            _JoinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            //RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            //NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            //SetClientRelayData();
            //NetworkManager.Singleton.StartClient();
            //SceneGameLobby.Instance.UIGameLobbyMain.gameObject.SetActive(false);
            
        }
        catch(RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }
}
