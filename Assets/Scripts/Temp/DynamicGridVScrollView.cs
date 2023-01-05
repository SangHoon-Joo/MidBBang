/*
 * DynamicCustomScrollView.cs
 * 
 * @author mosframe / https://github.com/mosframe
 * 
 

 namespace Mosframe {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections;
    using UnityEngine.Assertions;

    /// <summary>
    /// Dynamic Scroll View
    /// </summary>
    
    [RequireComponent(typeof(ScrollRect))]
    [AddComponentMenu("UI/Dynamic Grid V Scroll View")]
    public class DynamicGridVScrollView : UIBehaviour
    {

	    public int TotalItemCount = 99;
        public bool HasMinimumItemCount = false;
        public int MinimumItemCount = 18;
        [SerializeField]
	    private RectTransform   _ItemPrototype    = null;
        public bool KeepItemPrototypeWidth = false;
        public bool KeepItemPrototypeHeight = false;
        [SerializeField]
        private int _ColumnCount = 5;
        private float ContentAnchoredPosition { get { return -this._ContentRect.anchoredPosition.y; } set { this._ContentRect.anchoredPosition = new Vector2( this._ContentRect.anchoredPosition.x, -value ); } }
	    private float ContentSize             { get { return this._ContentRect.rect.height; } }
	    private float _ViewportSize { get { return this._ViewportRect.rect.height;} }

        private LinkedList<RectTransform> _Containers = new LinkedList<RectTransform>();
        private float _PrevAnchoredPosition = 0;
	    private int _NextInsertItemNo = 0; // item index from left-top of viewport at next insert
	    private int _PrevTotalItemCount = 99;
        [SerializeField]
        private ScrollRect _ScrollRect;
        [SerializeField]
        private RectTransform _ViewportRect;
        [SerializeField]
        private RectTransform _ContentRect;
        private float _ItemWidthSize;
        private float _ItemHeightSize;
        private float _MiddleGapSize;
        private int _HalfColumnCount;
        private bool _HasGap;
        [SerializeField]
        private float _ItemMargin = 15;

        public float leftMargin = 0f;
        public float topMargin = 0f;

        public void SetMiddleGapSize(float middleGapSize=0.0f)
        {
            if(middleGapSize <= 0.0f)
                return;

            Assert.IsTrue(_ColumnCount % 2 == 0);
            if (KeepItemPrototypeWidth)
                _ItemWidthSize = _ItemPrototype.sizeDelta.x;
            else
                _ItemWidthSize = (_ContentRect.rect.width - middleGapSize - (_ItemMargin * (_ColumnCount - 2))) / (float)_ColumnCount;
            if(KeepItemPrototypeHeight)
                _ItemHeightSize = _ItemPrototype.sizeDelta.y;
            else
                _ItemHeightSize = _ItemWidthSize;
            _MiddleGapSize = middleGapSize;
            _HasGap = true;
            _HalfColumnCount = (int)(_ColumnCount / 2);
        }

        public void scrollToLastPos () {

            this.ContentAnchoredPosition = this._ViewportSize - this.ContentSize;
            this.refresh();
        }
        public void scrollByItemIndex ( int itemIndex ) {
             if(this.TotalItemCount == 0)
                return;

            var totalLen = this.ContentSize;
            var itemLen  = totalLen / this.TotalItemCount;
            var pos = itemLen * itemIndex;
            this.ContentAnchoredPosition = -pos;
        }
        public void refresh () {
            var index = 0;
            
            if( this.ContentAnchoredPosition != 0 ) {
                index = (int)(-this.ContentAnchoredPosition / this._ItemHeightSize);
            }
        
            foreach( var itemRect in  this._Containers ) {

                // set item position
                var column = index % _ColumnCount;
                var posX = (this._ItemWidthSize + _ItemMargin) * column;
                posX += leftMargin;
                if(_HasGap && column >= _HalfColumnCount)
                    posX += _MiddleGapSize - _ItemMargin;
                var posY = (this._ItemHeightSize + _ItemMargin) * (index / _ColumnCount);
                posY += topMargin;
			    itemRect.anchoredPosition = new Vector2(posX, -posY);
                this.updateItem( index, itemRect.gameObject );

                ++index;
            }

            this._NextInsertItemNo = index - this._Containers.Count;
            this._PrevAnchoredPosition = (int)(this.ContentAnchoredPosition / this._ItemHeightSize) * this._ItemHeightSize;
        }


        protected override void Awake () {

            if( this._ItemPrototype == null ) {
                Debug.LogError( RichText.Red(new{this.name,this._ItemPrototype}) );
                return;
            }
            
            base.Awake();

            Assert.IsNotNull<ScrollRect>(_ScrollRect);
            Assert.IsNotNull<RectTransform>(_ViewportRect);
            Assert.IsNotNull<RectTransform>(_ContentRect);
    
            ContentAnchoredPosition = 0.0f;
            if (KeepItemPrototypeWidth)
                _ItemWidthSize = _ItemPrototype.sizeDelta.x;
            else
                _ItemWidthSize = (this._ContentRect.rect.width - _ItemMargin * (_ColumnCount - 1)) / (float)_ColumnCount;
            if(KeepItemPrototypeHeight)
                _ItemHeightSize = _ItemPrototype.sizeDelta.y;
            else
                _ItemHeightSize = _ItemWidthSize;
            _HasGap = false;
        }
        protected override void Start () {

            this._PrevTotalItemCount = this.TotalItemCount;

            this.StartCoroutine( this.onSeedData() );
	    }

        protected virtual IEnumerator onSeedData() {

            //yield return null;

            // hide prototype

            this._ItemPrototype.gameObject.SetActive(false);

            // instantiate items
            _ItemPrototype.sizeDelta = new Vector2(_ItemWidthSize, _ItemHeightSize);

            var itemCount = ((int)(this._ViewportSize / this._ItemHeightSize) + 3)*_ColumnCount;
            if(HasMinimumItemCount)
                itemCount = Mathf.Max(itemCount, MinimumItemCount);
            
		    for( var i = 0; i < itemCount; ++i ) {

			    var itemRect = Instantiate( this._ItemPrototype );
			    itemRect.SetParent( this._ContentRect, false );
			    itemRect.name = i.ToString();
                int row = i / _ColumnCount;
                int column = i % _ColumnCount;

                var posX = (_ItemWidthSize + _ItemMargin) * column;
                posX += leftMargin;
                if(_HasGap && column >= _HalfColumnCount)
                    posX += _MiddleGapSize - _ItemMargin;              

			    itemRect.anchoredPosition = new Vector2(posX, -(this._ItemHeightSize + _ItemMargin) * row - topMargin);
                this._Containers.AddLast( itemRect );

			    itemRect.gameObject.SetActive( true );

				this.updateItem( i, itemRect.gameObject );
		    }


            // resize content

			this.resizeContent();

			yield break;
        }

		public void forceUpdate()
		{
			Update();
		}


	    private void Update () {

            if( this.TotalItemCount != this._PrevTotalItemCount ) {

                this._PrevTotalItemCount = this.TotalItemCount;

                this.resizeContent();

				if( this._ViewportSize <= this.ContentSize && this._ViewportSize-this.ContentAnchoredPosition >= this.ContentSize-(this._ItemHeightSize + 0.5f*_ItemMargin)*0.5f )
                {
                    this.ContentAnchoredPosition = this._ViewportSize - this.ContentSize;
                }

                this.refresh();
            }


            // [ Scroll up ]

		    while( this.ContentAnchoredPosition - this._PrevAnchoredPosition  < -(this._ItemHeightSize + 0.5f*_ItemMargin) * 2 ) {

                this._PrevAnchoredPosition -= (this._ItemHeightSize + 0.5f*_ItemMargin);

                // move a first item to last
                for(int i=0; i<_ColumnCount; i++)
                {
                    var first = this._Containers.First;
                    if( first == null ) break;
                    var item = first.Value;
                    this._Containers.RemoveFirst();
                    this._Containers.AddLast(item);

                    // set item position
                    var column = (_Containers.Count + _NextInsertItemNo) % _ColumnCount;
                    var posX = (_ItemWidthSize + _ItemMargin) * column;
                    posX += leftMargin;
                    if (_HasGap && column >= _HalfColumnCount)
                        posX += _MiddleGapSize - _ItemMargin;
                    var posY = (this._ItemHeightSize + _ItemMargin) * (( this._Containers.Count + this._NextInsertItemNo ) / _ColumnCount);
                    posY += topMargin;
                    item.anchoredPosition = new Vector2(posX, -posY);

                    // update item

                    this.updateItem( this._Containers.Count+this._NextInsertItemNo, item.gameObject );

                    this._NextInsertItemNo++;
                }
                
		    }

            // [ Scroll down ]

            while ( this.ContentAnchoredPosition - this._PrevAnchoredPosition > 0 ) {

                this._PrevAnchoredPosition += (this._ItemHeightSize + 0.5f*_ItemMargin);

                // move a last item to first
                for(int i=0; i<_ColumnCount; i++)
                {
                    var last = this._Containers.Last;
                    if( last == null ) break;
                    var item = last.Value;
                    this._Containers.RemoveLast();
                    this._Containers.AddFirst(item);

                    this._NextInsertItemNo--;

                    // set item position
                    var column = _NextInsertItemNo % _ColumnCount;
                    var posX = (_ItemWidthSize + _ItemMargin) * ((_NextInsertItemNo ) % _ColumnCount);
                    posX += leftMargin;
                    if(_HasGap && column >= _HalfColumnCount)
                        posX += _MiddleGapSize - _ItemMargin;
                    var posY = (this._ItemHeightSize + _ItemMargin) * (( this._NextInsertItemNo ) / _ColumnCount);
                    posY += topMargin;
                    item.anchoredPosition = new Vector2(posX,-posY);

                    // update item

                    this.updateItem( this._NextInsertItemNo, item.gameObject );
                }
		    }
	    }

        private void resizeContent ()
        {
            var size = this._ContentRect.getSize();
            float rowCount = Mathf.Ceil((float)this.TotalItemCount / (float)_ColumnCount);
            size.y = this._ItemHeightSize * rowCount + Mathf.Max(0, rowCount - 1.0f) * _ItemMargin;
            this._ContentRect.setSize( size );
        }
	    private void updateItem ( int index, GameObject itemObj )
        {
            int maxIndex = TotalItemCount;
            if(HasMinimumItemCount)
                maxIndex = Mathf.Max(TotalItemCount, MinimumItemCount);
            
            if(index < 0 || index >= maxIndex)
                itemObj.SetActive(false);
            else
            {
                itemObj.SetActive(true);
            
                var item = itemObj.GetComponent<IDynamicScrollViewItem>();
                if( item != null ) item.onUpdateItem( index );
            }
	    }          
        
        protected virtual void clear() {

            while( this.transform.childCount>0 ) {
                DestroyImmediate( this.transform.GetChild( 0 ).gameObject );
            }
        }
    }
}

*/