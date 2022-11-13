using ImGuiNET;
using System;
using System.Collections.Generic;
using VfxEditor;

namespace VfxEditor.AvfxFormat2 {
    public class UiIntCombo : IUiBase {
        public readonly string Name;
        public readonly AvfxInt Literal;

        private readonly Dictionary<int, string> Mapping;
        private string DisplayText => Mapping.TryGetValue( Literal.GetValue(), out var displayText ) ? displayText : "[UNKNOWN]";

        public UiIntCombo( string name, AvfxInt literal, Dictionary<int, string> mapping ) {
            Name = name;
            Literal = literal;
            Mapping = mapping;
        }

        public void Draw( string id ) {
            // Unassigned
            if( AvfxBase.DrawAddButton( Literal, Name, id ) ) return;

            var value = Literal.GetValue();
            var spacing = ImGui.GetStyle().ItemSpacing.X;
            var comboWidth = ImGui.GetContentRegionAvail().X * 0.65f - 100 - spacing; // have to do this calculation now
            ImGui.SetNextItemWidth( 100 );
            if( ImGui.InputInt( $"{id}-MainInput", ref value ) ) CommandManager.Avfx.Add( new AvfxIntCommand( Literal, value ) );

            ImGui.SameLine( 100 + spacing );
            ImGui.SetNextItemWidth( comboWidth );

            var idx = 0;
            if( ImGui.BeginCombo( $"{Name}{id}", DisplayText ) ) {
                foreach( var entry in Mapping ) {
                    var isSelected = entry.Key == value;
                    if( ImGui.Selectable( $"{entry.Value}##{idx}", isSelected ) ) {
                        CommandManager.Avfx.Add( new AvfxIntCommand( Literal, entry.Key ) );
                    }

                    if( isSelected ) ImGui.SetItemDefaultFocus();
                    idx++;
                }
                ImGui.EndCombo();
            }

            AvfxBase.DrawRemoveContextMenu( Literal, Name, id );
        }
    }
}
