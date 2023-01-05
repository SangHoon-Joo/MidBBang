/*
using UnityEngine;
using UnityEngine.UI;
using Mosframe;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace ProjectWilson
{
	public class UIInventoryBoxItem : MonoBehaviour, IDynamicScrollViewItem, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
	{
		[SerializeField]
		private ScrollRect _ScrollRect;
		protected bool _IsScrolling = false;
		[SerializeField]
		protected UIInventoryBox _UIInventoryBox;
		[SerializeField]
		protected AtlasImage _IconImage;
		[SerializeField]
		protected Text _CountText;
		public int ItemID { get; protected set; }
		public string ItemName { get; protected set; }
		public string ItemDescription { get; protected set; }

		protected virtual void Awake()
		{
			Assert.IsNotNull<ScrollRect>(_ScrollRect);
			Assert.IsNotNull<UIInventoryBox>(_UIInventoryBox);
		}
		
		public virtual void onUpdateItem( int index )
		{
			_UIInventoryBox.Subscribe(this);
			ItemID = _UIInventoryBox.GetItemIDByIndex(index);
			int totalIndexCount = _UIInventoryBox.GetItemIDListCount();

			if(index >= totalIndexCount || !TableDataManager.Instance.Item.Data.ContainsKey(ItemID))
			{
				_IconImage.spriteName = "None";
				_CountText.text = string.Empty;
				ItemID = 0;
				ItemName = string.Empty;
				return;
			}

			_IconImage.spriteName = TableDataManager.Instance.Item.Data[ItemID].IconName;
			int itemCount = GameDataManager.Instance.Player.GetItemCount(ItemID);
			if(itemCount > 1)
				_CountText.text = itemCount.ToString();
			else
				_CountText.text = string.Empty;
			
			ItemName = TableDataManager.Instance.Item.Data[ItemID].Name;
			ItemDescription = TableDataManager.Instance.Item.Data[ItemID].Description;
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if(_ScrollRect != null && Mathf.Abs(eventData.delta.x) <= 5.0f)
			{
				_IsScrolling = true;
				_ScrollRect.OnBeginDrag(eventData);
				return;
			}
		}
		public void OnDrag(PointerEventData eventData)
		{
			if(_IsScrolling)
			{
				_ScrollRect.OnDrag(eventData);
				return;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if(_IsScrolling)
			{
				_IsScrolling = false;
				_ScrollRect.OnEndDrag(eventData);
				return;
			}
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
        {
			_UIInventoryBox.OnItemEnter(this);
        }

		public virtual void OnPointerClick(PointerEventData eventData)
        {
            _UIInventoryBox.OnItemClick(this);
        }

		public virtual void OnPointerExit(PointerEventData eventData)
        {
            _UIInventoryBox.OnItemExit(this);
        }
    }
}
*/