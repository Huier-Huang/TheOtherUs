using System.IO;
using OfficeOpenXml;

namespace TheOtherUs.Languages;

public class ExcelLoader : LanguageLoaderBase
{

    public ExcelLoader()
    {
        Filter = [".excel", ".xls", ".xlsx"];
    }

    public override void Load(LanguageManager _manager, Stream stream, string FileName)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var excel = new ExcelPackage(stream);
        var worksheet = excel.Workbook.Worksheets[0];
        for (var c = worksheet.Columns.StartColumn + 1; c <= worksheet.Columns.EndColumn; c++)
        {
            var lang = worksheet.Cells[worksheet.Rows.StartRow, c].Text.PareNameToLangId();
            var HasWhite = 0;
            for (var r = worksheet.Rows.StartRow + 1; r < worksheet.Rows.EndRow; r++)
            {
                if (HasWhite > 5)
                    break;
                
                var key = worksheet.Cells[r, worksheet.Columns.StartColumn].Text;
                var value = worksheet.Cells[r, c].Text;
                if (key.IsNullOrWhiteSpace() || value.IsNullOrWhiteSpace())
                {
                    HasWhite++;
                    continue;
                }

                HasWhite = 0;
                _manager.AddToMap(lang, key, value, nameof(ExcelLoader));
            }
        }
    }
}