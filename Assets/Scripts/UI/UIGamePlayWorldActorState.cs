using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using ProjectWilson.Lookups;
using ProjectWilson.Actions;
using TMPro;
using Unity.Netcode;

namespace ProjectWilson
{
	public class UIGamePlayWorldActorState : MonoBehaviour
	{

		[SerializeField]
		private TextMeshProUGUI _NameText;
		[SerializeField]
		private AtlasImage _HPFillImage;
		private RectTransform _RectTransform;
		private Camera _UICamera;
		private NetworkActor _OwnerActor;
		private float _Height;
		private NetworkVariable<int> _MaxHP;
		private NetworkVariable<int> _HP;

		private void Awake()
		{
			Assert.IsNotNull<TextMeshProUGUI>(_NameText);
			Assert.IsNotNull<AtlasImage>(_HPFillImage);

			_RectTransform = transform as RectTransform;
		}

		public void Attach(NetworkActor ownerActor, RectTransform originalRectTransform)
		{
			_UICamera = UIHorizontalManager.Instance.UICamera;
			_OwnerActor = ownerActor;
			if(_OwnerActor is NetworkCharacter character)
				_Height = ownerActor.TableData.UIHUDStateHeight;
			//else if(_OwnerActor is NetworkProp prop)
			//	_Height = prop.TableData.UIHUDStateHeight;
			else
				_Height = 0f;

			_NameText.text = ownerActor.Name.ToString();
			_HP = ownerActor.CharacterHPState.HP;
			_MaxHP = ownerActor.CharacterHPState.MaxHP;
			RefreshHPFillAmount();
			UIHorizontalManager.Instance.AttachUI(gameObject, originalRectTransform, UIHorizontalManager.CanvasType.World);
			gameObject.SetActive(false);
		}

		public void Detach()
		{
			if(UIHorizontalManager.Instance != null)
				UIHorizontalManager.Instance.DetachUI(gameObject);
			_OwnerActor = null;
		}

		public void Show()
		{
			if(_OwnerActor == null || _OwnerActor.Equals(null))
				return;

			transform.SetAsLastSibling();

			_NameText.text = _OwnerActor.Name.ToString();
			ResetLocation();
			
			gameObject.SetActive(true);
			//_RectTransform.localScale = Vector3.zero;
			//_RectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		private void ResetLocation()
		{
			if(_OwnerActor == null || _OwnerActor.Equals(null))
				return;

			Camera camera = CameraManager.Instance.GetCamera();
			Vector3 displayLocation = _OwnerActor.transform.position + _OwnerActor.transform.up * _Height;
			_RectTransform.position = _UICamera.ViewportToWorldPoint(camera.WorldToViewportPoint(displayLocation));
			_RectTransform.anchoredPosition3D = new Vector3(_RectTransform.anchoredPosition3D.x, _RectTransform.anchoredPosition3D.y, 0.0f);
			_RectTransform.localScale = CameraManager.Instance.GetUIScale() * Vector3.one;
		}

		private void Update()
		{
			ResetLocation();
		}

		/*
		public void SetMaxHP(int maxHP)
		{
			Debug.LogWarning($"[test] UIGamePlayWorldActorState : MaxHP : {_MaxHP}");
			_MaxHP = maxHP;
			RefreshHPFillAmount();
		}

		public void SetHP(int newHP)
		{			
			_HP = newHP;
			RefreshHPFillAmount();
		}
		*/

		public void RefreshHPFillAmount()
		{
			if(_MaxHP.Value == 0)
				return;
			
			_HPFillImage.fillAmount = (float)_HP.Value / (float)_MaxHP.Value;
		}
	}
}