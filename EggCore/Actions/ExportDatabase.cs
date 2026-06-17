using EggCore.Utils;

namespace EggCore.Actions;

public class ExportDatabase(string id) : EggAction(id)
{
    public override void Execute()
    {
        DataExporter.ExportDatabases();
    }
}