using CFACADESTRUC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MANEJOINVENTARIO
{
    public class CEncabezadoGrid
    {
        public static void fEncabezadoGrid(CEstructura ep, DataGridView Grid, string sTitulo, ref Int32 nRenglon)
        {
            Grid.Columns.Clear();
            Grid.Rows.Clear();
            DataGridViewCellStyle cell = new DataGridViewCellStyle();
            cell.Alignment = DataGridViewContentAlignment.TopCenter;
            Grid.RowHeadersVisible = false;

            Grid.Columns.Add("No", "No");
            Grid.Columns["No"].Width = 30;
            Grid.Columns["No"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Grid.Columns["No"].ReadOnly = true;

            Grid.Columns.Add("CATEGORIA", "CATEGORIA");
            Grid.Columns["CATEGORIA"].Width = 100;
            Grid.Columns["CATEGORIA"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Grid.Columns["CATEGORIA"].ReadOnly = true;

            Grid.Columns.Add("FOLIO_CODIGO", "FOLIO_CODIGO");
            Grid.Columns["FOLIO_CODIGO"].Width = 100;
            Grid.Columns["FOLIO_CODIGO"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Grid.Columns["FOLIO_CODIGO"].ReadOnly = true;

            Grid.Columns.Add("DESCRIPCION_ARTICULO", "DESCRIPCION_ARTICULO");
            Grid.Columns["DESCRIPCION_ARTICULO"].Width = 200;
            Grid.Columns["DESCRIPCION_ARTICULO"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Grid.Columns["DESCRIPCION_ARTICULO"].ReadOnly = true;

            Grid.Columns.Add("CANTIDAD_CAPTURAR", "CANTIDAD_CAPTURAR");
            Grid.Columns["CANTIDAD_CAPTURAR"].Width = 100;
            Grid.Columns["CANTIDAD_CAPTURAR"].SortMode = DataGridViewColumnSortMode.NotSortable;

            Grid.Columns.Add("CANTIDAD_ACTUAL", "CANTIDAD_ACTUAL");
            Grid.Columns["CANTIDAD_ACTUAL"].Width = 100;
            Grid.Columns["CANTIDAD_ACTUAL"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Grid.Columns["CANTIDAD_ACTUAL"].ReadOnly = true;

            Grid.Columns.Add("STOCK", "STOCK");
            Grid.Columns["STOCK"].Width = 100;
            Grid.Columns["STOCK"].SortMode = DataGridViewColumnSortMode.NotSortable;
            Grid.Columns["STOCK"].ReadOnly = true;

            Grid.Rows.Add(1);

            Grid.Rows[nRenglon].Cells["No"].Value = "No.";
            Grid.Rows[nRenglon].Cells["CATEGORIA"].Value = "CATEGORIA";
            Grid.Rows[nRenglon].Cells["CATEGORIA"].Style = cell;
            Grid.Rows[nRenglon].Cells["FOLIO_CODIGO"].Value = "FOLIO CODIGO";
            Grid.Rows[nRenglon].Cells["FOLIO_CODIGO"].Style = cell;
            Grid.Rows[nRenglon].Cells["DESCRIPCION_ARTICULO"].Value = "DESCRIPCION ARTICULO";
            Grid.Rows[nRenglon].Cells["DESCRIPCION_ARTICULO"].Style = cell;
            Grid.Rows[nRenglon].Cells["CANTIDAD_CAPTURAR"].Value = "CANTIDAD CAPTURAR";
            Grid.Rows[nRenglon].Cells["CANTIDAD_CAPTURAR"].Style = cell;
            Grid.Rows[nRenglon].Cells["CANTIDAD_ACTUAL"].Value = "CANTIDAD ACTUAL";
            Grid.Rows[nRenglon].Cells["CANTIDAD_ACTUAL"].Style = cell;
            Grid.Rows[nRenglon].Cells["STOCK"].Value = "STOCK";
            Grid.Rows[nRenglon].Cells["STOCK"].Style = cell;

            if (ep.Opcion == 0)
            {
                Grid.Columns.Add("REQUISICION", "REQUISICION");
                Grid.Columns["REQUISICION"].Width = 100;
                Grid.Columns["REQUISICION"].SortMode = DataGridViewColumnSortMode.NotSortable;

                Grid.Rows[nRenglon].Cells["REQUISICION"].Value = "REQUISICION";
            }
            else if (ep.Opcion == 1)
            {
                Grid.Columns.Add("OBSERVACION", "OBSERVACION");
                Grid.Columns["OBSERVACION"].Width = 100;
                Grid.Columns["OBSERVACION"].SortMode = DataGridViewColumnSortMode.NotSortable;

                Grid.Rows[nRenglon].Cells["OBSERVACION"].Value = "OBSERVACION";
            }

            Grid.Rows[nRenglon].DefaultCellStyle.BackColor = SystemColors.ControlDarkDark;
            Grid.AllowUserToResizeColumns = true;
            Grid.AllowUserToOrderColumns = false;
            Grid.ColumnHeadersVisible = false;
            Grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            nRenglon++;

            fRenglonesNo(Grid);
        }

        public static void fRenglonesNo(DataGridView Grid)
        {
            Int32 nRenglon = 0;

            DataGridViewCellStyle cell = new DataGridViewCellStyle();
            cell.Alignment = DataGridViewContentAlignment.TopCenter;

            nRenglon += Grid.Rows.Count - 1;
            Grid.Rows[nRenglon].Cells["No"].Value = nRenglon.ToString();
            Grid.Rows[nRenglon].Cells["No"].Style = cell;
        }
    }
}
