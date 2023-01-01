using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
//using UnityEngine.AddressableAssets;
//using Unity​Engine.ResourceManagement.AsyncOperations;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Krafton.SP2.X1.Lib;
//using DG.Tweening;

namespace ProjectWilson
{
	public class UIHorizontalManager : MonoBehaviourSingleton<UIHorizontalManager>
	{
		public enum CanvasType
		{
			World,
			Main,
			Sub,
			Popup,
			Effect,
			Loading,

			Max
		}

		[SerializeField]
		private Camera _UICamera;
		public Camera UICamera { get { return _UICamera; }}
		[SerializeField]
		private Canvas _WorldCanvas;
		[SerializeField]
		private Canvas _MainCanvas;
		[SerializeField]
		private CanvasScaler _MainCanvasScaler;
		[SerializeField]
		private Canvas _SubCanvas;
		[SerializeField]
		private Canvas _PopupCanvas;
		[SerializeField]
		private Canvas _EffectCanvas;
		[SerializeField]
		private Canvas _LoadingCanvas;
		[SerializeField]
		private CanvasGroup[] _CanvasGroups;

		public float CanvasWidth
		{
			get { return (_MainCanvas.transform as RectTransform).rect.width; }
		}

		public float CanvasHeight
		{
			get { return (_MainCanvas.transform as RectTransform).rect.height; }
		}

		public Vector2 CanvasReferenceResolution
		{
			get { return _MainCanvasScaler.referenceResolution; }
		}

		protected override void Awake()
		{
			base.Awake();

			Assert.IsNotNull<Camera>(_UICamera);
			Assert.IsNotNull<Canvas>(_WorldCanvas);
			Assert.IsNotNull<Canvas>(_MainCanvas);
			Assert.IsNotNull<CanvasScaler>(_MainCanvasScaler);
			Assert.IsNotNull<Canvas>(_SubCanvas);
			Assert.IsNotNull<Canvas>(_PopupCanvas);
			Assert.IsNotNull<Canvas>(_EffectCanvas);
			Assert.IsNotNull<Canvas>(_LoadingCanvas);
			Assert.IsTrue(_CanvasGroups.Length > 0);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		private void SetCanvasGroupsAlpha(float alpha)
		{
			for(int i = 0; i < _CanvasGroups.Length; i++)
			{
				_CanvasGroups[i].alpha = alpha;
			}
		}

		public void AttachUI(GameObject ui, RectTransform originalRectTransform, CanvasType canvasType)
		{
			Canvas targetCanvas = null;
			switch(canvasType)
			{
			case CanvasType.World:
				targetCanvas = _WorldCanvas;
				break;
			case CanvasType.Main:
				targetCanvas = _MainCanvas;
				break;
			case CanvasType.Sub:
				targetCanvas = _SubCanvas;
				break;
			case CanvasType.Popup:
				targetCanvas = _PopupCanvas;
				break;
			case CanvasType.Effect:
				targetCanvas = _EffectCanvas;
				break;
			case CanvasType.Loading:
				targetCanvas = _LoadingCanvas;
				break;
			}
			if(targetCanvas != null)
			{
				if(ui.transform is RectTransform rectTransform)
				{
					if(originalRectTransform != null)
					{
						Vector3 anchoredPosition3D = originalRectTransform.anchoredPosition3D;
						Vector2 anchorMin = originalRectTransform.anchorMin;
						Vector2 anchorMax = originalRectTransform.anchorMax;
						Vector2 sizeDelta = originalRectTransform.sizeDelta;
						Vector2 pivot = originalRectTransform.pivot;
						Vector2 offsetMin = originalRectTransform.offsetMin;
						Vector2 offsetMax = originalRectTransform.offsetMax;
						Quaternion localRotation = originalRectTransform.localRotation;
						Vector3 localScale = originalRectTransform.localScale;

						rectTransform.SetParent(targetCanvas.transform);

						rectTransform.anchoredPosition3D = anchoredPosition3D;
						rectTransform.anchorMin = anchorMin;
						rectTransform.anchorMax = anchorMax;
						rectTransform.sizeDelta = sizeDelta;
						rectTransform.pivot = pivot;
						rectTransform.offsetMin = offsetMin;
						rectTransform.offsetMax = offsetMax;
						rectTransform.localRotation = localRotation;
						rectTransform.localScale = localScale;
					}
					else
					{
						rectTransform.SetParent(targetCanvas.transform);

						rectTransform.anchoredPosition3D = Vector3.zero;
						rectTransform.offsetMin = Vector2.zero;
						rectTransform.offsetMax = Vector2.zero;
						rectTransform.localEulerAngles = Vector3.zero;
						rectTransform.localScale = Vector3.one;
					}
				}
			}
		}

		public void DetachUI(GameObject ui)
		{
			RectTransform rectTransform = ui.transform as RectTransform;
			rectTransform.SetParent(null);
		}

		public bool IsPositionOverGameObject(Vector2 screenPosition)
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = screenPosition;
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}

		public bool IsPositionOverGameObject(Vector2 screenPosition, out RaycastResult raycastResult)
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = screenPosition;
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			raycastResult = results.Count > 0 ? results[0] : default(RaycastResult);
			return results.Count > 0;
		}
	}
}