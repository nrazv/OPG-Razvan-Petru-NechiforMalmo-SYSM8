using FITTRACK.Models;
using FITTRACK.Services.DataService;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FITTRACK.Views;

/// <summary>
/// Interaction logic for UserView.xaml
/// </summary>
public partial class UserView : UserControl
{
    public UserView()
    {
        InitializeComponent();
        this.Loaded += LoadData;
    }
    private void LoadData(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is UserViewModel viewModel)
        {
            viewModel.setView();
        }
    }
}
