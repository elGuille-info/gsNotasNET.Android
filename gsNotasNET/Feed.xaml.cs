﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace gsNotasNET
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Feed : ContentPage
    {
        public Feed(string titulo="")
        {
            InitializeComponent();
            if (titulo.Any())
            {
                Title = $"{titulo} - gsNotasNET.Android {App.AppVersion}";
            }
            else
                Title = $"gsNotasNET.Android {App.AppVersion}";
        }
    }
}