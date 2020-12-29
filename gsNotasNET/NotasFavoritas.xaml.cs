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
    public partial class NotasFavoritas : ContentPage
    {
        private static NotasFavoritas Current;

        private List<NotaSQL> colNotas = null;

        public NotasFavoritas()
        {
            InitializeComponent();
            Current = this;
            Title = $"{App.AppName} {App.AppVersion}";
        }

        async private void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (UsuarioSQL.UsuarioLogin is null || UsuarioSQL.UsuarioLogin.ID == 0 || UsuarioSQL.UsuarioLogin.Email == "prueba")
            {
                await Navigation.PushAsync(new Login(Current));
                return;
            }
            // Solo las notas favoritas
            if (App.UsarNotasLocal)
                colNotas = App.Database.NotasFavoritas();
            else
                colNotas = NotaSQL.NotasFavoritas(UsuarioSQL.UsuarioLogin.ID);
            listView.ItemsSource = colNotas;

            var plural = colNotas.Count() == 1 ? "" : "s";
            LabelInfo.Text = $"{UsuarioSQL.UsuarioLogin.Email} con {colNotas.Count()} nota{plural} favorita{plural}."; ;

        }

        private void btnPrivacidad_Clicked(object sender, EventArgs e)
        {
            _ = App.MostrarPoliticaPrivacidad();
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