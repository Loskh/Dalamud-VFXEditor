namespace VfxEditor.AvfxFormat {
    public abstract class AvfxSelectableItem : AvfxItem, IUiSelectableItem {
        private int Idx;

        public AvfxSelectableItem( string avfxName ) : base( avfxName ) { }

        public int GetIdx() => Idx;
        public void SetIdx( int idx ) { Idx = idx; }
    }
}
