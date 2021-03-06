﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace gsNotasNET.Droid
{
    [Activity(Label = "gsNotasNET", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            App.AppName = "gsNotas.Android";
            App.ClientSecretJson = Assets.Open("client_secret_50463492690.json");
            App.CredencialesSQL = Assets.Open("encrypted-string.txt");
            App.CredencialesGuille = Assets.Open("encrypted-string-guille.txt");
            App.CredencialesCorreosGuille = Assets.Open("encrypted-string-correos-elguille.txt");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}