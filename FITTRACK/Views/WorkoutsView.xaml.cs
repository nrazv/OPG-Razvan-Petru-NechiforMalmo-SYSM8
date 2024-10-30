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

namespace FITTRACK.Views
{
    /// <summary>
    /// Interaction logic for WorkoutsView.xaml
    /// </summary>
    public partial class WorkoutsView : UserControl
    {
        public WorkoutsView()
        {
            InitializeComponent();
            this.Unloaded += ClearData;
            this.Loaded += LoadData;
        }

        private void ClearData(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is WorkoutsViewModel viewModel)
            {
                viewModel.AuthenticatedUser = null;
                viewModel.Workouts.Clear();
            }
        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is WorkoutsViewModel viewModel)
            {
                viewModel.UpdateData();

            }
        }
    }
}
