﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gsNotasNET.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace gsNotasNET
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotasBuscar : ContentPage
	{
        private static NotasBuscar Current;

        public NotasBuscar ()
		{
			InitializeComponent ();
            Current = this;
            //Title = $"Buscar - {App.AppName} {App.AppVersion}";
        }

        async private void ContentPage_Appearing(object sender, EventArgs e)
        {
            LabelStatus.Text = App.StatusInfo;

            if (UsuarioSQL.UsuarioLogin is null || UsuarioSQL.UsuarioLogin.ID == 0 || UsuarioSQL.UsuarioLogin.Email == "prueba")
            {
                await Navigation.PushAsync(new Login(Current));
                return;
            }
            txtBuscar.Text = App.BuscarTexto;
            txtBuscarGrupo.Text = App.BuscarGrupo;
            chkArchivada.IsToggled = App.BuscarArchivadas;
            chkFavorita.IsToggled = App.BuscarFavoritas;
            chkNotificar.IsToggled = App.BuscarNotificar;
            //AsignarBúsqueda();
        }

        private void btnBuscar_Clicked(object sender, EventArgs e)
        {
            App.BuscarTexto = txtBuscar.Text;
            App.BuscarGrupo = txtBuscarGrupo.Text;
            App.BuscarArchivadas = chkArchivada.IsToggled;
            App.BuscarFavoritas = chkFavorita.IsToggled;
            App.BuscarNotificar = chkNotificar.IsToggled;

            AsignarBúsqueda();
        }

        private void AsignarBúsqueda()
        {
            // Solo si hay texto en buscar
            if (txtBuscar.Text.Any() || txtBuscarGrupo.Text.Any())
            {
                List<NotaSQL> notas;
                if (App.UsarNotasLocal)
                    notas = App.Database.BuscarNotas(txtBuscar.Text, txtBuscarGrupo.Text); 
                else
                    notas = NotaSQL.NotasBuscar(UsuarioSQL.UsuarioLogin.ID, txtBuscar.Text, txtBuscarGrupo.Text, 
                                                chkFavorita.IsToggled, chkArchivada.IsToggled, 
                                                chkEliminada.IsToggled, chkNotificar.IsToggled);

                listView.ItemsSource = notas;
                var plural = notas.Count() == 1 ? "" : "s";
                LabelInfo.Text = $"Hallada{plural} {notas.Count()} nota{plural}.";
            }
            else
            {
                listView.ItemsSource = null;
                LabelInfo.Text = "No se ha indicado nada que buscar.";
                txtBuscar.Focus();
            }
        }

        async private void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new NotaEditar
                {
                    BindingContext = e.SelectedItem as NotaSQL
                });
            }
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}