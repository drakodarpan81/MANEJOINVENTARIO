using CFACADECONN;
using CFACADESTRUC;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UCCOMBOBOX;
using GroupBox = System.Windows.Controls.GroupBox;

namespace MANEJOINVENTARIO
{
    public partial class frmManejoDeInventario : CControl
    {
        string sTitulo = "";
        CEstructura ep;
        Int32 nRenglon;

        public frmManejoDeInventario(CEstructura ed)
        {
            InitializeComponent();
            ep = ed;
        }

        private void frmManejoDeInventario_Load(object sender, EventArgs e)
        {
            HabilitarTeclaEscape = true;
            HabilitarTeclasSalir = true;

            switch (ep.Opcion)
            {
                case 0:
                    lblTitulo.Text = "INGRESO AL ALMACEN";
                    sTitulo = "INGRESO AL ALMACEN";
                    cmbArticulosAlmacen.Select();
                    break;
                case 1:
                    lblTitulo.Text = "SALIDA DEL ALMACEN";
                    sTitulo = "SALIDA DEL ALMACEN";
                    cmbArticulosAlmacen.Select();
                    break;
                default:
                    break;
            }

            AgregarControl(cmbArticulosAlmacen, null, "", false);
            AgregarControl(btnAgregar, null, "", false);
            AgregarControl(Grid, null, "", true);

            // Botones
            AgregarControl(btnLimpiar, null, "", false);
            AgregarControl(btnGuardar, null, "", false);
            AgregarControl(btnSalir, null, "", false);

            fInicializa();
        }

        public void fInicializa()
        {
            fLimpiarInformacion(gbGral);

            cmbArticulosAlmacen.DataSource = null;
            string sPresentacion = "SELECT descripcion, identificador FROM cmb_mostrararticulos()";
            cmbComboBox.fLlenarComboBoxPostgres(ep, sTitulo, sPresentacion, 3, cmbArticulosAlmacen, 1);

            nRenglon = 0;
            CEncabezadoGrid.fEncabezadoGrid(ep, Grid, sTitulo, ref nRenglon);
            cmbArticulosAlmacen.Select();
        }

