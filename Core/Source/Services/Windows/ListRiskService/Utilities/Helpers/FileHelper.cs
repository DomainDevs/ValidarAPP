using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Excel.Helpers
{
    class FileHelper
    {
        public static Stylesheet GetStylesExcel()
        {
            Fonts fonts = new Fonts(
                new Font(new FontSize() { Val = 10 }), //Index 0 - Default
                new Font(new FontSize() { Val = 11 }, new Bold(), new Color() { Rgb = HexBinaryValue.FromString("000000") })); //Index 1 - Header

            Fills fills = new Fills(
                 new Fill(new PatternFill() { PatternType = PatternValues.Solid }),  //Index 0 - default
                 new Fill(new PatternFill() { PatternType = PatternValues.Solid })); // Index 1 - default

            Borders borders = new Borders(
                new Border(), // index 0 - default
                new Border( // index 1 - black border
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder()));

            CellFormats cellFormats = new CellFormats(new CellFormat());// Index 0 - default
            CellFormat cellFormat = new CellFormat { FontId = 0, FillId = 0, BorderId = 0 };
            cellFormat.Append(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }); //Index 1 -body
            CellFormat cellFormat2 = new CellFormat { FontId = 1, FillId = 0, BorderId = 0 };
            cellFormat2.Append(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }); //Index 2 -header
            cellFormats.Append(cellFormat);
            cellFormats.Append(cellFormat2);

            return new Stylesheet(fonts, fills, borders, cellFormats);
        }

        public static Cell ConstructCell(string value, CellValues dataType, int row = 0, int col = 0, int style = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                CellReference = row != 0 ? GetCellReference(Convert.ToUInt32(row), Convert.ToUInt32(col)) : null,
                StyleIndex = Convert.ToUInt32(style)
            };
        }
        public static string GetCellReference(uint row, uint column)
        {
            return string.Format("{0}{1}", GetColumnName("", column), row);
        }
        public static string GetColumnName(string prefix, uint column)
        {
            if (column < 26)
            {
                return string.Format("{0}{1}", prefix, (char)(65 + column));
            }
            else
            {
                return GetColumnName(GetColumnName(prefix, (column - column % 26) / 26 - 1), column % 26);
            }
        }

        public static MergeCell MergeCell(int row1, int col1, int row2, int col2)
        {
            string cell1 = GetCellReference(Convert.ToUInt32(row1), Convert.ToUInt32(col1));
            string cell2 = GetCellReference(Convert.ToUInt32(row2), Convert.ToUInt32(col2));
            return new MergeCell()
            {
                Reference = new StringValue(cell1 + ":" + cell2)
            };
        }
    }
}
