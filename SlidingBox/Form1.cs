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

        private int[,] board;

        public SlidingBox()
        {
            InitializeComponent();
            newGame();
        }

        private void newGame() {
            do {
                this.board = generateBox();
            } while (!isSolvable(this.board));

            setUpBox(this.board);

        }

        private void setUpBox(int[,] box)
        {
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

            int counter = 0;
            for(int i = 0; i < 3; i++)
            {
                for(int q = 0; q < 3; q++)
                {
                    boxes[counter++].Image = getImageFromNumber(box[i, q]);                     
                }
            }

        }

        
        private int getNumberFromImage(Image image)
        {
            /*
            switch(image)
            {
                case Properties.Resources.one; return 1;
            }
            */
            return 9;
        }

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
            return null;
        }

        private void showBox(int[,] test_box)
        {
            String op = "";
            for (int i = 0; i < 3; i++)
            {
                for (int q = 0; q < 3; q++)
                {
                    op += "" + test_box[i, q] + " ";
                }
                op += "\n";
            }
            MessageBox.Show(op);
        }

        private int[,] generateBox()
        {
            Random rand = new Random();
            int[,] board = new int[3,3];
            for(int i = 0; i < 3; i++)
            {
                for(int q = 0; q < 3; q++)
                {
                    int num;
                    do
                    {
                        num = rand.Next(1, 10);
                    } while (isInBox(board, num));//Might be in a constant loop
                    board[i, q] = num;
                }
            }
            return board;
        }

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

        private bool isSolvable(int[,] test_box)
        {
            int inversions = 0;

            int[] onedarray = new int[9];
            for (int i = 0; i < 3; i++)
            {
                for(int q = 0; q < 3; q++)
                {
                    onedarray[i] = test_box[i, q];
                }
            }

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

            if (inversions % 2 == 0)
                return true;
            return false;
        }

        private void SlidingBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void setUpSideBar()
        {

        }

        private void move(int row, int colum)
        {
            int num = this.board[row, colum];
            try
            {
                if(this.board[row-1,colum] == 9)
                {
                    this.board[row - 1, colum] = this.board[row, colum];
                    this.board[row, colum] = 9;
                    setUpBox(this.board);
                    checkForWin();
                    return;
                } 
            } catch(IndexOutOfRangeException ee)
            {
            }

            try
            {
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
        
        private void checkForWin()
        {/*
            if(check())
            {
                MessageBox.Show("Won");
            }
            */
        }

        private bool check()
        {
            int space = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int q = 0; q < 3; q++)
                {
                    if (this.board[i, q] != space++)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

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
    }
}
