using ScintillaNET;
using SequenceAnalyzer.Core;
using SequenceAnalyzer.Core.Sequences;
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

namespace SequenceAnalyzer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int maxLineNumberCharLength;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Window Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Configuring document styles...
            this.ScintillaComponent.StyleResetDefault();
            this.ScintillaComponent.Styles[ScintillaNET.Style.Default].Font = Constants.FONT_NAME;
            this.ScintillaComponent.Styles[ScintillaNET.Style.Default].Size = Constants.FONT_SIZE;
            this.ScintillaComponent.Margins[0].Width = Constants.DEFAULT_MARGIN_WIDTH;
        
            // Configure lexer styles...
            this.ScintillaComponent.Styles[ScintillaNET.Style.Asm.Default].ForeColor = System.Drawing.Color.Black;
            this.ScintillaComponent.Styles[ScintillaNET.Style.Asm.Comment].ForeColor = System.Drawing.Color.FromArgb(0, 128, 0);
            this.ScintillaComponent.Styles[ScintillaNET.Style.Asm.Identifier].ForeColor = System.Drawing.Color.Green;
            this.ScintillaComponent.Styles[ScintillaNET.Style.Asm.Operator].ForeColor = System.Drawing.Color.Red;
            this.ScintillaComponent.Lexer = Lexer.Asm;

            // Setting keywords...
            this.ScintillaComponent.SetKeywords(0, Constants.F_KEYWORDS);
            this.ScintillaComponent.SetKeywords(1, Constants.S_KEYWORDS);

            // Initializing core...
            AlgorithmsManager.Instance.RegisterAssembly();
        }

        #endregion

        #region Menu Events

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItemVerify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sequences = this.ScintillaComponent.Text.Split(new[] { '\r', '\n' });
                if (sequences.Length > 0)
                {
                    this.ListBoxResults.ItemsSource = null;

                    var results = AlgorithmsManager.Instance.ProcessEntry("ViewSerializability", sequences.Where(x => !string.IsNullOrEmpty(x)).ToList());
                    this.ListBoxResults.ItemsSource = results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.InnerException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Cancel);
            }
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }

        #endregion

        #region Scintilla Events

        private void ScintillaComponent_TextChanged(object sender, EventArgs e)
        {
            var maxLineNumberCharLength = this.ScintillaComponent.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            const int padding = 2;
            this.ScintillaComponent.Margins[0].Width = this.ScintillaComponent.TextWidth(ScintillaNET.Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;
        }

        private void ScintillaComponent_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Back)
                this.ListBoxResults.ItemsSource = null;
        }

        private void ScintillaComponent_CharAdded(object sender, CharAddedEventArgs e)
        {
            var currentPos = this.ScintillaComponent.CurrentPosition;
            var wordStartPos = this.ScintillaComponent.WordStartPosition(currentPos, true);

            List<string> readInstructionsList = new List<string>();
            var readInstructions = this.ScintillaComponent.Text.Split('r');
            foreach (var r in readInstructions)
            {
                int rThread = -1;
                if (!string.IsNullOrEmpty(r) && int.TryParse(r.Split('(')[0], out rThread) && !readInstructionsList.Contains('r' + rThread.ToString()))
                    readInstructionsList.Add('r' + rThread.ToString());
            }

            List<string> writeInstructionsList = new List<string>();
            var writeInstructions = this.ScintillaComponent.Text.Split('w');
            foreach (var w in writeInstructions)
            {
                int wThread = -1;
                if (!string.IsNullOrEmpty(w) && int.TryParse(w.Split('(')[0], out wThread) && !writeInstructionsList.Contains('w' + wThread.ToString()))
                    writeInstructionsList.Add('w' + wThread.ToString());
            }

            var lenEntered = currentPos - wordStartPos;
            if (lenEntered > 0)
            {
                if (!this.ScintillaComponent.AutoCActive)
                    this.ScintillaComponent.AutoCShow(lenEntered, string.Join(" ", readInstructionsList.Concat(writeInstructionsList).ToArray()));
            }
        }

        #endregion

        #region ListBox Events

        private void ListBoxResultsItem_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedSequenceResult = (SequenceResult)this.ListBoxResults.SelectedItem;

            DetailsWindow detailsWindow = new DetailsWindow(selectedSequenceResult);
            detailsWindow.Owner = this;
            detailsWindow.ShowDialog();
        }

        #endregion
    }
}
