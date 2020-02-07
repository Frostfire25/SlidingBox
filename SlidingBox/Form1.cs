using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlidingBox
{
    public partial class SlidingBox : Form
    {
        //3x3 board
        private int[,] board;
        //Number of times the player has moved
        private int score_int;

        public SlidingBox()
        {
            InitializeComponent();
            //Sets up a new game
            newGame();
        }

        //This method updates the score label on each move
        private void updateScore()
        {
            score.Text = "" + score_int++;
        }

        private void changeWinVisibility()
        {
            if(!newGameButton.Visible)
            {
                newGameButton.Visible = false;
                wonLabel.Visible = false;
            } else
            {
                newGameButton.Visible = true;
                wonLabel.Visible = true;
            }
        }

        private void newGame() {
            //Hides the buttons
            changeWinVisibility();
            //Sets the players score to 0
            this.score_int = 0;
            //Sets the score to 0
            score.Text = ""+0;
            //Updates the players score
            updateScore();
            //Creates a new board untill it is solvable and not equal to the winning combination.
            do {
                this.board = generateBox();
            } while (!isSolvable(this.board) && check());
            //Sets up the box with whatever order was generated above.
            setUpBox(this.board);

        }

        //Sets up the boxes in corrispondence with arguement
        private void setUpBox(int[,] box)
        {
            //A list with each pictureBox
            List<PictureBox> boxes = new List<PictureBox>();
            boxes.Add(boxSpace00);
            boxes.Add(boxSpace10);
            boxes.Add(boxSpace20);
            boxes.Add(boxSpace01);
            boxes.Add(boxSpace11);
            boxes.Add(boxSpace21);
            boxes.Add(boxSpace02);
            boxes.Add(boxSpace12);
            boxes.Add(boxSpace22);

            //Sets each picture box to the box in the box
            int counter = 0;
            for(int i = 0; i < 3; i++)
            {
                for(int q = 0; q < 3; q++)
                {
                    boxes[counter++].Image = getImageFromNumber(box[i, q]);                     
                }
            }

        }
        
        //Returns the image from the number passed
        private Image getImageFromNumber(int number)
        {
            switch(number)
            {
                case 1:
                    {
                        return Properties.Resources.one;
                    } 
                case 2:
                    {
                        return Properties.Resources.two;
                    }
                    
                case 3:
                    {
                        return Properties.Resources.three;
                    }
                    
                case 4:
                    {
                        return Properties.Resources.four;
                    }
                    
                case 5:
                    {
                        return Properties.Resources.five;
                    }
                    
                case 6:
                    {
                        return Properties.Resources.six;
                    }
                    
                case 7:
                    {
                        return Properties.Resources.seven;
                    }
                    
                case 8:
                    {
                        return Properties.Resources.eight;
                    }
                    
                case 9:
                    {
                        //Creates a new black image with the same background as the other images
                        //Since I was able to find a collage of immages 1-8 online but no blank one for the empty space.
                        Bitmap bitmap = new Bitmap(1, 1);
                        bitmap.SetPixel(0, 0, Properties.Resources.one.GetPixel(0,0));                    
                        return new Bitmap(bitmap,Properties.Resources.one.Width, Properties.Resources.one.Height);
                    }
                    
            }
            //Returns null if a number != to 1-9 was passed
            return null;
        }

        //Generates a new 3x3 box.
        private int[,] generateBox()
        {
            Random rand = new Random();
            //2d array that's going to be returned
            int[,] board = new int[3,3];
            for(int i = 0; i < 3; i++)
            {
                for(int q = 0; q < 3; q++)
                {
                    //If the number is already in the array a new number is generated.
                    int num;
                    do
                    {
                        num = rand.Next(1, 10);
                    } while (isInBox(board, num));
                    board[i, q] = num;
                }
            }
            return board;
        }

        //Checks a 2d array and see's if a number is in the array
        private bool isInBox(int[,] test_box, int n)
        {
            for(int i = 0; i < 3; i++)
            {
                for(int q = 0; q < 3; q++)
                {
                    if (test_box[i, q] == n)
                        return true;
                }
            }
            return false;
        }

        //Solver algorithm
        private bool isSolvable(int[,] test_box)
        {
            //Total amount of inversions.
            int inversions = 0;
            //one-diminsional array where every space from the 2d array is added
            int[] onedarray = new int[9];
            //Space holder for 1-9
            int space = 0;
            //Adds a number from the test_box array to onedarry
            for (int i = 0; i < 3; i++)
            {
                for(int q = 0; q < 3; q++)
                {
                    onedarray[space++] = test_box[i, q];
                }
            }

            //Goes through each number in onedarry and if that number is greater than
            //the rest in that array holder++, than the total amounts of holders for
            //that number is added to the inversions
            for(int i = 0; i < 9; i++)
            {
                int holder = 0;
                for(int q = i+1; q < 9; q++)
                {
                 if (i == 8)
                       break;

                if (onedarray[i] > onedarray[q])
                            holder++;
                   
                }
                inversions += holder;
            }
            //If the inversions is even then the puzzle is solvable
            if (inversions % 2 == 0)
                return true;
            return false;
        }

        //Closes the form.
        private void SlidingBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); 
        }

        //On each move it checks to see if the empty space is next to the box, if it is than it moves to that space.
        private void move(int row, int colum)
        {
            try
            {
                //If the empty space is to the left
                if(this.board[row-1,colum] == 9)
                {
                    //Set's the empty space to the number
                    this.board[row - 1, colum] = this.board[row, colum];
                    //Set's the moved space to the empty space
                    this.board[row, colum] = 9;
                    //Set's up the box and changes the images
                    setUpBox(this.board);
                    //Checks for wins
                    checkForWin();
                    //Returns the method
                    return;
                } 
            } catch(IndexOutOfRangeException ee)
            {
            }

            try
            {
                //If the empty space is to the right
                if (this.board[row + 1, colum ] == 9)
                {
                    this.board[row + 1, colum] = this.board[row, colum];
                    this.board[row, colum] = 9;
                    setUpBox(this.board);
                    checkForWin();
                    return;
                }
            }
            catch (IndexOutOfRangeException ee)
            {
            }

            try
            {
                //If the empty space is below
                if (this.board[row, colum - 1] == 9)
                {
                    this.board[row, colum - 1] = this.board[row, colum];
                    this.board[row, colum] = 9;
                    setUpBox(this.board);
                    checkForWin();
                    return;
                }
            }
            catch (IndexOutOfRangeException ee)
            {
            }

            try
            {
                //If the empty space is above
                if (this.board[row, colum + 1] == 9)
                {
                    this.board[row, colum+1] = this.board[row, colum];
                    this.board[row, colum] = 9;
                    setUpBox(this.board);
                    checkForWin();
                    return;
                }
            }
            catch (IndexOutOfRangeException ee)
            {
            }
           
        }
        
        //Checks for a win
        private void checkForWin()
        {
            //Updates the score
            updateScore();
            if(check())
            {
                changeWinVisibility(); 
            }
            
        }

        //If the board is equal to the winning board than check() returns true

        // 1 2 3
        // 4 5 6
        // 7 8 9
        private bool check()
        {
            //Space holder
            int space = 1;

            int[] onedarray = new int[9];
            int s1 = 0;
            //Adds a number from the test_box array to onedarry
            for (int i = 0; i < 3; i++)
            {
                for (int q = 0; q < 3; q++)
                {
                    onedarray[s1++] = board[i, q];
                }
            }

            //Correct winning array
            int[] correct_array = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            //Goes through each space in onedarry, and if it is not equal to the correct space
            //in correct_array the mothod returns false
            for(int i = 0; i < 9; i++)
            {
                if (onedarray[i] != correct_array[i])
                    return false;
            }

            return true;
        }

        /*
         * Picture Box Listeners
         * 
         * In each method the move method is called for the space in the board array.
         * 
         */

        private void BoxSpace00_Click(object sender, EventArgs e)
        {
            move(0, 0);
        }

        private void BoxSpace10_Click(object sender, EventArgs e)
        {
            move(0,1);
        }

        private void BoxSpace20_Click(object sender, EventArgs e)
        {
            move(0,2);
        }

        private void BoxSpace01_Click(object sender, EventArgs e)
        {
            move(1,0);
        }

        private void BoxSpace11_Click(object sender, EventArgs e)
        {
            move(1, 1);
        }

        private void BoxSpace21_Click(object sender, EventArgs e)
        {
            move(1,2);
        }

        private void BoxSpace02_Click(object sender, EventArgs e)
        {
            move(2,0);
        }

        private void BoxSpace12_Click(object sender, EventArgs e)
        {
            move(2,1);
        }

        private void BoxSpace22_Click(object sender, EventArgs e)
        {
            move(2, 2);
        }

        //When the exit button is clicked, the application is closed

        private void Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
