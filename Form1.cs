using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        public Image ZdjecieSzachow;
        public int[,] map = new int[8, 8]
        {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
        };

        public Button[,] butts = new Button[8, 8];

        public int AktualnyGracz;

        public Button WczesniejszyPrzycisk;

        public bool Wruchu = false;

        /// <summary>
        /// ta funkcja wczytuje nam zdjęcie planszy do gry w szachy
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            ZdjecieSzachow = new Bitmap("chess.png");



            Init();
        }
        
        /// <summary>
        /// ta funkcja tworzy nam tablice o rozmiarach 8x8,1to białe pionki a 2 czarne
        /// </summary>
        public void Init()           
        {      
            InitializeComponent();
            
            map = new int[8, 8]
            {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
            };

            AktualnyGracz = 1;
            RysujMape();

        }
        /// <summary>
        /// rysowanie mapy jako tablicy dwu wymiarowej
        /// </summary>
        public void RysujMape() 
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j] = new Button();


                    Button butt = new Button();
                    butt.Size = new Size(50, 50);
                    butt.Location = new Point(j * 50, i * 50);
                    
                    switch (map[i, j] / 10)
                    {
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            g.DrawImage(ZdjecieSzachow, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part;
                            break;
                        case 2:
                            Image part1 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part1);
                            g1.DrawImage(ZdjecieSzachow, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part1;
                            break;
                    }
                    butt.BackColor = Color.White;
                    butt.Click += new EventHandler(PoWybraniu);
                    this.Controls.Add(butt);

                    butts[i, j] = butt;
                }
            }
        }
        /// <summary>
        /// ta funckja odpowiada za zmiane tła pionka
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PoWybraniu(object sender, EventArgs e)
        {
            if (WczesniejszyPrzycisk != null)
                WczesniejszyPrzycisk.BackColor = Color.White;

            Button WcisnietyPrzycisk = sender as Button;



            if (map[WcisnietyPrzycisk.Location.Y / 50, WcisnietyPrzycisk.Location.X / 50] != 0 && map[WcisnietyPrzycisk.Location.Y / 50, WcisnietyPrzycisk.Location.X / 50] / 10 == AktualnyGracz)
            {
                ZmienTloNaBialy();
                WcisnietyPrzycisk.BackColor = Color.Red;
                WylaczPrzyciski();
                WcisnietyPrzycisk.Enabled = true;
                PokazRuchy(WcisnietyPrzycisk.Location.Y / 50, WcisnietyPrzycisk.Location.X / 50, map[WcisnietyPrzycisk.Location.Y / 50, WcisnietyPrzycisk.Location.X / 50]);

                if (Wruchu)
                {
                    ZmienTloNaBialy();
                    WcisnietyPrzycisk.BackColor = Color.White;
                    AktywujPrzyciski();
                    Wruchu = false;
                }
                else
                    Wruchu = true;
            }
            else
            {
                if (Wruchu)
                {
                    int temp = map[WcisnietyPrzycisk.Location.Y / 50, WcisnietyPrzycisk.Location.X / 50];
                    map[WcisnietyPrzycisk.Location.Y / 50, WcisnietyPrzycisk.Location.X / 50] = map[WczesniejszyPrzycisk.Location.Y / 50, WczesniejszyPrzycisk.Location.X / 50];
                    map[WczesniejszyPrzycisk.Location.Y / 50, WczesniejszyPrzycisk.Location.X / 50] = temp;
                    WcisnietyPrzycisk.BackgroundImage = WczesniejszyPrzycisk.BackgroundImage;
                    WczesniejszyPrzycisk.BackgroundImage = null;
                    Wruchu = false;
                    ZmienTloNaBialy();
                    AktywujPrzyciski();
                    if (obliczenia.ZmianaGracza(AktualnyGracz))
                    {

                        AktualnyGracz = 2;


                    }
                    else
                    {
                        AktualnyGracz = 1;
                    }
                }
            }

            WczesniejszyPrzycisk = WcisnietyPrzycisk;
        }
        /// <summary>
        /// pokazuje możliwe ruchy dla wszystkich
        /// </summary>
        /// <param name="Wiersz">ile możliwych wierszy</param>
        /// <param name="Kolumna">ile możliwych kolumn</param>
        /// <param name="WybranaFigura">figura którą zamierzamy wykonać ruch</param>
        public void PokazRuchy(int Wiersz, int Kolumna, int WybranaFigura)
        {
            int Wyznacz = AktualnyGracz == 1 ? 1 : -1;
            switch (WybranaFigura % 10)
            {
                case 6:
                    if (obliczenia.NaPlanszy(Wiersz + 1 * Wyznacz, Kolumna))
                    {
                        if (map[Wiersz + 1 * Wyznacz, Kolumna] == 0)
                        {
                            butts[Wiersz + 1 * Wyznacz, Kolumna].BackColor = Color.Green;
                            butts[Wiersz + 1 * Wyznacz, Kolumna].Enabled = true;
                        }
                        if (map[Wiersz + 1 * Wyznacz, Kolumna] == 0 & (Wiersz == 1 & map[Wiersz, Kolumna] == 16 & map[Wiersz + 2 * Wyznacz, Kolumna] == 0) ^ (Wiersz == 6 & map[Wiersz, Kolumna] == 26 & map[Wiersz + 2 * Wyznacz, Kolumna] == 0))
                        {
                            butts[Wiersz + 2 * Wyznacz, Kolumna].BackColor = Color.Green;
                            butts[Wiersz + 2 * Wyznacz, Kolumna].Enabled = true;
                        }
                    }

                    if (obliczenia.NaPlanszy(Wiersz + 1 * Wyznacz, Kolumna + 1))
                    {
                        if (map[Wiersz + 1 * Wyznacz, Kolumna + 1] != 0 && map[Wiersz + 1 * Wyznacz, Kolumna + 1] / 10 != AktualnyGracz)
                        {
                            butts[Wiersz + 1 * Wyznacz, Kolumna + 1].BackColor = Color.Green;
                            butts[Wiersz + 1 * Wyznacz, Kolumna + 1].Enabled = true;
                        }
                    }
                    if (obliczenia.NaPlanszy(Wiersz + 1 * Wyznacz, Kolumna - 1))
                    {
                        if (map[Wiersz + 1 * Wyznacz, Kolumna - 1] != 0 && map[Wiersz + 1 * Wyznacz, Kolumna - 1] / 10 != AktualnyGracz)
                        {
                            butts[Wiersz + 1 * Wyznacz, Kolumna - 1].BackColor = Color.Green;
                            butts[Wiersz + 1 * Wyznacz, Kolumna - 1].Enabled = true;
                        }
                    }
                    break;
                case 5:
                    PionoweRuchy(Wiersz, Kolumna);
                    break;
                case 3:
                    SkosyRuchy(Wiersz, Kolumna);
                    break;
                case 2:
                    PionoweRuchy(Wiersz, Kolumna);
                    SkosyRuchy(Wiersz, Kolumna);
                    break;
                case 1:
                    PionoweRuchy(Wiersz, Kolumna, true);
                    SkosyRuchy(Wiersz, Kolumna, true);
                    break;
                case 4:
                    RuchKonia(Wiersz, Kolumna);
                    break;
            }
        }
        /// <summary>
        /// funkcja odpowiada za mozliwe ruchy konia
        /// </summary>
        /// <param name="Wiersz">ile wierszy</param>
        /// <param name="Kolumna">ile kolumn</param>
        public void RuchKonia(int Wiersz, int Kolumna)
        {
            if (obliczenia.NaPlanszy(Wiersz - 2, Kolumna + 1))
            {
                PokazRuch(Wiersz - 2, Kolumna + 1);
            }
            if (obliczenia.NaPlanszy(Wiersz - 2, Kolumna - 1))
            {
                PokazRuch(Wiersz - 2, Kolumna - 1);
            }
            if (obliczenia.NaPlanszy(Wiersz + 2, Kolumna + 1))
            {
                PokazRuch(Wiersz + 2, Kolumna + 1);
            }
            if (obliczenia.NaPlanszy(Wiersz + 2, Kolumna - 1))
            {
                PokazRuch(Wiersz + 2, Kolumna - 1);
            }
            if (obliczenia.NaPlanszy(Wiersz - 1, Kolumna + 2))
            {
                PokazRuch(Wiersz - 1, Kolumna + 2);
            }
            if (obliczenia.NaPlanszy(Wiersz + 1, Kolumna + 2))
            {
                PokazRuch(Wiersz + 1, Kolumna + 2);
            }
            if (obliczenia.NaPlanszy(Wiersz - 1, Kolumna - 2))
            {
                PokazRuch(Wiersz - 1, Kolumna - 2);
            }
            if (obliczenia.NaPlanszy(Wiersz + 1, Kolumna - 2))
            {
                PokazRuch(Wiersz + 1, Kolumna - 2);
            }
        }
        /// <summary>
        /// Funkcja wyłącza przyciski
        /// </summary>
        public void WylaczPrzyciski()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = false;

                }
            }
        }
        /// <summary>
        /// Funkcja włącza przyciski
        /// </summary>
        public void AktywujPrzyciski()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = true;
                }
            }
        }
        /// <summary>
        /// Funkcja ma na celu pokazanie możliwych ruchów po ukosie
        /// </summary>
        /// <param name="Wiersz">ilosc wierszy</param>
        /// <param name="Kolumna">ilosc kolumn</param>
        /// <param name="Pruch"></param>
        public void SkosyRuchy(int Wiersz, int Kolumna, bool Pruch = false)
        {
            int j = Kolumna + 1;
            for (int i = Wiersz - 1; i >= 0; i--)
            {
                if (obliczenia.NaPlanszy(i, j))
                {
                    if (!PokazRuch(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (Pruch)
                    break;
            }

            j = Kolumna - 1;
            for (int i = Wiersz - 1; i >= 0; i--)
            {
                if (obliczenia.NaPlanszy(i, j))
                {
                    if (!PokazRuch(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (Pruch)
                    break;
            }

            j = Kolumna - 1;
            for (int i = Wiersz + 1; i < 8; i++)
            {
                if (obliczenia.NaPlanszy(i, j))
                {
                    if (!PokazRuch(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (Pruch)
                    break;
            }

            j = Kolumna + 1;
            for (int i = Wiersz + 1; i < 8; i++)
            {
                if (obliczenia.NaPlanszy(i, j))
                {
                    if (!PokazRuch(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (Pruch)
                    break;
            }
        }
        /// <summary>
        /// funkcja  pokazuej pionowe i poziome ruchy
        /// </summary>
        /// <param name="Wiersz">ile wierszy </param>
        /// <param name="Kolumna"> ile kolumn</param>
        /// <param name="Pruch"></param>
        public void PionoweRuchy(int Wiersz, int Kolumna, bool Pruch = false)
        {
            for (int i = Wiersz + 1; i < 8; i++)
            {
                if (obliczenia.NaPlanszy(i, Kolumna))
                {
                    if (!PokazRuch(i, Kolumna))
                        break;
                }
                if (Pruch)
                    break;
            }
            for (int i = Wiersz - 1; i >= 0; i--)
            {
                if (obliczenia.NaPlanszy(i, Kolumna))
                {
                    if (!PokazRuch(i, Kolumna))
                        break;
                }
                if (Pruch)
                    break;
            }
            for (int j = Kolumna + 1; j < 8; j++)
            {
                if (obliczenia.NaPlanszy(Wiersz, j))
                {
                    if (!PokazRuch(Wiersz, j))
                        break;
                }
                if (Pruch)
                    break;
            }
            for (int j = Kolumna - 1; j >= 0; j--)
            {
                if (obliczenia.NaPlanszy(Wiersz, j))
                {
                    if (!PokazRuch(Wiersz, j))
                        break;
                }
                if (Pruch)
                    break;
            }
        }
        /// <summary>
        /// funkcja pokazuje możliwe ruchy pionków
        /// </summary>
        /// <param name="Wiersz">ile wierszy</param>
        /// <param name="j">podkreslenia na zielono</param>
        /// <returns></returns>
        public bool PokazRuch(int Wiersz, int j)
        {
            if (map[Wiersz, j] == 0)
            {
                butts[Wiersz, j].BackColor = Color.Green;
                butts[Wiersz, j].Enabled = true;
            }
            else
            {
                if (map[Wiersz, j] / 10 != AktualnyGracz)
                {
                    butts[Wiersz, j].BackColor = Color.Green;
                    butts[Wiersz, j].Enabled = true;
                }
                return false;
            }
            return true;
        }

       
        /// <summary>
        /// funkcja zmienia tlo na biale
        /// </summary>
        public void ZmienTloNaBialy()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].BackColor = Color.White;
                }
            }
        }

        

        private void Button1_Click(object sender, EventArgs e)
        {

            this.Controls.Clear();
            Init();
        }
    }
}
