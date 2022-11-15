using ImGuiNET;
using System;
using System.IO;
using VfxEditor.AvfxFormat2;

namespace VfxEditor.Parsing {
    public class ParsedBool : IParsedBase {
        public readonly string Name;
        private int Size;
        public bool? Value = false;

        public ParsedBool( string name, int size = 4 ) {
            Name = name;
            Size = size;
        }

        public void Read( BinaryReader reader, int size ) {
            var b = reader.ReadByte();
            Value = b switch {
                0x00 => false,
                0x01 => true,
                0xff => null,
                _ => null
            };
            Size = size;
        }

        public void Write( BinaryWriter writer ) {
            byte v = Value switch {
                true => 0x01,
                false => 0x00,
                null => 0xff
            };
            writer.Write( v );
            AvfxBase.WritePad( writer, Size - 1 );
        }

        public void Draw( string id, CommandManager manager ) {
            // Copy/Paste
            var copy = manager.Copy;
            if( copy.IsCopying ) copy.Bools[Name] = Value == true;
            if( copy.IsPasting && copy.Bools.TryGetValue( Name, out var val ) ) {
                copy.PasteCommand.Add( new ParsedBoolCommand( this, val ) );
            }

            var value = Value == true;
            if( ImGui.Checkbox( Name + id, ref value ) ) manager.Add( new ParsedBoolCommand( this, value ) );
        }
    }
}