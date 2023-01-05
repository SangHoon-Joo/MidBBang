/*
using UnityEngine;
using UnityEngine.Assertions;
using Mosframe;
using System.Collections.Generic;


namespace ProjectWilson
{
	public class UIInventoryBox : MonoBehaviour
	{
		[SerializeField]
		protected DynamicGridVScrollView _DynamicGridVScrollView;
		protected List<int> _ItemIDList = new List<int>();
		protected List<UIInventoryBoxItem> _ItemList = new List<UIInventoryBoxItem>();
		public UIInventoryBoxItem SelectedItem { protected set; get; }
		
		public void Subscribe(UIInventoryBoxItem item)
		{
			_ItemList.Add(item);
		}

		protected virtual void Awake()
		{
			Assert.IsNotNull<DynamicGridVScrollView>(_DynamicGridVScrollView);
		}

		protected virtual void OnEnable()
		{
			if(CameraManager.Instance != null)
				CameraManager.Instance.Lock();
		}

		protected virtual void OnDisable()
		{
			if(CameraManager.Instance != null)
				CameraManager.Instance.Unlock();
		}

		public virtual void Refresh()
		{
			ForceResetItems();
			_ItemIDList.Clear();
			_ItemList.Clear();
		}

		public virtual void OnItemEnter(UIInventoryBoxItem button)
        {
			if(button.ItemID == 0)
				return;
        }

        public virtual void OnItemClick(UIInventoryBoxItem button)
        {
			if(button.ItemID == 0)
				return;

            SelectedItem = button;
        }

        public virtual void OnItemExit(UIInventoryBoxItem button)
        {
			if(string.IsNullOrEmpty(button.ItemName))
				return;
        }

        protected void ForceResetItems()
        {
			SelectedItem = null;
		}

		public virtual int GetItemIDByIndex(int index)
        {
            if(index < 0 || index >= _ItemIDList.Count)
               return 0;

            return _ItemIDList[index];
        }

		public int GetItemIDListCount()
        {
            return _ItemIDList.Count;
        }
	}
}
*/