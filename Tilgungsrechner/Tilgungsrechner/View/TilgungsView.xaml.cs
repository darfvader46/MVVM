using System;
using System.Windows;
using System.Windows.Controls;

namespace Tilgungsrechner.View
{
    /// <summary>
    /// Interaktionslogik für TilgungsView.xaml
    /// </summary>
    public partial class TilgungsView : UserControl
    {
        public TilgungsView()
        {
            InitializeComponent();
        }

        private void BtnInitialisieren_Click(object sender, RoutedEventArgs e)
        {
            txbxDarlehen.Text = String.Empty;
            txbxTilgungsrate.Text = String.Empty;
            txbxZinsfuss.Text = String.Empty;
        }
    }
}
