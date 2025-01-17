using Dalamud.Logging;
using ImGuiNET;
using System;
using VfxEditor.Data;
using VfxEditor.Utils;
using VfxEditor.Penumbra;
using VfxEditor.TexTools;
using VfxEditor.TextureFormat;
using VfxEditor.Tracker;

namespace VfxEditor {
    public partial class Plugin {
        public static void Draw() {
            if( Loading ) return;

            CopyManager.ResetAll();
            CheckWorkspaceKeybinds();

            TexToolsDialog.Draw();
            PenumbraDialog.Draw();
            ToolsDialog.Draw();
            VfxTracker.Draw();
            Configuration.Draw();

            AvfxManager.Draw();
            TextureManager.Draw();
            TmbManager.Draw();
            PapManager.Draw();
            ScdManager.Draw();

            CopyManager.FinalizeAll();

            if ( Configuration.AutosaveEnabled && 
                 Configuration.AutosaveSeconds > 10 && 
                 !string.IsNullOrEmpty(CurrentWorkspaceLocation) && 
                 (DateTime.Now - LastAutoSave).TotalSeconds > Configuration.AutosaveSeconds
            ) {
                LastAutoSave = DateTime.Now;
                SaveWorkspace();
            }
        }

        private static void CheckWorkspaceKeybinds() {
            if( Configuration.OpenKeybind.KeyPressed() ) OpenWorkspace();
            if( Configuration.SaveKeybind.KeyPressed() ) SaveWorkspace();
            if( Configuration.SaveAsKeybind.KeyPressed() ) SaveAsWorkspace();
        }

        public static void DrawFileMenu() {
            if( ImGui.BeginMenu( "File##Menu" ) ) {
                ImGui.TextDisabled( "Workspace" );
                ImGui.SameLine();
                UiUtils.HelpMarker( "A workspace allows you to save multiple vfx replacements at the same time, as well as any imported textures or item renaming (such as particles or emitters)" );

                if( ImGui.MenuItem( "New##Menu" ) ) NewWorkspace();
                if( ImGui.MenuItem( "Open##Menu" ) ) OpenWorkspace();
                if( ImGui.MenuItem( "Save##Menu" ) ) SaveWorkspace();
                if( ImGui.MenuItem( "Save As##Menu" ) ) SaveAsWorkspace();

                ImGui.Separator();
                if( ImGui.MenuItem( "Settings##Menu" ) ) Configuration.Show();
                if( ImGui.MenuItem( "Tools##Menu" ) ) ToolsDialog.Show();
                if( ImGui.BeginMenu( "Help##Menu" ) ) {
                    if( ImGui.MenuItem( "Github##Menu" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor" );
                    if( ImGui.MenuItem( "Report an Issue##Menu" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/issues" );
                    if( ImGui.MenuItem( "Wiki##Menu" ) ) UiUtils.OpenUrl( "https://github.com/0ceal0t/Dalamud-VFXEditor/wiki" );
                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }

            if( ImGui.BeginMenu( "Export##Menu" ) ) {
                if( ImGui.MenuItem( "Penumbra##Menu" ) ) PenumbraDialog.Show();
                if( ImGui.MenuItem( "TexTools##Menu" ) ) TexToolsDialog.Show();
                ImGui.EndMenu();
            }
        }

        public static void DrawManagersMenu() {
            if( ImGui.MenuItem( "Textures##Menu" ) ) TextureManager.Show();
            ImGui.Separator();
            if( ImGui.MenuItem( "Vfx##Menu" ) ) AvfxManager.Show();
            if( ImGui.MenuItem( "Tmb##Menu" ) ) TmbManager.Show();
            if( ImGui.MenuItem( "Pap##Menu" ) ) PapManager.Show();
            if( ImGui.MenuItem( "Scd##Menu" ) ) ScdManager.Show();
        }
    }
}