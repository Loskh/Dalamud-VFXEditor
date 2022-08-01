using ImGuiNET;
using VFXEditor.Select.Rows;

namespace VFXEditor.Select.TMB {
    public class TMBEmoteSelect : TMBSelectTab<XivEmoteTmb, XivEmoteTmb> {
        private ImGuiScene.TextureWrap Icon;

        public TMBEmoteSelect( string parentId, string tabId, TMBSelectDialog dialog ) :
            base( parentId, tabId, SheetManager.EmoteTmb, dialog ) {
        }

        protected override bool CheckMatch( XivEmoteTmb item, string searchInput ) {
            return Matches( item.Name, searchInput );
        }

        protected override void OnSelect() {
            LoadIcon( Selected.Icon, ref Icon );
        }

        protected override void DrawSelected( XivEmoteTmb loadedItem ) {
            if( loadedItem == null ) { return; }
            ImGui.Text( loadedItem.Name );
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            DrawIcon( Icon );

            DrawPath( "Tmb Path", loadedItem.TmbFiles, Id, Dialog, SelectResultType.GameEmote, "EMOTE", loadedItem.Name );
        }

        protected override string UniqueRowTitle( XivEmoteTmb item ) {
            return item.Name + "##" + item.RowId;
        }
    }
}
