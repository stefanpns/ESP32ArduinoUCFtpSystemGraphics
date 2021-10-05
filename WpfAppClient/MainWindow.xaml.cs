using coreapp;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int count = 0;
        ClientSender clientSender = new ClientSender();
        public MainWindow()
        {
            InitializeComponent();

            EditorPrinter.setup(EditorObj, StatusLabel);
        }

      

        async void OnEnterClicked(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
            {
                return;
            }

            TextBox entry = sender as TextBox;



            string command = entry.Text;
            
            EditorPrinter.PrintStatus("Sending");

            await clientSender.sendCommand(command, ESP32ipEntry.Text);
            EditorPrinter.scrollToEndResultBox();
            entry.Text = "";
            entry.Focus();
        }
    }
}
