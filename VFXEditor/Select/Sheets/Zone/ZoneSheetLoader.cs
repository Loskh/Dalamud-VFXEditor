using Dalamud.Logging;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Linq;
using VFXEditor.Select.Rows;

namespace VFXEditor.Select.Sheets {
    public class ZoneSheetLoader : SheetLoader<XivZone, XivZoneSelected> {
        public override void OnLoad() {
            var sheet = Plugin.DataManager.GetExcelSheet<TerritoryType>().Where( x => !string.IsNullOrEmpty( x.Name ) );
            foreach( var item in sheet ) {
                Items.Add( new XivZone( item ) );
            }
        }

        public override bool SelectItem( XivZone item, out XivZoneSelected selectedItem ) {
            selectedItem = null;
            var lgbPath = item.GetLgbPath();
            var result = Plugin.DataManager.FileExists( lgbPath );
            if( result ) {
                try {
                    var file = Plugin.DataManager.GetFile<Lumina.Data.Files.LgbFile>( lgbPath );
                    selectedItem = new XivZoneSelected( file, item );
                }
                catch( Exception e ) {
                    PluginLog.Error( "Error reading LGB file " + lgbPath, e );
                    return false;
                }
            }
            return result;
        }
    }
}
