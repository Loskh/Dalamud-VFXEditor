namespace VfxEditor.AvfxFormat {
    public class UiNodeSelectCommand<T> : ICommand where T : AvfxNode {
        private readonly UiNodeSelect<T> Item;
        private readonly T State;
        private readonly T PrevState;

        public UiNodeSelectCommand( UiNodeSelect<T> item, T state ) {
            Item = item;
            State = state;
            PrevState = item.Selected;
        }

        public void Execute() => Item.Select( State );

        public void Redo() => Item.Select( State );

        public void Undo() => Item.Select( PrevState );
    }
}
