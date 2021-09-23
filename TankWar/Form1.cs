using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankWar
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Для задания позиции кнопок и лейблов
        /// </summary>
        private int posButX = 35;
        /// <summary>
        /// Для итерации по массиву
        /// </summary>
        private int Iterator = 0;

        public Bitmap background = new Bitmap(@"C:\Users\Иван\source\repos\TankWar\TankWar\Images\Background.jpg");

        public Button AtackButton;

        Tank[] T34 = { new Tank("T34"), new Tank("T34"), new Tank("T34"), new Tank("T34"), new Tank("T34") };
        Tank[] Pantera = { new Tank("Pantera"), new Tank("Pantera"), new Tank("Pantera"), new Tank("Pantera"), new Tank("Pantera") };
        WorldOfTanks WarTanks = new WorldOfTanks();
        Timer timer1;
        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            InitButton();
            initTexture();
            initTimer();
        }
        private void ShowLabelWin(int loc)
        {
            Label LabelWin = new Label();
            LabelWin.AutoSize = true;
            LabelWin.Font = new Font("Microsoft Sans Serif", 21.9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            LabelWin.Location = new Point(300, loc);
            LabelWin.Size = new Size(150, 100);
            LabelWin.Text = Massage.Text;
            this.Controls.Add(LabelWin);

            Label LabelT34 = new Label();
            LabelT34.AutoSize = true;
            LabelT34.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            LabelT34.Location = new Point(10, loc);
            LabelT34.Size = new Size(150, 100);
            LabelT34.Text = $"Имя: {T34[Iterator].Name} " +
                             $"\nБроня: {T34[Iterator].Armor} " +
                            $"\nАммуниция: {T34[Iterator].Ammunition} " +
                             $"\nМфневренность: {T34[Iterator].Maneuverability}";
            this.Controls.Add(LabelT34);

            Label LabelPantera = new Label();
            LabelPantera.AutoSize = true;
            LabelPantera.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            LabelPantera.Location = new Point(700, loc);
            LabelPantera.Size = new Size(150, 100);
            LabelPantera.Text = $"Имя: {Pantera[Iterator].Name} " +
                             $"\nБроня: {Pantera[Iterator].Armor} " +
                            $"\nАммуниция: {Pantera[Iterator].Ammunition} " +
                             $"\nМфневренность: {Pantera[Iterator].Maneuverability}";
            this.Controls.Add(LabelPantera);

        }
        
        private void InitButton()
        {
            AtackButton = new Button();
            AtackButton.Location = new Point(390, posButX);
            AtackButton.Size = new Size(100, 50);
            AtackButton.Text = "ATACK";
            AtackButton.UseVisualStyleBackColor = true;
            AtackButton.Click += new EventHandler(AtackButton_Click);
            this.Controls.Add(AtackButton);
        }
        private void AtackButton_Click(object sender, EventArgs e)
        {
           
            
            WarTanks.War(T34[Iterator], Pantera[Iterator]);
            ShowLabelWin(posButX);
            AtackButton.Location = new Point(390, posButX += 100);
            Iterator++;
            if (Iterator > 4)
            {
                AtackButton.Dispose();
            }
            
        }
        private void initTimer()
        {
            timer1 = new Timer();
            timer1.Enabled = true;
            timer1.Interval = 25;
            timer1.Tick += new EventHandler(timer1_Tick);

        }
        private void timer1_Tick(object obj, EventArgs e)
        {
            Refresh();
        }
        private void initTexture()
        {
            this.BackgroundImage = background;
            this.Paint += new PaintEventHandler(Form1_Paint);
        }
        private void Form1_Paint(object obj, PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            for (int i = 0; i < 5; i++)
            {
                var TankBlueRect = new Rectangle(150, 25 + i * 100, 100, 70);
                var TankRedRect = new Rectangle(600, 25 + i * 100, 100, 70);


                g.DrawImage(T34[i].Texture, TankBlueRect);
                g.DrawImage(Pantera[i].Texture, TankRedRect);
            }
        }
    }

    public delegate void Win(Tank tank);

    /// <summary>
    /// Класс сообщений
    /// </summary>
    public static class Massage
    {
        public static string Text { get; set; }
      
        public static void MasssageWin(Tank tank)
        {
           Text = $"Победил {tank.Name}";
        }
        
    }

    /// <summary>
    /// Класс для реализации сражения танков
    /// </summary>
    public class WorldOfTanks
    {
        private event Win win;

        public WorldOfTanks()
        {
            win += Massage.MasssageWin;
        }
        public void War(Tank one, Tank two)
        {
            Tank tmp = new Tank(" ");
            try
            {
              tmp = one * two;
                win?.Invoke(tmp);
            }
            catch (ExceptionWar ew)
            {
                Massage.Text = ew.Draw;
            }

        }

    }

    /// <summary>
    /// Класс с текстурами танков
    /// </summary>
    public  class TextureTanks
    {
        private Bitmap[] TankBlue = { Resource1.TankBlue_1, Resource1.TankBlue_2, Resource1.TankBlue_3, Resource1.TankBlue_4 };

        private Bitmap[] TankRed = { Resource1.TankRed_1, Resource1.TankRed_2, Resource1.TankRed_3, Resource1.TankRed_4 };

        public Bitmap GetTextureBlueTank()
        {
            return TankBlue[new Random().Next(0, 4)];
        }
        public Bitmap GetTextureRedTank()
        {
            return TankRed[new Random().Next(0, 4)];
        }
    }

    /// <summary>
    /// Исключение. 
    /// </summary>
    public class ExceptionWar :  Exception
    {
        public string Draw = "Ничья!";
    }

    /// <summary>
    /// Класс танка
    /// </summary>
    public class Tank
    {
        public Bitmap Texture { get; private set; }
        public string Name { get; private set; }
        public int Ammunition { get; private set; }
        public int Armor { get; private set; }
        public int Maneuverability { get; private set; }

        public Tank(string name)
        {
            
            
            Name = name;
            var r = new Random();
            Ammunition = r.Next(0, 100);
            Task.Delay(10).GetAwaiter().GetResult();
            Armor = r.Next(0, 100);
            Task.Delay(10).GetAwaiter().GetResult();
            Maneuverability = r.Next(0, 100);
            Task.Delay(10).GetAwaiter().GetResult();
            if (Name == "T34")
            {
                Texture = new TextureTanks().GetTextureBlueTank();
                
            }
            else if (Name == "Pantera")
            {
                Texture = new TextureTanks().GetTextureRedTank();
            }
        }

        /// <summary>
        /// Перегруженный оператор произведения
        /// </summary>
        /// <param name="one">
        /// Первый танк
        /// </param>
        /// <param name="two">
        /// Второй танк
        /// </param>
        /// <returns>
        /// Вернёт первый танк, если его парпаметры больше
        /// 
        /// Вернёт второй танк если его параметры больше
        /// 
        /// Если парметры танков равны выбрасывается исключение ExceptionWar.
        /// 
        /// </returns>
        public static Tank operator *(Tank one, Tank two)
        {
            int blue = 0, red = 0;
            if (one.Ammunition > two.Ammunition)
            {
                blue++;
            }
            else if(one.Ammunition < two.Ammunition)
            {
                red++;
            }

            if (one.Armor > two.Armor)
            {
                blue++;
            }
            else if(one.Armor < two.Armor)
            {
                red++;
            }
            if (one.Maneuverability > two.Maneuverability)
            {
                blue++;
            }
            else if(one.Maneuverability < two.Maneuverability)
            {
                red++;
            }
            if (blue == red)
            {
                throw new ExceptionWar();
            }

            return blue > red ? one : two;
        }
       
    }

}
