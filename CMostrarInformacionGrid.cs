using CFACADESTRUC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CFACADECONN;
using Npgsql;

namespace MANEJOINVENTARIO
{
    public class CMostrarInformacionGrid
    {
        public static void fMostrarInformacionGrid(CEstructura ep, DataGridView Grid, string sTitulo, Int32 nCategoria, Int32 nFolio, string sDescripcionArticulo, ref Int32 nRenglon)
        {
            string sMensaje, sError = "", sConsulta;

            if(fValidarArticulo(Grid, nCategoria, nFolio))
            {
                Grid.Rows.Add(1);

                DataGridViewCellStyle cell = new DataGridViewCellStyle();
                cell.Alignment = DataGridViewContentAlignment.TopCenter;

                Grid.Rows[nRenglon].Cells["No"].Value = nRenglon.ToString();
                Grid.Rows[nRenglon].Cells["No"].Style = cell;

                Grid.Rows[nRenglon].Cells["CATEGORIA"].Value = nCategoria.ToString();
                Grid.Rows[nRenglon].Cells["CATEGORIA"].Style = cell;

                Grid.Rows[nRenglon].Cells["FOLIO_CODIGO"].Value = nFolio.ToString("D4");
                Grid.Rows[nRenglon].Cells["FOLIO_CODIGO"].Style = cell;

                Grid.Rows[nRenglon].Cells["DESCRIPCION_ARTICULO"].Value = sDescripcionArticulo.ToString();
                Grid.Rows[nRenglon].Cells["DESCRIPCION_ARTICULO"].Style = cell;

                try
                {
                    NpgsqlConnection conn = new NpgsqlConnection();
                    if (CConeccion.conexionPostgre(ep, ref conn, ref sError))
                    {
                        sConsulta = String.Format("SELECT cantidad, stock FROM cmb_mostrar_informacion_articulos({0}::INTEGER, {1}::INTEGER)", nCategoria, nFolio);
                        NpgsqlCommand com = new NpgsqlCommand(sConsulta, conn);
                        NpgsqlDataReader reader;

                        reader = com.ExecuteReader();

                        if(reader.Read())
                        {
                            Grid.Rows[nRenglon].Cells["CANTIDAD_ACTUAL"].Value = Convert.ToInt32(reader["cantidad"].ToString().Trim());
                            Grid.Rows[nRenglon].Cells["CANTIDAD_ACTUAL"].Style = cell;

                            Grid.Rows[nRenglon].Cells["STOCK"].Value = Convert.ToInt32(reader["stock"].ToString().Trim());
                            Grid.Rows[nRenglon].Cells["STOCK"].Style = cell;
                        }
                    }

                    if( conn.State== System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Se presento un problema al mostar la información..." + ex.Message.ToString().Trim(), sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                nRenglon++;

                fRenglonesNo(Grid);
            }
            else
            {
                sMensaje = String.Format("El artículo [ {0}-{1} ] ya se encuentra en la información mostrada", nCategoria.ToString(), nFolio.ToString("D4"));
                MessageBox.Show(sMensaje, sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        public static bool fValidarArticulo(DataGridView Grid, Int32 nCategoria, Int32 nFolio)
        {
            string sCategoria, sFolio;
            bool valorRegresa = true;

            for (int i = 1; i <= Grid.Rows.Count - 1; i++)
            {
                if (Grid.Rows[i].Cells["CATEGORIA"].Value != null && 
                    Grid.Rows[i].Cells["FOLIO_CODIGO"].Value != null)
                {
                    sCategoria = Grid["CATEGORIA", i].Value.ToString();
                    sFolio = Grid["FOLIO_CODIGO", i].Value.ToString();

                    if (Convert.ToInt32(sCategoria.ToString()) == nCategoria && 
                        Convert.ToInt32(sFolio.ToString()) == nFolio)
                    {
                        valorRegresa = false;
                        break;
                    }
                }
            }

            return valorRegresa;
        }
    }
}
