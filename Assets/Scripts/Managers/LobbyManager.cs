using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Krafton.SP2.X1.Lib;
using System.Threading.Tasks;

public class LobbyManager : MonoBehaviourSingleton<LobbyManager>
{
    private Lobby _HostLobby;
    private Lobby _JoinedLobby;
    private List<Lobby> _JoinableLobbies;
    private float _HostLobbyHeartbeatTimer;
    private readonly float _MaxHostLobbyHeartbeatTimer = 15f;
    private float _JoinedLobbyUpdateTimer;
    private readonly float _MaxJoinedLobbyUpdateTimer = 1.1f;
    private string _PlayerName;
    private Player _Player;
    private Player _ClientPlayer;
    private bool _IsReadyGame;
    //private bool _IsReadyToSetSkills;
    public event System.Action OnReadyToSetSkill;
    public event System.Action OnStartGame;
    private readonly string _RelayJoinCode = $"RelayJoinCode";
    private readonly string _StartGame = $"StartGame";

    public void Init(string playerName)
    {
        _PlayerName = playerName;
        _IsReadyGame = false;
        Authenticate();
        SetPlayer();
    }

    public async void Authenticate()
    {    
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(_PlayerName);

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            // do nothing
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void MatchLobby()
    {
        await GetJoinableLobbyList();

        if(_JoinableLobbies == null || _JoinableLobbies.Count == 0)
            CreateLobby();
        else
            QuickJoinLobby();
    }

    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MidBBangLobby" + Random.Range(1, 100);
            int maxPlayers = 2;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>{
                    {_RelayJoinCode, new DataObject(DataObject.VisibilityOptions.Member, string.Empty)},
                    {_StartGame, new DataObject(DataObject.VisibilityOptions.Member, string.Empty)}
                }
            };
            _HostLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            _JoinedLobby = _HostLobby;
            Debug.LogWarning($"Create Lobby! Name : {lobbyName}, MaxPlayers : {maxPlayers}");
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private async Task GetJoinableLobbyList()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions {
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder> {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            _JoinableLobbies = queryResponse.Results;
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
            _JoinableLobbies = null;
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions {
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder> {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            Debug.LogWarning($"Lobbies found : {queryResponse.Results.Count}");
            foreach(var lobby in queryResponse.Results)
            {
                Debug.LogWarning($"Lobby name : {lobby.Name}, maxPlayers : {lobby.MaxPlayers}");
            } 
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleHostLobbyHeartbeat();
        HandleJoinedLobbyPollForUpdate();
    }

    private async void HandleHostLobbyHeartbeat()
    {
        if(_HostLobby == null)
            return;
        
        _HostLobbyHeartbeatTimer -= Time.deltaTime;
        if(_HostLobbyHeartbeatTimer < 0f)
        {
            _HostLobbyHeartbeatTimer = _MaxHostLobbyHeartbeatTimer;
            await LobbyService.Instance.SendHeartbeatPingAsync(_HostLobby.Id);
        }
    }

    private async void HandleJoinedLobbyPollForUpdate()
    {
        if(_JoinedLobby == null)
            return;
        
        _JoinedLobbyUpdateTimer -= Time.deltaTime;
        if(_JoinedLobbyUpdateTimer < 0f)
        {
            _JoinedLobbyUpdateTimer = _MaxJoinedLobbyUpdateTimer;
            _JoinedLobby =  await LobbyService.Instance.GetLobbyAsync(_JoinedLobby.Id);
            
            if(_JoinedLobby.AvailableSlots == 0)
                OnReadyToSetSkill?.Invoke();

            if(_IsReadyGame && !string.IsNullOrEmpty(_JoinedLobby.Data[_RelayJoinCode].Value))
            {
                _HostLobby = _JoinedLobby;
                UpdateStartGameStatus();
            }
            if(!string.IsNullOrEmpty(_JoinedLobby.Data[_StartGame].Value))
            {
                await RelayManager.Instance.JoinRelay(_JoinedLobby.Data[_RelayJoinCode].Value);
                OnStartGame?.Invoke();
                _JoinedLobby = null;
            }
        }
    }

    private async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            JoinLobbyByIdOptions joinLobbyByIdOptions = new JoinLobbyByIdOptions {
                Player = GetPlayer()
            };
            _JoinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id, joinLobbyByIdOptions);
            Debug.LogWarning($"Join Lobby! Name : {_JoinedLobby.Name}");
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions {
                Player = GetPlayer()
            };
            _JoinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            Debug.LogWarning($"Joined the Lobby : LobbyCode = {lobbyCode}");
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions {
                Player = GetPlayer()
            };
            _JoinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);
            Debug.LogWarning($"QuickJoin Lobby! Name : {_JoinedLobby.Name}");
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private void PrintPlayersInLobby(Lobby lobby)
    {
        Debug.LogWarning($"Players in Lobby {lobby.Name}");
        foreach (var player in lobby.Players)
        {
            Debug.LogWarning($"Player Id : {player.Id}, Player Name : {player.Data["PlayerName"].Value}");
        }
    }

    private void SetPlayer()
    {
        _Player = new Player {
            Data = new Dictionary<string, PlayerDataObject> {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, _PlayerName)}
            }
        };
    }

    private Player GetPlayer()
    {
        if(_Player == null)
            SetPlayer();

        return _Player;
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(_JoinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    public Lobby GetJoinedLobby() {
        return _JoinedLobby;
    }

    public bool IsLobbyHost() {
        return _JoinedLobby != null && _JoinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public async void ReadyGameForLobbyHost()
    {
        try
        {
            foreach(var player in _JoinedLobby.Players)
            {
                if(player.Id != AuthenticationService.Instance.PlayerId)
                    _ClientPlayer = player;
            }
            string relayJoinCode = await RelayManager.Instance.CreateRelay();
            _JoinedLobby = await Lobbies.Instance.UpdateLobbyAsync(_JoinedLobby.Id, new UpdateLobbyOptions{
                HostId = _ClientPlayer.Id,
                Data = new Dictionary<string, DataObject>{
                    { _RelayJoinCode, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode)}
                }
            });
            _HostLobby = null;
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }

    public void ReadyGameForLobbyGuest()
    {
        _IsReadyGame = true;
    }

    private async void UpdateStartGameStatus()
    {
        try
        {
            _JoinedLobby = await Lobbies.Instance.UpdateLobbyAsync(_JoinedLobby.Id, new UpdateLobbyOptions{
                Data = new Dictionary<string, DataObject>{
                    { _StartGame, new DataObject(DataObject.VisibilityOptions.Member, "StartGame")}
                }
            });
        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
    }
}
