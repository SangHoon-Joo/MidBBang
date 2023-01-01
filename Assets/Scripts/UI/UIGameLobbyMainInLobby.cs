using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;

namespace ProjectWilson
{
	public class UIGameLobbyMainInLobby : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _PlayerText;
		[SerializeField]
		private TMP_Text _EnemyText;
		[SerializeField]
		private Button _ReadyButton;
		private bool _IsLobbyHost;

		private void Awake()
		{
			Assert.IsNotNull<TMP_Text>(_PlayerText);
			Assert.IsNotNull<TMP_Text>(_EnemyText);
			Assert.IsNotNull<Button>(_ReadyButton);
		}

		private void OnEnable()
		{
			_PlayerText.text = GameDataManager.Instance.Player.Name;

			foreach (Player player in LobbyManager.Instance.GetJoinedLobby().Players)
			{
				if(player.Id != AuthenticationService.Instance.PlayerId)
				{
					_EnemyText.text = player.Data["PlayerName"].Value;
					break;
				}
			}
			_ReadyButton.interactable = true;
			_IsLobbyHost = LobbyManager.Instance.IsLobbyHost();
			LobbyManager.Instance.OnStartGame += OnStartGame;
		}

		private void OnDisable()
		{
			LobbyManager.Instance.OnStartGame -= OnStartGame;
		}

		public void OnClickReady()
		{
			_ReadyButton.interactable = false;
			if(_IsLobbyHost)
			{
				LobbyManager.Instance.ReadyGameForLobbyHost();
			}
			else
			{
				LobbyManager.Instance.ReadyGameForLobbyGuest();
			}
		}

		private void OnStartGame()
		{
			SceneManager.LoadScene("SceneGamePlay", LoadSceneMode.Single);
		}
	}
}