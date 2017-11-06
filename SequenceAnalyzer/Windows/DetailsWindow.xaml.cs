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
using System.Windows.Shapes;

namespace SequenceAnalyzer.Windows
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private SequenceResult sequenceResult;

        private const string RF_ITEM_HEADER = "ReadsFrom";
        private const string FW_ITEM_HEADER = "FinalWrites";

        public DetailsWindow(SequenceResult sequenceResult)
        {
            this.sequenceResult = sequenceResult;

            InitializeComponent();
        }

        #region Window Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sequenceTitle = this.sequenceResult.Sequence.ToString();
            if (sequenceTitle.Length > 56)
                sequenceTitle = string.Format("{0} ...", sequenceTitle.Substring(0, 56));

            this.LabelSequence.Content = sequenceTitle;

            if (!this.sequenceResult.IsValid)
            {
                this.FaIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle;
                this.FaIcon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#f04747");
            }

            this.constructTreeView();
        }

        #endregion

        #region Private Methods

        private void constructTreeView()
        {
            this.TreeViewResults.Items.Clear();

            var mainSequenceItem = new TreeViewItem();
            mainSequenceItem.Focusable = false;
            mainSequenceItem.Header = string.Format("Base : {0}", this.sequenceResult.Sequence.ToString());

            var mainSequenceRfItem = new TreeViewItem();
            mainSequenceRfItem.Focusable = false;
            mainSequenceRfItem.Header = string.Format("{0} ({1})", RF_ITEM_HEADER, this.sequenceResult.Sequence.ReadsFrom.Count);
            foreach (var readFrom in this.sequenceResult.Sequence.ReadsFrom)
            {
                mainSequenceRfItem.Items.Add(new TreeViewItem() { Header = string.Format("{0}, {1}", readFrom.Key.ToString(), readFrom.Value.ToString()) });
            }
            mainSequenceItem.Items.Add(mainSequenceRfItem);

            var mainSequenceFwItem = new TreeViewItem();
            mainSequenceFwItem.Focusable = false;
            mainSequenceFwItem.Header = string.Format("{0} ({1})", FW_ITEM_HEADER, this.sequenceResult.Sequence.FinalWrites.Count); ;
            foreach (var finalWrite in this.sequenceResult.Sequence.FinalWrites)
            {
                mainSequenceFwItem.Items.Add(new TreeViewItem() { Header = string.Format("{0}: {1}", finalWrite.Key, finalWrite.Value.ToString()) });
            }
            mainSequenceItem.Items.Add(mainSequenceFwItem);

            this.TreeViewResults.Items.Add(mainSequenceItem);

            foreach (var permutation in this.sequenceResult.TestedPermutations)
            {
                var permutationItem = new TreeViewItem();
                permutationItem.Focusable = false;

                StackPanel permutationItemStackPanel = new StackPanel();
                permutationItemStackPanel.Orientation = Orientation.Horizontal;

                Label permuationItemLabel = new Label();
                permuationItemLabel.Content = string.Format("{0} : {1}", permutation.Key.Order, permutation.Key.ToString());

                permutationItemStackPanel.Children.Add(permutation.Value ? this.checkCircleIcon : this.exclamationCircleIcon);
                permutationItemStackPanel.Children.Add(permuationItemLabel);

                permutationItem.Header = permutationItemStackPanel;

                var permutationRfItem = new TreeViewItem();
                permutationRfItem.Focusable = false;
                permutationRfItem.Header = string.Format("{0} ({1})", RF_ITEM_HEADER, permutation.Key.ReadsFrom.Count);
                foreach (var readFrom in permutation.Key.ReadsFrom)
                {
                    permutationRfItem.Items.Add(new TreeViewItem() { Header = string.Format("{0}, {1}", readFrom.Key.ToString(), readFrom.Value.ToString()) });
                }
                permutationItem.Items.Add(permutationRfItem);

                var permutationFwItem = new TreeViewItem();
                permutationFwItem.Focusable = false;
                permutationFwItem.Header = string.Format("{0} ({1})", FW_ITEM_HEADER, permutation.Key.FinalWrites.Count);
                foreach (var finalWrite in permutation.Key.FinalWrites)
                {
                    permutationFwItem.Items.Add(new TreeViewItem() { Header = string.Format("{0}: {1}", finalWrite.Key, finalWrite.Value.ToString()) });
                }
                permutationItem.Items.Add(permutationFwItem);

                this.TreeViewResults.Items.Add(permutationItem);
            }
        }

        #endregion

        private FontAwesome.WPF.FontAwesome checkCircleIcon
        {
            get
            {
                FontAwesome.WPF.FontAwesome icon = new FontAwesome.WPF.FontAwesome();
                icon.Margin = new System.Windows.Thickness(0, 7, 0, 0);
                icon.FontSize = 15;
                icon.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckCircle;
                icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#43b581");
                return icon;
            }
        }

        private FontAwesome.WPF.FontAwesome exclamationCircleIcon
        {
            get
            {
                FontAwesome.WPF.FontAwesome icon = new FontAwesome.WPF.FontAwesome();
                icon.Margin = new System.Windows.Thickness(0, 7, 0, 0);
                icon.FontSize = 15;
                icon.Icon = FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle;
                icon.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#f04747");
                return icon;
            }
        }
    }
}
