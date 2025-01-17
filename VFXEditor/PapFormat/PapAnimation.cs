using ImGuiFileDialog;
using ImGuiNET;
using System.IO;
using VfxEditor.Utils;
using VfxEditor.Interop;
using VfxEditor.TmbFormat;
using VfxEditor.Parsing;

namespace VfxEditor.PapFormat {
    public class PapAnimation {
        public short HavokIndex = 0;

        private readonly string HkxTempLocation;
        private readonly ParsedString Name = new( "Name", "cbbm_replace_this", 31 );
        private readonly ParsedShort Unk1 = new( "Unknown 1" );
        private readonly ParsedInt Unk2 = new( "Unknown 2" );
        public TmbFile Tmb;

        public PapAnimation( string hkxPath ) {
            HkxTempLocation = hkxPath;
        }

        public PapAnimation( BinaryReader reader, string hkxPath ) {
            HkxTempLocation = hkxPath;
            Name.Read( reader );
            reader.ReadBytes( 32 - Name.Value.Length - 1 ); // name padded to 32 bytes. also account for trailing null
            Unk1.Read( reader );
            HavokIndex = reader.ReadInt16();
            Unk2.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            Name.Write( writer );
            for( var i = 0; i < ( 32 - Name.Value.Length - 1 ); i++ ) {
                writer.Write( ( byte )0 );
            }
            Unk1.Write( writer );
            writer.Write( HavokIndex );
            Unk2.Write( writer );
        }

        public void ReadTmb( BinaryReader reader ) {
            Tmb = new TmbFile( reader, true );
        }

        public void ReadTmb( string path ) {
            Tmb = TmbFile.FromLocalFile( path, true );
        }

        public byte[] GetTmbBytes() => Tmb.ToBytes();

        public string GetName() => Name.Value;

        public void Draw( string parentId, int modelId, SkeletonType modelType ) {
            Name.Draw( parentId, CommandManager.Pap );
            Unk1.Draw( parentId, CommandManager.Pap );
            Unk2.Draw( parentId, CommandManager.Pap );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            ImGui.Text( $"This animation has Havok index: {HavokIndex}" );

            if( ImGui.Button( $"Replace Havok data{parentId}" ) ) {
                FileDialogManager.OpenFileDialog( "Select a File", ".hkx,.*", ( bool ok, string res ) => {
                    if( ok ) {
                        PapManager.IndexDialog.OnOk = ( int idx ) => {
                            CommandManager.Pap.Add( new PapHavokFileCommand( HkxTempLocation, () => {
                                HavokInterop.ReplaceHavokAnimation( HkxTempLocation, HavokIndex, res, idx, HkxTempLocation );
                            } ) );
                            UiUtils.OkNotification( "Havok data replaced" );
                        };
                        PapManager.IndexDialog.Show();
                    }
                } );
            }

            ImGui.BeginTabBar( "AnimationTabls" );
            DrawTmb( parentId );
            DrawAnimation3D( parentId, modelId, modelType );
            ImGui.EndTabBar();
        }

        private void DrawTmb( string parentId ) {
            if( !ImGui.BeginTabItem( "TMB" + parentId ) ) return;

            if( ImGui.Button( $"Replace TMB{parentId}" ) ) {
                FileDialogManager.OpenFileDialog( "Select a File", ".tmb,.*", ( bool ok, string res ) => {
                    if( ok ) {
                        CommandManager.Pap.Add( new PapReplaceTmbCommand( this, TmbFile.FromLocalFile( res, true ) ) );
                        UiUtils.OkNotification( "TMB data imported" );
                    }
                } );
            }

            ImGui.SameLine();
            if( ImGui.Button( $"Export TMB{parentId}" ) ) UiUtils.WriteBytesDialog( ".tmb", Tmb.ToBytes(), "tmb" );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            Tmb.Draw( parentId + "/Tmb" );

            ImGui.EndTabItem();
        }

        private void DrawAnimation3D( string parentId, int modelId, SkeletonType modelType ) {
            if( !ImGui.BeginTabItem( "3D View" + parentId ) ) return;
            Plugin.AnimationManager.Draw( this, HkxTempLocation, HavokIndex, modelId, modelType );
            ImGui.EndTabItem();
        }
    }
}
