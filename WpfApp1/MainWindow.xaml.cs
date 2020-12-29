using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        /// <summary>
        /// Holds curent result assigned to a cell in active game
        /// </summary>
        private MarkType[] mResults;

        /// <summary>
        /// True if it's player1's turn (x) or false when it's player2'sturn
        /// </summary>
        bool mPlayer1Turn;


        /// <summary>
        /// True if game has ended
        /// </summary>
        bool mGameEnded;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        #endregion

        #region New Game
        /// <summary>
        /// Starts a new game and clearas values from previous game
        /// </summary>
        private void NewGame()
        {
            //Create a new blank array of free cells
            mResults = new MarkType[9];

            for (var i = 0; i < mResults.Length; i++)
                mResults[i] = MarkType.Free;

            //Make sure players 1 is current player
            mPlayer1Turn = true;

            //Iterate every button on the grid
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                //Change content, backgroung and foreground to default
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;

                //Make sure the game hasn't ended
                mGameEnded = false;
            });

        }
        #endregion

        #region Button Click events handlers

        /// <summary>
        /// Handles button click event
        /// </summary>
        /// <param name="sender">The button that was clicked</param>
        /// <param name="e">The events of the click</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Start new game if the game was ended
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            //Cast the sender to a button
            var button = (Button)sender;

            //Finds the button position in the array
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var index = column + row * 3;

            //Dont do anything if there is already a value
            if (mResults[index] != MarkType.Free) 
                return;

            //Set the cell value based on whos turn it is
            mResults[index] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;

            //Set button text to the result
            button.Content = mPlayer1Turn ? "X" : "O";

            //Change O to red
            if (!mPlayer1Turn)
                button.Foreground = Brushes.Red;

            //Change players turn
            mPlayer1Turn ^= true;

            //Check for a winre
            CheckForWinner();
        }
        #endregion

        #region Check for winner
        /// <summary>
        /// Checks if there is a winner of three lines strainght
        /// </summary>
        private void CheckForWinner()
        {

            #region Check for horizontal wins

            //Check for horizontal wins
            //-Row 0
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[1] & mResults[2]) == mResults[0])
            {
                //End game
                mGameEnded = true;

                //Highlight winning cells
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
            }

            //-Row 1
            if (mResults[3] != MarkType.Free && (mResults[3] & mResults[4] & mResults[5]) == mResults[3]) 
            {
                //End game
                mGameEnded = true;

                //Highlight winning cells
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
            }

            //-Row 2
            if (mResults[6] != MarkType.Free && (mResults[6] & mResults[7] & mResults[8]) == mResults[6])
            {
                //End game
                mGameEnded = true;

                //Highlight winning cells
                Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
            }
            #endregion

            #region Check for vertical wins

            //-Column 0
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[3] & mResults[6]) == mResults[0])
            {
                //End game
                mGameEnded = true;

                //Highlight winning cells
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
            }

            //-Column 1
            if (mResults[1] != MarkType.Free && (mResults[1] & mResults[4] & mResults[7]) == mResults[1])
            {
                //End game
                mGameEnded = true;

                //Highlight winning cells
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
            }

            //-Column 2
            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[5] & mResults[8]) == mResults[2])
            {
                //End game
                mGameEnded = true;

                //Highlight winning cells
                Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
            }
            #endregion

            #region Check for diagonal wins

            //-Top Left Bottom Right
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[4] & mResults[8]) == mResults[0])
            {
                //End game
                mGameEnded = true;

                //Highlight winning cells
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
            }

            //-Top Right Bottom Left
            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[4] & mResults[6]) == mResults[2])
            {
                //End game
                mGameEnded = true;

                //Highlight winning cells
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;
            }  
            #endregion

            #region Check for full board and no winner
            //Check for full board and no winner
            if (!mResults.Any(result => result == MarkType.Free))
            {
                mGameEnded = true;
                // Turn all cells orange
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    //Change content, backgroung and foreground to default
                    button.Background = Brushes.Orange;


                });
            }
            #endregion
        }
        #endregion
    }
}
