namespace BubbleSort
{
    // Mark Kimmel and Dov Kimmel 
    // T00443600  T00505770
    public partial class Form1 : Form
    {
        private int[] numbers; // array for the lines
        private Graphics graphics;

        public Form1()
        {
            InitializeComponent();
            
            GenerateRandomArray();
            button1.BackColor = Color.Yellow;
            button1.FlatStyle = FlatStyle.Flat;
            button2.BackColor = Color.Yellow;
            button2.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Impact", 14F, FontStyle.Regular);
            button2.Font = new Font("Impact", 14f, FontStyle.Regular); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            button1.Enabled = false;

            BubbleSort();
          
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateRandomArray();
        }

        private void GenerateRandomArray() // random "bars" randomly represented - based on the height of panel 
        {
            Random rand = new Random();
            numbers = new int[panel1.Width / 12]; 

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = rand.Next(0, panel1.Height);
            }

            DrawBars();
        }
        private void BubbleSort()
        {
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = 0; j < numbers.Length - i - 1; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        
                        int temp = numbers[j];
                        numbers[j] = numbers[j + 1];
                        numbers[j + 1] = temp;

                        // in addtion to swapping we need to swap visisually which basically means redrawing two bars
                        DrawSingleBar(j);
                        DrawSingleBar(j + 1);

                        
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
        }

        private void DrawBars() // uses the array for the bars and creates them 
        {
            graphics = panel1.CreateGraphics();
            graphics.Clear(panel1.BackColor);

            for (int i = 0; i < numbers.Length; i++)
            {
                 // color, x coordinate (starting point for edge of bar in pixels), starting point bottom, thickness, height
                graphics.FillRectangle(Brushes.Pink, i * 12, panel1.Height - numbers[i], 10, numbers[i]);
            }
        }

        private void DrawSingleBar(int index)
        {
            graphics.FillRectangle(Brushes.DarkBlue, index * 12, 0, 10, panel1.Height);
            graphics.FillRectangle(Brushes.Pink, index * 12, panel1.Height - numbers[index], 10, numbers[index]);
        }
        
    }
}