        public static void fLimpiarInformacion(System.Windows.Forms.GroupBox gb)
        {
            foreach (System.Windows.Forms.Control c in gb.Controls)
            {
                if (c is System.Windows.Forms.TextBox || c is System.Windows.Forms.RichTextBox)
                {
                    c.Text = "";
                }
                else if (c is System.Windows.Forms.ComboBox)
                {
                    var tmp = c as System.Windows.Forms.ComboBox;
                    tmp.DataSource = null;
                    tmp.Items.Clear();
                }
                else if (c is DataGridView)
                {
                    var tmp = c as DataGridView;
                    tmp.Rows.Clear();
                    tmp.Columns.Clear();
                }
                else if (c is System.Windows.Forms.CheckBox)
                {
                    var tmp = c as System.Windows.Forms.CheckBox;
                    tmp.Checked = false;
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            fInicializa();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string sDato, sDato1, sDescripcionArticulo;
            char sDelimitador = '-', sDelimitador1 = '|';
            Int32 nCategoria, nFolio;

            sDato = cmbArticulosAlmacen.Text;
            string[] valores = sDato.Split(sDelimitador1);
            sDato1 = valores[0].ToString();
            sDescripcionArticulo = valores[1].ToString();
            string[] valor = sDato1.Split(sDelimitador);
            nCategoria = Convert.ToInt32(valor[0].ToString());
            nFolio = Convert.ToInt32(valor[1].ToString());

            CMostrarInformacionGrid.fMostrarInformacionGrid(ep, Grid, sTitulo, nCategoria, nFolio, sDescripcionArticulo, ref nRenglon);
        }

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Columns_KeyPress);
            if (Grid.CurrentCell.ColumnIndex == 4) //Columnas deseadas
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Columns_KeyPress);
                }
            }
        }

        private void Columns_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= Grid.Rows.Count - 1; i++)
            {
                if (Grid.Rows[i].Cells["CATEGORIA"].Value != null)
                {
                    if (fGuardarInformacion())
                    {
                        fInicializa();
                    }
                }
                else
                {
                    MessageBox.Show("No existe información para guardad. Verifique!!!", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        public bool fGuardarInformacion() 
        { 
            string sConsulta, sError = "", sObservacion="", sRequisicion="", sCantidad, sCategoria, sFolio, sCantidadAct;
            Int32 nFolio = 0, nCantidadAct;
            bool valorRegresa = false, bMensajeCantidad = true;
            List<string> lista = new List<string>();

            for (int i = 1; i <= Grid.Rows.Count - 2; i++)
            {
                if (Grid.Rows[i].Cells["CANTIDAD_CAPTURAR"].Value != null )
                {
                    sCantidad = Grid["CANTIDAD_CAPTURAR", i].Value.ToString();
                    sCantidadAct = Grid["CANTIDAD_ACTUAL", i].Value.ToString();

                    if (!String.IsNullOrEmpty(sCantidad))
                    {
                        if(ep.Opcion!=0)
                        {
                            nCantidadAct = Convert.ToInt32(sCantidadAct.ToString().Trim()) - Convert.ToInt32(sCantidad.ToString().Trim());

                            if (nCantidadAct < 0)
                            {
                                MessageBox.Show("Favor de revisar que la resta de las columnas [ CANTIDAD CAPTURAR - CANTIDAD ACTUAL ], no sea menor a cero...", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                bMensajeCantidad = false;
                                break;
                            }
                            else
                            {
                                valorRegresa = true;
                            }
                        }
                        else
                        {
                            valorRegresa = true;
                        }
                    }
                }
                else
                {
                    valorRegresa = false;
                    break;
                }
            }

            if (valorRegresa)
            {
                try
                {

                    for (int i = 1; i <= Grid.Rows.Count - 2; i++)
                    {
                        sCantidad = Grid["CANTIDAD_CAPTURAR", i].Value.ToString();
                        sCategoria = Grid["CATEGORIA", i].Value.ToString();
                        sFolio = Grid["FOLIO_CODIGO", i].Value.ToString();

                        switch (ep.Opcion)
                        {
                            case 0:
                                if (Grid.Rows[i].Cells["REQUISICION"].Value != null)
                                {
                                    sRequisicion = Grid["REQUISICION", i].Value.ToString();
                                }
                                break;
                            case 1:
                                if (Grid.Rows[i].Cells["OBSERVACION"].Value != null)
                                {
                                    sObservacion = Grid["OBSERVACION", i].Value.ToString();
                                }
                                break;
                            default:
                                break;
                        }

                        NpgsqlConnection conn = new NpgsqlConnection();
                        if (CConeccion.conexionPostgre(ep, ref conn, ref sError))
                        {
                            sConsulta = String.Format(" SELECT actualizar_articulos_almacen({0}::SMALLINT, {1}::INTEGER, {2}::INTEGER, {3}::INTEGER, '{4}', '{5}')",
                                                        ep.Opcion, Convert.ToInt32(sCategoria), Convert.ToInt32(sFolio), Convert.ToInt32(sCantidad), sRequisicion, sObservacion);

                            NpgsqlCommand com = new NpgsqlCommand(sConsulta, conn);
                            NpgsqlDataReader reader;

                            reader = com.ExecuteReader();

                            if (reader.Read())
                            {
                                nFolio = Convert.ToInt32(sFolio.ToString());
                                sConsulta = String.Format("Se creo el folio [ {0} ] para el articulo [ {1} - {2} ]", reader["actualizar_articulos_almacen"].ToString().Trim(), sCategoria.ToString().Trim(), nFolio.ToString("D4"));
                                lista.Add(sConsulta);
                            }
                        }

                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Se presento un problema al guardar la información..." + ex.Message.ToString(), sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    valorRegresa = false;
                }
            }
            else
            {
                if(bMensajeCantidad)
                {
                    MessageBox.Show("Favor de revisar que el campo [ CANTIDAD CAPTURAR ] este lleno...", sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    valorRegresa = false;
                }
            }

            if (valorRegresa)
            {
                foreach(var dato in lista)
                {
                    MessageBox.Show(dato.ToString(), sTitulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            return valorRegresa;
        }
    }
}
