namespace VfxEditor.Select.Rows {
    public class XivCutscene {
        public string Name;
        public int RowId;
        public string Path;

        public XivCutscene( Lumina.Excel.GeneratedSheets.Cutscene cutscene ) {
            RowId = ( int )cutscene.RowId;
            var path = cutscene.Path.ToString();
            var splitPath = path.Split( '/' );
            Name = $"{splitPath[0]}/{splitPath[^1]}"; // ffxiv/anvwil/anvwil00500/anvwil00500 -> ffxiv/anvwil00500
            Path = $"cut/{path}.cutb";
        }
    }
}
