using System;
using System.Windows.Forms;

namespace Calculator
{
    /* Simple calculator in C# to illustrate basic programming techniques */
    public partial class Calculator : Form
    {
        private string num1 = string.Empty;     // first input value
        private string num2 = string.Empty;     // second input value
        private string memory = string.Empty;   // internal memory 
        private char op = ' ';                  // tracks most recently pressed operator
        private bool clearScreen = false;       // whether to clear the screen before the next entry


        public Calculator()
        {
            InitializeComponent();
        }

        /*  Input a number
         *  Reads the number on the button text
         *  Push that number onto the input string
         *  Update the output box with the updated input string */
        private void num_Click(object sender, EventArgs e)
        {
            Button thisButton = (Button)sender;

            /* if we've marked to clear the screen before the next input, or the input bar only
             * contains "0", we want to replace it with whatever the new button was */
            if (clearScreen || boxOutput.Text == "0")
            {
                boxOutput.Text = thisButton.Text;
                clearScreen = false;
            }
            /* otherwise we want to append to the current output */
            else
            {
                boxOutput.Text += thisButton.Text;
            }
        }


        /* Add a decimal point
         * If the input is empty, make it "0."
         * Otherwise, if the input doesn't already contain a decimal point then add one on to the end
         * Then update the textbox */
        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            /* If we have empty input, we want it to become "0."*/
            if (boxOutput.Text == "")
            {
                boxOutput.Text = "0.";
            }
            /* Otherwise, we want to check that we don't already have a decimal point
             * if we don't then we want to append a decimal point */
            else if (!boxOutput.Text.Contains("."))
            {
                boxOutput.Text += ".";
            }
        }

        /* = */
        private void buttonEquals_Click(object sender, EventArgs e)
        {
            calculate();
        }

        /* Calculate the result
         * 1. Check that we have two values to calculate
         * 2. Find what operator has been pressed
         * 3. +  -> add the values 
         *    -  -> value 1 - value 2
         *    *  -> value 1 * value 2
         *    /  -> check for divide by zero: if safe, value 1/ value 2
         *  4. Display the result
         *  5. Clear the internal values we've dealt with */
        private void calculate()
        {
            float n1, n2, result = 0;
            num2 = boxOutput.Text;

            /* put the values from string into floats for calculation */
            if (float.TryParse(num1, out n1) && float.TryParse(num2, out n2))
            {
                switch (op)
                {
                    case '+':
                        result = n1 + n2;
                        goto default;

                    case '-':
                        result = n1 - n2;
                        goto default;

                    case '*':
                        result = n1 * n2;
                        goto default;

                    case '/':
                        /* check for divide by zero: on divide by zero, reset inputs and display error */
                        if (n2 == 0)
                        {
                            boxOutput.Text = "ERROR: Divide by zero";
                            clearInternalInput();
                            break;
                        }
                        else
                        {
                            result = n1 / n2;
                            goto default;
                        }

                    /* display the result and reset inputs */
                    default:
                        boxOutput.Text = result.ToString();

                        /* This resets it for the next calculation
                        * If there was a calculation with a result, that result will have been put on the display, so if the user
                        * tries to use it in a calculation it will be read back in at that point
                        * Clearing the input values like this means if the user just types another number it will replace what is
                        * on the screen instead of appending */
                        clearInternalInput();
                        break;
                }
            }
        }

        /* Clear all input values and operator; set screen to get cleared on new number input */
        private void clearInternalInput()
        {
            num1 = string.Empty;
            num2 = string.Empty;
            op = ' ';
            clearScreen = true;
        }

        /* Deal with an operator being pressed */
        private void operatorClick(char o)
        {
            /* If there's nothing input, do nothing */
            if (boxOutput.Text == "")
                return;

            /* If we already read in a value we need to calculate what we already have */
            if (num1 != string.Empty)
            {
                num2 = boxOutput.Text;
                calculate();
            }

            /* Now we either have the result of the calculation or the last input number on screen
             * so we store this as our first input value, and then note what operator was pressed */
            num1 = boxOutput.Text;
            clearScreen = true;
            op = o;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            operatorClick('+');
        }

        private void Divide_Click(object sender, EventArgs e)
        {
            operatorClick('/');
        }

        private void buttonMultiply_Click(object sender, EventArgs e)
        {
            operatorClick('*');
        }

        /* Minus button
         * If there is already an entry, treat it as a subtraction
         * If there isn't, start a negative number */
        private void buttonMinus_Click(object sender, EventArgs e)
        {
            if (boxOutput.Text != "")
            {
                operatorClick('-');
            }
            else
            {
                boxOutput.Text = "-";
                clearScreen = false;
            }
        }

        /* C - Clear screen and all buttons pressed so far */
        private void buttonClear_Click(object sender, EventArgs e)
        {
            boxOutput.Clear();
            clearInternalInput();
        }

        /* CE - clear screen*/
        private void buttonClearEntry_Click(object sender, EventArgs e)
        {
            boxOutput.Clear();
        }

        /* MC - clear memory */
        private void buttonMC_Click(object sender, EventArgs e)
        {
            memory = string.Empty;
        }

        /* MR - restore memory */
        private void buttonMR_Click(object sender, EventArgs e)
        {
            if (memory != String.Empty)
            {
                boxOutput.Text = memory;
            }
        }

        /* MS - Save the current input line to memory */
        private void buttonMS_Click(object sender, EventArgs e)
        {
            memory = boxOutput.Text;
        }
    }
}
