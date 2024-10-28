﻿using FITTRACK.Services.DataService;
using FITTRACK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FITTRACK.Views
{
    /// <summary>
    /// Interaction logic for AddWorkoutWindow.xaml
    /// </summary>
    public partial class AddWorkoutWindowView : Window
    {
        public AddWorkoutWindowView(IDataService dataService)
        {

            InitializeComponent();
            DataContext = new AddWorkoutWindowViewMode(dataService);
        }
    }
}
