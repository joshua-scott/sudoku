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

namespace sudoku
{
    /// <summary>
    /// Interaction logic for SudokuBoard.xaml
    /// </summary>
    public partial class SudokuBoard : UserControl
    {
        private Sudoku game;
        public SudokuBoard()
        {
            InitializeComponent();
            game = new Sudoku(this);                  // Create new instance of MiniMine called 'game'
        }

        // Event handler
        internal void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            game.Play(sender);
        }
    }
}
