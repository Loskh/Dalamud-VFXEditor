using ImGuiNET;
using System.Collections.Generic;
using System.IO;

namespace VFXEditor.TMB.Items {
    public class C198 : TMBItem {
        private int Duration = 0;
        private int Unk_3 = 0;
        private int Unk_4 = 0;
        private int Unk_5 = 0;
        private int Unk_6 = 0;
        private int Unk_7 = 0;
        private int Unk_8 = 0;

        public static readonly string DisplayName = "Lemure (C198)";
        public override string GetDisplayName() => DisplayName;
        public override string GetName() => "C198";

        public C198() { }
        public C198( BinaryReader reader ) {
            ReadInfo( reader );
            Duration = reader.ReadInt32();
            Unk_3 = reader.ReadInt32();
            Unk_4 = reader.ReadInt32();
            Unk_5 = reader.ReadInt32();
            Unk_6 = reader.ReadInt32();
            Unk_7 = reader.ReadInt32();
            Unk_8 = reader.ReadInt32();
        }

        public override int GetSize() => 0x28;
        public override int GetExtraSize() => 0;

        public override void Write( BinaryWriter entryWriter, int entryPos, BinaryWriter extraWriter, int extraPos, Dictionary<string, int> stringPositions, int stringPos ) {
            WriteInfo( entryWriter );
            entryWriter.Write( Duration );
            entryWriter.Write( Unk_3 );
            entryWriter.Write( Unk_4 );
            entryWriter.Write( Unk_5 );
            entryWriter.Write( Unk_6 );
            entryWriter.Write( Unk_7 );
            entryWriter.Write( Unk_8 );
        }

        public override void Draw( string id ) {
            DrawInfo( id );
            ImGui.InputInt( $"Duration{id}", ref Duration );
            ImGui.InputInt( $"Unknown 3{id}", ref Unk_3 );
            ImGui.InputInt( $"Unknown 4{id}", ref Unk_4 );
            ImGui.InputInt( $"Unknown 5{id}", ref Unk_5 );
            ImGui.InputInt( $"Unknown 6{id}", ref Unk_6 );
            ImGui.InputInt( $"Unknown 7{id}", ref Unk_7 );
            ImGui.InputInt( $"Unknown 8{id}", ref Unk_8 );
        }

        public override void PopulateStringList( List<string> stringList ) {
        }
    }
}