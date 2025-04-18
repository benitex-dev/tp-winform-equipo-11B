﻿using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPWinForm_equipo_11
{
    public partial class VentanaPrincipal : Form
    {

        private List<Articulo> listaArticulos;
        public VentanaPrincipal()
        {
            InitializeComponent();
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FormAltaArticulo altaArticulo = new FormAltaArticulo();
            
            altaArticulo.ShowDialog();
            cargar();
        }

        

        private void VentanaPrincipal_Load(object sender, EventArgs e)
        {
            cargar();
            
            comboBoxCampo.Items.Add("Código");
            comboBoxCampo.Items.Add("Nombre");
            comboBoxCampo.Items.Add("Descripción");
            comboBoxCampo.Items.Add("Precio");
        }
        private void cargar()
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();


            try
            {
                listaArticulos = articuloNegocio.listar();
                //listamos los articulos en nuestro data grid view
                dgvCatalogo.DataSource = articuloNegocio.listar();
                ocultarColumnas();


            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void cargarImagen(string imagen)
        {
            try
            {
                pictureBoxImagen.Load(imagen);
            }
            catch (Exception ex)
            {
                pictureBoxImagen.Load("https://cdn-icons-png.flaticon.com/512/813/813728.png"); //se carga una imagen por defecto
            }
        }

        private void dgvCatalogo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCatalogo.CurrentRow != null)
            {

                Articulo seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;

                ImagenNegocio negocio = new ImagenNegocio();
                List<Imagen> imagenes = negocio.listarImagenesArticulo(seleccionado.Id);

                if (imagenes.Count > 0 && imagenes != null) cargarImagen(imagenes[0].URL); //selecciona la primera imagen de la lista por lo tanto es la imagen principal del articulo
                     
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;

            FormDetalle formDetalle = new FormDetalle(seleccionado);

            formDetalle.ShowDialog();
        }

        private void eliminar()
        {
            Articulo seleccionado;
            ArticuloNegocio articulo = new ArticuloNegocio();

            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad queres Eliminar?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvCatalogo.CurrentRow.DataBoundItem;

                    articulo.eliminarArticulo(seleccionado.Id);

                    cargar();
                }
            }
            catch (Exception ex)

            {

                MessageBox.Show(ex.ToString());
            }


        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();      

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = textBoxFiltro.Text;

            if(filtro != "")
            {
                listaFiltrada = listaArticulos.FindAll(x => x.CodArticulo.ToUpper().Contains(filtro.ToUpper())|| x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dgvCatalogo.DataSource = null;
            dgvCatalogo.DataSource = listaFiltrada;
            ocultarColumnas();
        }



        private void ocultarColumnas()
        {
            //escondemos la columna imagen ya que la url de la misma no es importante para el usuario
            dgvCatalogo.Columns["Imagen"].Visible = false;
            // escondemos la columna id ya que la misma solo es importante para el desarrollador
            dgvCatalogo.Columns["Id"].Visible = false;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            textBoxFiltro.Text = "";

        }

        private void comboBoxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {

            string opcion = comboBoxCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                comboBoxCriterio.Items.Clear();
                comboBoxCriterio.Items.Add("Mayor a");
                comboBoxCriterio.Items.Add("Menor a");
                comboBoxCriterio.Items.Add("Igual a");
            }
            else
            {
                comboBoxCriterio.Items.Clear();
                comboBoxCriterio.Items.Add("Comienza con");
                comboBoxCriterio.Items.Add("Termina con");
                comboBoxCriterio.Items.Add("Contiene");
            }
        }
    }
}
