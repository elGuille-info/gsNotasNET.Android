﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using gsNotasNET.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//using System.IO;
using gsNotasNET.Data;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using Xamarin.Essentials;

namespace gsNotasNET
{
    public partial class NotasActivas : ContentPage
    {
        public static NotasActivas Current;

        private List<NotaSQL> colNotas = null;

        public NotasActivas()
        {
            InitializeComponent();
            Current = this;
        }

        async private void ContentPage_Appearing(object sender, EventArgs e)
        {
            LabelStatus.Text = App.StatusInfo;

            if (UsuarioSQL.UsuarioLogin is null || UsuarioSQL.UsuarioLogin.ID == 0 || UsuarioSQL.UsuarioLogin.Email == "prueba")
            {
                await Navigation.PushAsync(new Login(Current));
                return;
            }
            // Solo las notas que no estén archivadas ni eliminadas
            if (App.UsarNotasLocal)
                colNotas = App.Database.Notas(App.Database.NotasUsuarioAsync(archivadas: false, eliminadas: false));
            else
                colNotas = NotaSQL.NotasUsuario(UsuarioSQL.UsuarioLogin.ID, archivadas: false, eliminadas: false);

            listView.ItemsSource = colNotas;
            TituloNotas();
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotaEditar
            {
                DatosMostrar = NotasDatosMostrar.Activas,
                BindingContext = new NotaSQL()
            });
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new NotaEditar
                {
                    DatosMostrar = NotasDatosMostrar.Activas,
                    BindingContext = e.SelectedItem as NotaSQL
                });
            }
        }

        async public static void TituloNotas()
        {
            string s = "";
            var total = 0;
            var nGrupos = 0; 
            if(App.UsarNotasLocal)
            {
                total = await App.Database.CountAsync();
                nGrupos = App.Database.Grupos().Count();
            }
            else
            {
                total = NotaSQL.Count(UsuarioSQL.UsuarioLogin.ID);
                nGrupos = NotaSQL.Grupos(UsuarioSQL.UsuarioLogin.ID).Count();
            }

            string sGrupo;

            //if (total == 0)
            //    s = "ninguna nota";
            //else if (total == 1)
            //    s = "1 nota";
            //else
            //    s = $"{total} notas";

            //if (nGrupos == 0)
            //    sGrupo = "sin grupos";
            //else if (nGrupos == 1)
            //    sGrupo = "en 1 grupo";
            //else
            //    sGrupo = $"en {nGrupos} grupos";

            //s = $"{UsuarioSQL.UsuarioLogin.Email} - con {s} {sGrupo}.";

            if (total == 0 && nGrupos == 0)
            {
                s = "sin notas ni grupos";
                sGrupo = "";
            }
            else
            {
                s = "con ";

                if (total == 0)
                    s = "sin notas";
                else if (total == 1)
                    s += "1 nota";
                else
                    s += $"{total} notas";

                if (nGrupos == 0)
                    sGrupo = " y sin grupos";
                else if (nGrupos == 1)
                    sGrupo = " en 1 grupo";
                else
                    sGrupo = $" en {nGrupos} grupos";
            }
            s = $"{UsuarioSQL.UsuarioLogin.Email} - {s}{sGrupo}.";

            //Current.Title = $"{App.AppName} {App.AppVersion}";
            Current.LabelInfo.Text = s;
        }

        #region  Para copiar las notas en Drive

        private async void CopiarEnDrive_Clicked(object sender, EventArgs e)
        {
            // Por ahora no usarlo
            await Navigation.PushAsync(new NotaEditar
            {
                BindingContext = new NotaSQL() { Texto = $"Por ahora no se sincroniza el contenido con GoogleDrive.", Grupo = "Drive-Docs" }
            }); ;
            return;

            //gsNotasNET.APIs.ApisDriveDocs.IniciadoGuardarNotasEnDrive += ApisDriveDocs_IniciadoGuardarNotasEnDrive;
            //gsNotasNET.APIs.ApisDriveDocs.FinalizadoGuardarNotasEnDrive += ApisDriveDocs_FinalizadoGuardarNotasEnDrive;
            //gsNotasNET.APIs.ApisDriveDocs.GuardandoNotas += ApisDriveDocs_GuardandoNotas;

            //var lasNotas = new Dictionary<string, List<string>>();
            //var grupos = NotaSQL.Grupos(UsuarioSQL.UsuarioLogin.ID);
            //foreach (var g in grupos)
            //{
            //    if (!lasNotas.ContainsKey(g))
            //    {
            //        lasNotas.Add(g, new List<string>());

            //        var colNotas = App.Database.GetNotesAsync(g);

            //        var col = new List<string>();

            //        foreach (var n in colNotas.Result)
            //            col.Add(n.Text);

            //        lasNotas[g] = col;
            //    }
            //}
            //try
            //{
            //    var t = gsNotasNET.APIs.ApisDriveDocs.GuardarNotasDrive("", "NO", lasNotas);

            //    LabelInfo.Text = $"Se han copiado {t} notas en los documentos de Drive.";
            //}
            //catch (Exception ex)
            //{
            //    var crlf = "\r\n";
            //    await Navigation.PushAsync(new NotaEditar
            //    {
            //        BindingContext = new NotaSQL() { Texto = $"Error:{crlf}{ex.Message}", Grupo = "Drive-Docs" }
            //    }); ;
            //    ApisDriveDocs_FinalizadoGuardarNotasEnDrive();
            //}

            //gsNotasNET.APIs.ApisDriveDocs.IniciadoGuardarNotasEnDrive -= ApisDriveDocs_IniciadoGuardarNotasEnDrive;
            //gsNotasNET.APIs.ApisDriveDocs.FinalizadoGuardarNotasEnDrive -= ApisDriveDocs_FinalizadoGuardarNotasEnDrive;
            //gsNotasNET.APIs.ApisDriveDocs.GuardandoNotas -= ApisDriveDocs_GuardandoNotas;
        }

        private void ApisDriveDocs_GuardandoNotas(string mensaje)
        {
            LabelInfo.Text = mensaje;
        }

        private void ApisDriveDocs_FinalizadoGuardarNotasEnDrive()
        {
            Current.LabelInfo.BackgroundColor = (Color)Application.Current.Resources["NavigationBarColor"];
            Current.LabelInfo.TextColor = (Color)Application.Current.Resources["NavigationBarTextColor"];
        }

        private void ApisDriveDocs_IniciadoGuardarNotasEnDrive()
        {
            Current.LabelInfo.BackgroundColor = Color.Firebrick;
            Current.LabelInfo.TextColor = Color.White;
        }

        #endregion

        //private void btnPrivacidad_Clicked(object sender, EventArgs e)
        //{
        //    _ = App.MostrarPoliticaPrivacidad();
        //}

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        //private void Button_Focused(object sender, FocusEventArgs e)
        //{
        //    if(e.IsFocused)
        //    {
        //        System.Diagnostics.Debug.WriteLine(e.IsFocused);
        //    }
        //}
    }
}