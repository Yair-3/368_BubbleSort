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

            comboBox1.SelectedIndexChanged += new EventHandler(comboBox_change);

            panel1.BackColor = Color.DarkBlue;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false; 

            
            Task.Run(() =>
            {
                StrategySort sortStrategy = null;
                string selectedAlgorithm = string.Empty;

                
                this.Invoke(new Action(() =>
                {
                    selectedAlgorithm = comboBox1.SelectedItem.ToString();
                }));

                switch (selectedAlgorithm)
                {
                    case "Bubble Sort":
                        sortStrategy = new BubbleSortStrategy();
                        break;
                    case "Insertion Sort":
                        sortStrategy = new InsertionSortStrategy();
                        break;
                    case "QuickSort":
                        sortStrategy = new QuickSortStrategy();
                        break;
                    case "Radix Sort":
                        sortStrategy = new RadixSortStrategy();
                        break;
                    case "Counting Sort":
                        sortStrategy = new CountingSortStrategy();
                        break;

                      
                }

                if (sortStrategy != null)
                {
                    sortStrategy.Sort(numbers, index =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            DrawSingleBar(index); 
                        }));
                    });
                }

                
                this.Invoke(new Action(() =>
                {
                    button1.Enabled = true;
                }));
            });
        }


        /*
        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;

            BubbleSort();

            button1.Enabled = true;
        }
        */



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
        private void comboBox_change(object sender, EventArgs e)
        {
            GenerateRandomArray();
   
        }

        private void DrawSingleBar(int index)
        {
            int barWidth = 10;
            int gap = 2; // Space between bars
            int x = index * (barWidth + gap);
            int barHeight = numbers[index];

            // Clear the previous bar (optional, if needed for clean-up)
            graphics.FillRectangle(new SolidBrush(panel1.BackColor), x, 0, barWidth, panel1.Height);


            
            graphics.FillRectangle(Brushes.Pink, x, panel1.Height - barHeight, barWidth, barHeight);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public interface StrategySort {
        void Sort(int[] array, Action<int> drawSingleBar); 
    }

    public class BubbleSortStrategy : StrategySort
    {
        public void Sort(int[] array, Action<int> drawSingleBar)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;

                        drawSingleBar(j);
                        drawSingleBar(j + 1);

                        System.Threading.Thread.Sleep(1);
                    }
                }
            }
        }
    }

    public class InsertionSortStrategy : StrategySort
    {
        public void Sort(int[] array, Action<int> drawSingleBar)
        {
            for (int i = 1; i < array.Length; ++i)
            {
                int key = array[i];
                int j = i - 1;

                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    drawSingleBar(j + 1);
                    j = j - 1;
                
                }
                System.Threading.Thread.Sleep(100);

                array[j + 1] = key;
                drawSingleBar(j + 1);

                System.Threading.Thread.Sleep(100);
            }
        }
    }
    public class QuickSortStrategy : StrategySort
    {
        public void Sort(int[] array, Action<int> drawSingleBar)
        {
            QuickSort(array, 0, array.Length - 1, drawSingleBar);
        }

        private void QuickSort(int[] array, int low, int high, Action<int> drawSingleBar)
        {
            if (low < high)
            {
                int pivotIndex = Partition(array, low, high, drawSingleBar);
                QuickSort(array, low, pivotIndex - 1, drawSingleBar);
                QuickSort(array, pivotIndex + 1, high, drawSingleBar);
            }
        }

        private int Partition(int[] array, int low, int high, Action<int> drawSingleBar)
        {
            int pivot = array[high];
            int i = (low - 1);

            for (int j = low; j < high; j++)
            {
                if (array[j] < pivot)
                {
                  
                    i++;
                    Swap(ref array[i], ref array[j]);
                    drawSingleBar(i);
                    drawSingleBar(j);
                    System.Threading.Thread.Sleep(40);
                }
            }
            Swap(ref array[i + 1], ref array[high]);
            drawSingleBar(i + 1);
            drawSingleBar(high);
            System.Threading.Thread.Sleep(40);
            return i + 1;
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }

    public class RadixSortStrategy : StrategySort
    {
        public void Sort(int[] arr, Action<int> drawSingleBar)
        {
            int maxNumber = FindMax(arr);

            
            for (int place = 1; maxNumber / place > 0; place *= 10)
            {
                CountingSortByDigit(arr, place, drawSingleBar);
            }
        }

        private int FindMax(int[] arr)
        {
            int max = arr[0];
            foreach (int num in arr)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            return max;
        }

        private void CountingSortByDigit(int[] arr, int place, Action<int> drawSingleBar)
        {
            int n = arr.Length;
            int[] output = new int[n];
            int[] count = new int[10];

            
            for (int i = 0; i < n; i++)
            {
                count[(arr[i] / place) % 10]++;
            }

            
            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            
            for (int i = n - 1; i >= 0; i--)
            {
                output[count[(arr[i] / place) % 10] - 1] = arr[i];
                count[(arr[i] / place) % 10]--;
            }

            
            for (int i = 0; i < n; i++)
            {
                arr[i] = output[i];
                drawSingleBar(i);
                System.Threading.Thread.Sleep(100); 
            }
        }
    }

    public class CountingSortStrategy : StrategySort
    {
        public void Sort(int[] arr, Action<int> drawSingleBar)
        {
            int max = FindMax(arr);
            CountingSort(arr, max, drawSingleBar);
        }

        private int FindMax(int[] arr)
        {
            return arr.Max();
        }

        private void CountingSort(int[] arr, int max, Action<int> drawSingleBar)
        {
            int n = arr.Length;
            int[] output = new int[n];
            int[] count = new int[max + 1];

           
            for (int i = 0; i < n; i++)
            {
                count[arr[i]]++;
            }

            
            for (int i = 1; i <= max; i++)
            {
                count[i] += count[i - 1];
            }

            
            for (int i = n - 1; i >= 0; i--)
            {
                output[count[arr[i]] - 1] = arr[i];
                count[arr[i]]--;
                drawSingleBar(i);
            }

            
            for (int i = 0; i < n; i++)
            {
                arr[i] = output[i];
                drawSingleBar(i);
                System.Threading.Thread.Sleep(30);
            }
        }
    }




}