using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sudoku
{  
    class Sudoku : BoardGame 
    { 
        // How many numbers there is in each row or column 
        private const int amount = 9; 
        // Combobox choices 
        private string[] items = new String[] { " ", "1", "2", "3", "4", "5", "6", "7", "8", "9" }; 
        // Covering patterns where 1 stands for combobox and 0 for label  
        private int[] pattern = new int[]{  1,1,0,0,1,0,0,0,0,                                                 
                                            1,0,0,1,1,1,0,0,0,                                                
                                            0,1,1,0,0,0,0,1,0, 
                                            1,0,0,0,1,0,0,0,1, 
                                            1,0,0,1,0,1,0,0,1, 
                                            1,0,0,0,1,0,0,0,1, 
                                            0,1,0,0,0,0,1,1,0, 
                                            0,0,0,1,1,1,0,0,1, 
                                            0,0,0,0,1,0,0,1,1 }; 
        // To obtain a random line from the data.dat file 
        private Random random = new Random(); 
        // Contains the numbers  
        private string[] data = new string[amount * amount]; 
        // Name of the file that contains a sudoku in each row numbers separated with comma 
        private string filename = "data.dat"; 
        // How many numbers are left to be quessed 
        private int points; 
        // Gameboard 
        private SudokuBoard gameboard; 
 
        /// <summary> 
        /// parameterized constructor 
        /// </summary> 
        /// <param name="grid">reference to gameboard</param> 
        public Sudoku(SudokuBoard grid) 
        { 
            this.gameboard = grid;
            NewGame();
        } 
 
        /// <summary> 
        /// inherited new implementation 
        /// </summary> 
        new public void NewGame() 
        { 
            gameboard.grid.Children.Clear(); 
            points = 0; 
            ReadSudoku(random.Next(0, 4));
            Init();             
        } 
 
        /// <summary> 
        /// reads a random line from a file 
        /// if the random line is bigger than the amount of lines the reading starts from the beginning again 
        /// If there is a exception in reading from file a default sudoku is used instead 
        /// </summary> 
        /// <param name="line">random line to be read from file</param> 
        private void ReadSudoku(int line) 
        { 
            try
            {
                string dataFromFile = File.ReadLines(filename).Skip(line).Take(1).First();      // Read the line chosen by 'random'
                data = dataFromFile.Split(',');                                                 // Put each comma-separated string into array 'data'
            }
            catch (Exception)                                                                 // If read fails, use backup data
            {
                data = new string[] { "5", "3", "4", "6", "7", "8", "9", "1", "2",  
                                      "6", "7", "2", "1", "9", "5", "3", "4", "8", 
                                      "1", "9", "8", "3", "4", "2", "5", "6", "7",  
                                      "8", "5", "9", "7", "6", "1", "4", "2", "3",  
                                      "4", "2", "6", "8", "5", "3", "7", "9", "1",  
                                      "7", "1", "3", "9", "2", "4", "8", "5", "6",  
                                      "9", "6", "1", "5", "3", "7", "2", "8", "4",  
                                      "2", "8", "7", "4", "1", "9", "6", "3", "5",  
                                      "3", "4", "5", "2", "8", "6", "1", "7", "9" };
            }         
        } 
 
        /// <summary> 
        /// inherited overridden method 
        /// </summary> 
        /// <param name="sender">which element raised the event</param> 
        override public void Play(object sender) 
        { 
            ComboBox item = (ComboBox)sender; 
            int index = gameboard.grid.Children.IndexOf(item);                  // Get index of sender

            if (item.SelectedValue.Equals(data[index]) && item.SelectedValue != null)  // If correct number chosen
            {
                Label lb = new Label();                                         // Create label (which will soon replace the combobox)
                lb.Content = item.SelectedValue;

                lb.FontSize = 28;                                           // Set lb visual options
                lb.HorizontalContentAlignment = HorizontalAlignment.Center;
                lb.VerticalContentAlignment = VerticalAlignment.Center;
                // Set colours
                int row = index / 9;                                            // Get row number
                int col = index % 9;                                            // Get column number
                bool tealRow = (row == 3 || row == 4 || row == 5);          // Is it a 'teal row'?
                bool tealColumn = (col == 3 || col == 4 || col == 5);       // Is it a 'teal column'?

                if (tealRow ^ tealColumn)                                   // XOR (so middle isn't teal)
                    lb.Background = Brushes.Teal;
                else
                    lb.Background = Brushes.Peru;   

                gameboard.grid.Children.RemoveAt(index);                    // Remove the combobox
                gameboard.grid.Children.Insert(index, lb);                  // Add the label into the same place

                points++;
                if (points == amount * amount)                              // Check for winner (81 points), invite user to play again
                {
                    if (MessageBox.Show("You are a Sudoku master!\nDo you want to play again?",
                        "You win!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        NewGame();
                    }
                    else
                        Application.Current.Shutdown();
                }
            } 
        }

        /// <summary> 
        /// inherited and overriden method to create comboboxes and labels 
        /// and reset points 
        /// </summary> 
        public override void Init()
        {
            for (int i = 0; i < amount * amount; i++)                   // Add comboboxes and labels to grid
            {
                if (pattern[i] == 1)                                   // If pattern is 1, add combobox
                {
                    ComboBox cb = new ComboBox();
                    gameboard.grid.Children.Add(cb);

                    cb.FontSize = 28;                                           // Set cb visual options
                    cb.HorizontalContentAlignment = HorizontalAlignment.Center;
                    cb.VerticalContentAlignment = VerticalAlignment.Center;

                    foreach (string s in items)                                 // Add 1-9 options to each combobox
                        cb.Items.Add(s);

                    ((ComboBox)cb).SelectionChanged += new                      // Add event handler to combobox 
                        SelectionChangedEventHandler(gameboard.SelectionChanged);
                }
                else                                                  // If pattern is 0, add label
                {
                    points++;                                                   // If winner, points should be 81. This line ensures there's a point for each label.
                    Label lb = new Label();
                    lb.Content = data[i];                                       // Put correct number in label
                    gameboard.grid.Children.Add(lb);

                    lb.FontSize = 28;                                           // Set lb visual options
                    lb.HorizontalContentAlignment = HorizontalAlignment.Center;
                    lb.VerticalContentAlignment = VerticalAlignment.Center;
                    //Set colours
                    int row = i / 9;                                            // Get row number
                    int col = i % 9;                                            // Get column number
                    bool tealRow = (row == 3 || row == 4 || row == 5);          // Is it a 'teal row'?
                    bool tealColumn = (col == 3 || col == 4 || col == 5);       // Is it a 'teal column'?

                    if (tealRow ^ tealColumn)                                   // XOR (so middle isn't teal)
                        lb.Background = Brushes.Teal;
                    else
                        lb.Background = Brushes.Peru;                     
                }
            }
        } 
    } 
} 
