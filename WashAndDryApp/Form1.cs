using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WashAndDryApp
{
    public partial class Form1 : Form
    {
        private enum State { Idle, Fill, Wash, Rinse, Spin, End }
        private State currentState = State.Idle;

        private Button btnStart;
        private Button btnTimer;
        private Label lblState;
        private TextBox textBox;

        private Label[] stateLabels = new Label[6];
        private List<Label> arrows = new List<Label>();

        public Form1()
        {
            InitializeComponent1();
            InitializeUI();
        }

        private void InitializeComponent1()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnTimer = new System.Windows.Forms.Button();
            this.lblState = new System.Windows.Forms.Label();
            this.textBox = new TextBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(50, 50);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 50);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnTimer
            // 
            this.btnTimer.Location = new System.Drawing.Point(200, 50);
            this.btnTimer.Name = "btnTimer";
            this.btnTimer.Size = new System.Drawing.Size(100, 50);
            this.btnTimer.Text = "Timer";
            this.btnTimer.Click += new System.EventHandler(this.btnTimer_Click);
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(50, 120);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(76, 13);
            this.lblState.Text = "Current State: ";
            // 
            // textBox
            // 
            this.textBox.Multiline = true;
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(500, 170);
            this.textBox.Location = new System.Drawing.Point(100, 250);
            this.textBox.Text = "States\n" +
                     "Idle (S0): The initial state when the machine is off or waiting to start.\n" + Environment.NewLine +
                     "Fill (S1): The state when the machine is filling with water.\n" + Environment.NewLine +
                     "Wash (S2): The state when the machine is washing clothes.\n" + Environment.NewLine +
                     "Rinse (S3): The state when the machine is rinsing clothes.\n" + Environment.NewLine +
                     "Spin (S4): The state when the machine is spinning to remove water.\n" + Environment.NewLine +
                     "End (S5): The final state indicating the wash cycle is complete.\n\n" + Environment.NewLine +
                     "Inputs\n" + Environment.NewLine +
                     "Start (I0): Signal to start the washing machine.\n" + Environment.NewLine +
                     "Timer (I1): Signal indicating the completion of a timed operation (e.g., fill, wash, rinse, spin).\n" + Environment.NewLine +
                     "Error (I2): Signal indicating an error (optional, for a more robust design).\n\n" + Environment.NewLine +
                     "Outputs\n" + Environment.NewLine +
                     "Cycle Complete (O0): Signal indicating the completion of the wash cycle.";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(700, 550);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.btnTimer);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.textBox);
            this.Name = "Form1";
            this.Text = "Washing Machine State Machine";
            this.ResumeLayout(false);
            this.PerformLayout();

            InitializeUI();
        }

        private void InitializeUI()
        {
            // Create labels for each state
            for (int i = 0; i < stateLabels.Length; i++)
            {
                stateLabels[i] = new Label();
                stateLabels[i].AutoSize = false;
                stateLabels[i].Size = new Size(80, 50);
                stateLabels[i].BorderStyle = BorderStyle.FixedSingle;
                stateLabels[i].TextAlign = ContentAlignment.MiddleCenter;
                stateLabels[i].Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
                stateLabels[i].Text = "S" + i;
                stateLabels[i].Location = new Point(50 + i * 100, 170);
                stateLabels[i].BackColor = Color.LightBlue; // Set light blue background
                Controls.Add(stateLabels[i]);
            }

            UpdateState();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (currentState == State.Idle)
            {
                currentState = State.Fill;
                UpdateState();
            }
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            switch (currentState)
            {
                case State.Fill:
                    currentState = State.Wash;
                    break;
                case State.Wash:
                    currentState = State.Rinse;
                    break;
                case State.Rinse:
                    currentState = State.Spin;
                    break;
                case State.Spin:
                    currentState = State.End;
                    break;
                case State.End:
                    MessageBox.Show("Cycle Complete!");
                    // Reset state to Idle
                    currentState = State.Idle;
                    // Clear arrows
                    ClearArrows();
                    // Update UI
                    UpdateState();
                    break;
            }
            UpdateState();
        }

        private void UpdateState()
        {
            lblState.Text = $"Current State: {currentState}";

            btnStart.Enabled = (currentState == State.Idle);
            btnTimer.Enabled = (currentState != State.Idle);

            // Draw arrows between labels to represent transitions
            if (currentState != State.Idle)
            {
                // Draw arrows between all state labels
                for (int i = 0; i < (int)currentState; i++)
                {
                    DrawArrow(stateLabels[i].Location, stateLabels[i + 1].Location);
                }
            }
        }

        private void DrawArrow(Point fromPoint, Point toPoint)
        {
            // Calculate the midpoint between fromPoint and toPoint
            int midX = (fromPoint.X + toPoint.X) / 2;
            int midY = (fromPoint.Y + toPoint.Y) / 2;

            // Create a new label for the arrow
            Label arrow = new Label();
            arrow.AutoSize = false;
            arrow.Size = new Size(20, 20); // Adjust arrow size
            arrow.Text = "→"; // Arrow symbol
            arrow.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            arrow.Location = new Point(midX + 30, midY + 17);
            Controls.Add(arrow);
            arrows.Add(arrow); // Add arrow label to the list
        }

        private void ClearArrows()
        {
            // Remove arrow labels from the form and clear the list
            foreach (var arrow in arrows)
            {
                Controls.Remove(arrow);
            }
            arrows.Clear();
        }
    }
}