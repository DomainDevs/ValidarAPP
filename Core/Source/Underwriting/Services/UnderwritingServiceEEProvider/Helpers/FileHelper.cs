// -----------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Helpers
{
    using System;
    using System.Collections;
    using System.Linq;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Sistran.Core.Framework.BAF;

    /// <summary>
    /// Helper para el archivo
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Genera un nombre Random de un archivo
        /// </summary>
        /// <param name="extension">extension del archivo (.doc .txt .xls)</param>
        /// <returns>configuración extension</returns>
        public static string GetRandomFileName(string extension)
        {
            if (!extension.Contains('.'))
            {
                extension = "." + extension;
            }

            return string.Format("{0}{1}", Guid.NewGuid(), extension);
        }

        /// <summary>
        /// Obtiene el valor de una propiedad del modelo correspondiente
        /// </summary>
        /// <param name="model">modelo a evaluar</param>
        /// <param name="prop">string de la porpiedad a evaluar
        /// <example>Policy.Product.Description</example>
        /// </param>
        /// <returns>valor de la propiedad</returns>
        public static object GetValue(dynamic model, string prop)
        {
            try
            {
                if (!prop.Contains(".") && !prop.Contains("["))
                {
                    return model.GetType().GetProperty(prop).GetValue(model, null);
                }
                else
                {
                    var array = prop.Split('.');
                    var strProp = string.Join(".", array.Skip(1).ToArray());

                    dynamic newModel = model.GetType().GetProperty(array[0]).GetValue(model, null);

                    if (newModel is IList)
                    {
                        var index = Convert.ToInt16(array[1]);
                        strProp = string.Join(".", array.Skip(2).ToArray());
                        newModel = newModel[index];
                        return GetValue(newModel, strProp);
                    }
                    else
                    {
                        return GetValue(newModel, strProp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en GetValue", ex);
            }
        }

        /// <summary>
        /// ELimina archivo
        /// </summary>
        /// <param name="path">Ruta del archivo</param>
        public static void DeleteFile(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en DeleteFile", ex);
            }
        }

        /// <summary>
        /// Copia un archivo entre dos rutas
        /// </summary>
        /// <param name="pathFrom">ruta completa del archivo a copiar</param>
        /// <param name="pathTo">ruta completa del destino</param>      
        public static void CopyFile(string pathFrom, string pathTo)
        {
            try
            {
                if (System.IO.File.Exists(pathFrom))
                {
                    System.IO.File.Copy(pathFrom, pathTo);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CopyFile", ex);
            }
        }

        /// <summary>
        ///  Copia un archivo entre dos rutas
        /// </summary>
        /// <param name="pathFrom">ruta completa del archivo a copiar</param>
        /// <param name="pathTo">ruta completa del destino</param>
        public static void MoveFile(string pathFrom, string pathTo)
        {
            try
            {
                if (System.IO.File.Exists(pathFrom))
                {
                    System.IO.File.Move(pathFrom, pathTo);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en MoveFile", ex);
            }
        }

        /// <summary>
        /// Genera una nueva celda con los parametros especificados
        /// </summary>
        /// <param name="value">valor de la celda</param>
        /// <param name="dataType">tipo de celda</param>
        /// <param name="row">index de la fila</param>
        /// <param name="col">index de la columna</param>
        /// <param name="style">index del estilo</param>
        /// <returns>configuración columnas</returns>
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

        /// <summary>
        /// Genera una celda combinada
        /// </summary>
        /// <param name="row1">index fila celda 1</param>
        /// <param name="col1">index columna celda 1</param>
        /// <param name="row2">index fila celda 2</param>
        /// <param name="col2">index columna celda 2</param>
        /// <returns>configuración columnas</returns>
        public static MergeCell MergeCell(int row1, int col1, int row2, int col2)
        {
            string cell1 = GetCellReference(Convert.ToUInt32(row1), Convert.ToUInt32(col1));
            string cell2 = GetCellReference(Convert.ToUInt32(row2), Convert.ToUInt32(col2));
            return new MergeCell()
            {
                Reference = new StringValue(cell1 + ":" + cell2)
            };
        }

        /// <summary>
        /// obtiene el la posicion exacta de la celda 
        /// </summary>
        /// <param name="prefix">configuracion prefix</param>
        /// <param name="column">configuracion columnas</param>
        /// <returns>Cadena formateada</returns>
        public static string GetColumnName(string prefix, uint column)
        {
            if (column < 26)
            {
                return string.Format("{0}{1}", prefix, (char)(65 + column));
            }
            else
            {
                string result = GetColumnName(prefix, (column - (column % 26)) / (26 - 1));
                return GetColumnName(result, column % 26);
            }
        }

        /// <summary>
        /// Obtiene la refencia de la celda  EJ: (E:12)
        /// </summary>
        /// <param name="row">index fila</param>
        /// <param name="column">index columna</param>
        /// <returns>Cadena de columna</returns>
        public static string GetCellReference(uint row, uint column)
        {
            return string.Format("{0}{1}", GetColumnName(string.Empty, column), row);
        }

        /// <summary>
        ///  Obtiene los estilos definidos para el archivo excel
        /// </summary>
        /// <returns>configuracion hojas</returns>
        public static Stylesheet GetStylesExcel()
        {
            Fonts fonts =
                new Fonts(
                new Font(
                    new FontSize()
                    {
                        Val = 10
                    }),
                new Font(
                    new FontSize()
                    {
                        Val = 11
                    },
                    new Bold(),
                    new Color()
                    {
                        Rgb = "000000"
                    }));

            Fills fills =
                new Fills(
                 new Fill(
                     new PatternFill()
                     {
                         PatternType = PatternValues.Solid
                     }),
                 new Fill(
                     new PatternFill()
                     {
                         PatternType = PatternValues.Solid
                     }));

            Borders borders = new Borders(
                new Border(),
                new Border(
                    new LeftBorder(
                        new Color()
                        {
                            Auto = true
                        })
                    {
                        Style = BorderStyleValues.Thin
                    },
                    new RightBorder(
                        new Color()
                        {
                            Auto = true
                        })
                    {
                        Style = BorderStyleValues.Thin
                    },
                    new TopBorder(
                        new Color()
                        {
                            Auto = true
                        })
                    {
                        Style = BorderStyleValues.Thin
                    },
                    new BottomBorder(
                        new Color()
                        {
                            Auto = true
                        })
                    {
                        Style = BorderStyleValues.Thin
                    },
                    new DiagonalBorder()));

            CellFormats cellFormats = new CellFormats(
                new CellFormat());
            CellFormat cellFormat = new CellFormat
            {
                FontId = 0,
                FillId = 0,
                BorderId = 0
            };
            cellFormat.Append(
                new Alignment()
                {
                    Horizontal = HorizontalAlignmentValues.Center,
                    Vertical = VerticalAlignmentValues.Center
                });
            CellFormat cellFormat2 = new CellFormat
            {
                FontId = 1,
                FillId = 0,
                BorderId = 0
            };
            cellFormat2.Append(
                new Alignment()
                {
                    Horizontal = HorizontalAlignmentValues.Center,
                    Vertical = VerticalAlignmentValues.Center
                });
            cellFormats.Append(cellFormat);
            cellFormats.Append(cellFormat2);

            return new Stylesheet(fonts, fills, borders, cellFormats);
        }
    }
}
