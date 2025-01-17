using ImGuiNET;
using VfxEditor.Data;
using VfxEditor.FileManager;
using VfxEditor.NodeLibrary;
using VfxEditor.Select.VfxSelect;

namespace VfxEditor.AvfxFormat {
    public class AvfxManager : FileManagerWindow<AvfxDocument, WorkspaceMetaAvfx, AvfxFile> {
        public static VfxSelectDialog SourceSelect { get; private set; }
        public static VfxSelectDialog ReplaceSelect { get; private set; }
        public static AvfxNodeLibrary NodeLibrary { get; private set; }
        public static CopyManager Copy { get; private set; } = new();

        public static void Setup() {
            SourceSelect = new VfxSelectDialog( "File Select [LOADED]", Plugin.Configuration.RecentSelects, true, SetSourceGlobal );
            ReplaceSelect = new VfxSelectDialog( "File Select [REPLACED]", Plugin.Configuration.RecentSelects, false, SetReplaceGlobal );
            NodeLibrary = new( Plugin.Configuration.VFXNodeLibraryItems, Plugin.Configuration.WriteLocation );
        }

        public static void SetSourceGlobal( SelectResult result ) {
            Plugin.AvfxManager?.SetSource( result );
            Plugin.Configuration.AddRecent( Plugin.Configuration.RecentSelects, result );
        }

        public static void SetReplaceGlobal( SelectResult result ) {
            Plugin.AvfxManager?.SetReplace( result );
            Plugin.Configuration.AddRecent( Plugin.Configuration.RecentSelects, result );
        }

        public static readonly string PenumbraPath = "VFX";

        // =================

        public AvfxManager() : base( title: "VFXEditor", id: "Vfx", tempFilePrefix: "VfxTemp", extension: "avfx", penumbaPath: PenumbraPath ) { }

        protected override AvfxDocument GetNewDocument() => new( LocalPath );

        protected override AvfxDocument GetImportedDocument( string localPath, WorkspaceMetaAvfx data ) => new( LocalPath, localPath, data );

        protected override void DrawMenu() {
            if( CurrentFile == null ) return;

            if( ImGui.BeginMenu( "Edit##Menu" ) ) {
                CopyManager.Avfx.Draw();
                CommandManager.Avfx.Draw();

                if( ImGui.BeginMenu( "Templates##Menu" ) ) {
                    if( ImGui.MenuItem( "Blank##Menu" ) ) ActiveDocument?.OpenTemplate( @"default_vfx.avfx" );
                    if( ImGui.MenuItem( "Weapon##Menu" ) ) ActiveDocument?.OpenTemplate( @"default_weapon.avfx" );
                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }
        }

        public override void Dispose() {
            base.Dispose();
            SourceSelect.Hide();
            ReplaceSelect.Hide();
        }

        public override void DrawBody() {
            SourceSelect.Draw();
            ReplaceSelect.Draw();
            NodeLibrary.Draw();
            base.DrawBody();
        }

        public void Import( string path ) => ActiveDocument.Import( path );

        public void ShowExportDialog( AvfxNode node ) => ActiveDocument.ShowExportDialog( node );
    }
}
