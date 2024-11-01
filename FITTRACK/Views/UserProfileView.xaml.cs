﻿using FITTRACK.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FITTRACK.Views;

public partial class UserProfileView : UserControl
{
    public UserProfileView()
    {
        InitializeComponent();
        this.Loaded += LoadData;

    }
    private void LoadData(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is UserProfileViewModel viewModel)
        {
            viewModel.UpdateData();
        }
    }
}
