
using System.Windows.Controls;

namespace WpfApp2
{
    class EditorPrinter
    {
        public static TextBox resultBox = null;
        public static Label statusLabel = null;


        public static void setup(TextBox resultBox, Label statusLabel)
        {
            EditorPrinter.resultBox = resultBox;
            EditorPrinter.statusLabel = statusLabel;
        }

        public static void PrintStatus(string status)
        {
            EditorPrinter.statusLabel.Content = "Status: " + status;
        }
        
        public static void WriteLine(string text)
        {
            if (EditorPrinter.resultBox != null)
            {
                EditorPrinter.resultBox.Text += text + "\r\n";
                
            }
        }

        public static void Write(string text)
        {
            if (EditorPrinter.resultBox != null)
            {
                EditorPrinter.resultBox.Text += text;
            }

        }

        public static void Write(char ch)
        {
            if (EditorPrinter.resultBox != null)
            {
                EditorPrinter.resultBox.Text += ch;

            }
        }

        public static void WriteLine()
        {
            if (EditorPrinter.resultBox != null)
            {
                EditorPrinter.resultBox.Text += "\r\n";

            }
        }

        public static void scrollToEndResultBox()
        {
            EditorPrinter.resultBox.Focus();
            EditorPrinter.resultBox.CaretIndex 
                = EditorPrinter.resultBox.Text.Length;
            EditorPrinter.resultBox.ScrollToEnd();
        }

    }
}
