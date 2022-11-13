using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VfxEditor;

namespace VfxEditor.AvfxFormat2 {
    public class UiNodeSelectListRemoveCommand<T> : ICommand where T : AvfxNode {
        private readonly UiNodeSelectList<T> Item;
        private readonly int Idx;
        private readonly T State;

        public UiNodeSelectListRemoveCommand( UiNodeSelectList<T> item, int idx ) {
            Item = item;
            Idx = idx;
            State = item.Selected[idx];
        }

        public void Execute() {
            Item.UnlinkParentChild( State );
            Item.Selected.RemoveAt( Idx );
        }

        public void Redo() => Execute();

        public void Undo() {
            Item.Selected.Insert( Idx, State );
            Item.LinkParentChild( State );
        }
    }
}
