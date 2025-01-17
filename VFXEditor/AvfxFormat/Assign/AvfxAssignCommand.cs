using VfxEditor;

namespace VfxEditor.AvfxFormat {
    public class AvfxAssignCommand : ICommand {
        private readonly AvfxBase Item;
        private readonly bool State;
        private readonly bool PrevState;

        public AvfxAssignCommand( AvfxBase item, bool state ) {
            Item = item;
            State = state;
            PrevState = item.IsAssigned();
        }

        public void Execute() => Item.SetAssigned( State );

        public void Redo() => Item.SetAssigned( State );

        public void Undo() => Item.SetAssigned( PrevState );
    }
}
