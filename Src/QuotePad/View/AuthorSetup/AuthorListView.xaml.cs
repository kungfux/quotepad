using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuotePad.View
{
    /// <summary>
    /// Interaction logic for AuthorListView.xaml
    /// </summary>
    public partial class AuthorListView : UserControl
    {
        public AuthorListView()
        {
            InitializeComponent();
        }

        /*private void BringSelectionIntoView(object sender, SelectionChangedEventArgs e)
        {
            var selector = sender as Selector;
            if (selector is ListBox)
            {
                (selector as ListBox).ScrollIntoView(selector.SelectedItem);
            }
        }*/
    }
}
