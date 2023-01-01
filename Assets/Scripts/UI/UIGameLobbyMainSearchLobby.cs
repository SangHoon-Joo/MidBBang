using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;

namespace ProjectWilson
{
	public class UIGameLobbyMainSearchLobby : MonoBehaviour
	{
		[SerializeField]
		private UIGameLobbyMainSearchLobbyPopup _UIGameLobbyMainSearchLobbyPopup;
		[SerializeField]
		private Button _MatchButton;
		[SerializeField]
		private TMP_Text _MatchText;

		private void Awake()
		{
			Assert.IsNotNull<UIGameLobbyMainSearchLobbyPopup>(_UIGameLobbyMainSearchLobbyPopup);
			Assert.IsNotNull<Button>(_MatchButton);
			Assert.IsNotNull<TMP_Text>(_MatchText);
		}

		private void Start()
		{
			LobbyManager.Instance.Init(GameDataManager.Instance.Player.Name);
		}

		private void OnEnable()
		{
			_UIGameLobbyMainSearchLobbyPopup.gameObject.SetActive(false);
			_UIGameLobbyMainSearchLobbyPopup.OnCancel += ActiveMatchButton;
			LobbyManager.Instance.OnReadyToSetSkill += ShowUIGameLobbyMainSearchLobbyPopup;
		}

		private void OnDisable()
		{
			_UIGameLobbyMainSearchLobbyPopup.gameObject.SetActive(false);
			_UIGameLobbyMainSearchLobbyPopup.OnCancel -= ActiveMatchButton;
			LobbyManager.Instance.OnReadyToSetSkill -= ShowUIGameLobbyMainSearchLobbyPopup;
		}

		public void OnClickMatch()
		{
			LobbyManager.Instance.MatchLobby();
			_MatchButton.interactable = false;
			_MatchText.text = $"Matching...";
		}

		private void ShowUIGameLobbyMainSearchLobbyPopup()
		{
			_UIGameLobbyMainSearchLobbyPopup.gameObject.SetActive(true);
		}

		private void ChangeModeToInLobby()
		{
			SceneGameLobby.Instance.UIGameLobbyMain.SetMode(UIGameLobbyMain.Mode.InLobby);
		}

		private void ActiveMatchButton()
		{
			_MatchButton.interactable = true;
			_MatchText.text = $"Match";
		}
	}
}