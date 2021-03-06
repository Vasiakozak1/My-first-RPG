﻿ using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Threading;
namespace My_first_RPG
{
    
    
   public class Activity
    {
        public static DirectoryInfo ThisDirectory = Directory.CreateDirectory(Directory.GetCurrentDirectory());

        private List<Choice> PossibleActions=null;
        /// <summary>
        /// Гравець клас якого вар
        /// </summary>
        public Warrior PlayerWarrior = null;
        /// <summary>
        /// Гравець, клас якого жрець
        /// </summary>
        private Priest PlayerPriest = null;
        private StackPanel PanelForText = null;// Текст бокс куда записується процес гри, треба синхронізовувати з Текст боксом що у GameWindow
        private Border GameBorder;// Використовується щоб додавати туда кнопки, коли треба робити вибір під час гри, а потім синхронізувати з Грідом у GameWindow
        private Monster CelectedEnemy = null;// Використовується в двох таймерах бою(1-ий для гравця, другий для Enemy
        private GameWindow GWindow = null;
        private DispatcherTimer PlayerTimer = new DispatcherTimer();
        private DispatcherTimer EnemyTimer = new DispatcherTimer();
        /// <summary>
        /// Сюда добавляються ексемпляри кнопок в Border, спочатку всі під ряд у методі OfferBaseActions , потім лиш ті які не являються visible
        /// </summary>
        public static List<Button> AvailableBtns = new List<Button>();
        private Stack<Button> RemovedButtons = null;

        public Activity(Player player, StackPanel Panel,Border MainBorder,GameWindow WinForGame)
        {
            if (player is Warrior)
                PlayerWarrior = player as Warrior;
            else
                PlayerPriest = player as Priest;
            this.PossibleActions = new List<Choice>();
            this.PanelForText = Panel;

            this.GameBorder = MainBorder;
            this.GameBorder = this.OfferBaseActions();
            this.GWindow = WinForGame;
            this.RemovedButtons = new Stack<Button>();
            this.GWindow.LabelPlayerName.Content = this.PlayerWarrior.Name;
            
            
        }
        public Border OfferBaseActions()//прості дії йти по напрямку і шукати монстрів
        {
            string[] BaseActions = { "Йти в сторону пiвночi", "Йти в сторону пiвдня", "Йти в сторону сходу", "Йти в сторону заходу", "Показати монстрiв", };

            StackPanel coll = this.GameBorder.Child as StackPanel;
            
            foreach (UIElement element in coll.Children)
            {
                if (element is Button)
                    AvailableBtns.Add(element as Button);
            }
            int count = 0;
            for (; count < BaseActions.Length;)
            {
                AvailableBtns[0].Visibility = Visibility.Visible;
                PossibleActions.Add(new Choice(BaseActions[count], AvailableBtns[0]));
                AvailableBtns.RemoveAt(0);
                count++;
            }

            Border tmp = this.GameBorder;
            coll = tmp.Child as StackPanel;
            coll.Children.Clear();
            foreach (Choice ch in PossibleActions)
                coll.Children.Add(this.InitializeEventForButton(ch.Btn));
            tmp.Child = coll;
            
            return tmp;
           

        }
        public void WarriorMainActivity()
        {
            if (PlayerWarrior == null)
                return;
            DispatcherTimer GameTimer = new DispatcherTimer();
            GameTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);
            GameTimer.Tick += this.GameActivityTimer;
            GameTimer.Start();
        }

        /// <summary>
        /// Використовується коли треба оновити текстбокс під час виведення нової інфи 
        /// </summary>
        /// <param name="NewTextBox"></param>
        /// <returns></returns>
        public StackPanel RefreshStackPanel()
        {
            return this.PanelForText;
        }
        public Border RefreshBorder()
        {
            return this.GameBorder;
        }



        /// <summary>
        /// Повертає всі доступні екземпляри класу Choice у вигляді string
        /// </summary>
        /// <returns></returns>
        
        private Button InitializeEventForButton(Button ButtonForInit)
        {
            ButtonForInit.Click += ButtonHandler;
            return ButtonForInit;
        }

        /// <summary>
        /// Викликається коли нажимається якась кнопка в Бордері
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ShowEnemiesWindow showmonstrswindow = null;
        private void ButtonHandler(object sender,RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string Message;
            //Покищо не добавив монстрів у ексземпляри Place
            switch (btn.Content.ToString())
            {
                case "Йти в сторону пiвночi":
                    Message = this.PlayerWarrior.Move(Directions.Північ, this.PlayerWarrior.CurrentPlace)+"\n";
                    this.AddTextBlockToPanel(Message);
                    break;
                case "Йти в сторону пiвдня":
                    Message = this.PlayerWarrior.Move(Directions.Південь, this.PlayerWarrior.CurrentPlace) + "\n";
                    this.AddTextBlockToPanel(Message);
                    break;
                case "Йти в сторону сходу":
                    Message = this.PlayerWarrior.Move(Directions.Схід, this.PlayerWarrior.CurrentPlace) + "\n";
                    this.AddTextBlockToPanel(Message);
                    break;
                case "Йти в сторону заходу":
                    Message = this.PlayerWarrior.Move(Directions.Захід, this.PlayerWarrior.CurrentPlace) + "\n";
                    this.AddTextBlockToPanel(Message);
                    break;
                case "Показати монстрiв":
                    showmonstrswindow = new ShowEnemiesWindow(this.PlayerWarrior.CurrentPlace.Mobs.ToArray());
                    showmonstrswindow.Show();
                    showmonstrswindow.Closed += TakeMonster;  //коли вибрали противника    
                    
                                                                           
                    break;

            }
            
        }
        private void TakeMonster(object sender,EventArgs e)
        {
            if (showmonstrswindow.RetMon() == null)
                return;
            Monster Enemy = showmonstrswindow.RetMon();
            this.CelectedEnemy = Enemy;
            string Message = string.Format($"{this.PlayerWarrior.Name} проти {Enemy.Name}\n");
            this.AddTextBlockToPanel(Message);
            this.Fight(this.PlayerWarrior, Enemy);
        }
        private void Fight(Warrior Player, Monster Enemy)
        {
            this.GWindow.LabelEnemyName.Content = Enemy.Name;
            this.GWindow.ElipseEnemyHealth.Visibility = Visibility.Visible;
            this.GWindow.LabelEnemyHealth.Visibility = Visibility.Visible;
            this.GWindow.LabelEnemyName.Visibility = Visibility.Visible;

            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)Player.TotalSpeed);
            EnemyTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)Enemy.TotalSpeed);
            PlayerTimer.Tick += PlayerAction;
            EnemyTimer.Tick += EnemyAction;
            EnemyTimer.Start();
            PlayerTimer.Start();

            //
            this.HideButtonsInLeftBorder();
           // 
        }
        /// <summary>
        /// Ховає всі кнопки в лівому бордері
        /// </summary>
        private void HideButtonsInLeftBorder()
        {
            StackPanel TmpPanel = this.GWindow.Border1.Child as StackPanel;
            foreach (Button tmpbtn in TmpPanel.Children)
                tmpbtn.Visibility = Visibility.Hidden;
        }
        
        private void ShowButtonsInLeftBorder()
        {
            StackPanel TmpPanel = this.GWindow.Border1.Child as StackPanel;
            foreach (Button tmpbtn in TmpPanel.Children)
                tmpbtn.Visibility = Visibility.Visible;
        }

        private void PlayerAction(object sender,EventArgs e)
        {
            
            DispatcherTimer tim = sender as DispatcherTimer;
            if (this.CelectedEnemy == null)
            {
                tim.Stop();
                return;
            }
            if (!this.PlayerWarrior.IsAlive)
            {
                this.EndGame();
                return;
            }
            if (!this.CelectedEnemy.IsAlive)
                tim.Stop();
            string Message = this.PlayerWarrior.Attack(this.CelectedEnemy) + "\n";
            this.AddTextBlockToPanel(Message);
            
        }
        private void EndGame()
        {
            MessageBox.Show("На жаль, гра завершена");
            this.GWindow.Close();
        }
        private void EnemyAction(object sender,EventArgs e)
        {
            DispatcherTimer tim = sender as DispatcherTimer;
            if (!this.PlayerWarrior.IsAlive || !this.CelectedEnemy.IsAlive)
            {
                tim.Stop();
                this.GWindow.ElipseEnemyHealth.Visibility = Visibility.Hidden;
                this.GWindow.LabelEnemyHealth.Visibility = Visibility.Hidden;
                this.GWindow.LabelEnemyName.Visibility = Visibility.Hidden;
                #region Лут
                this.Looting(this.CelectedEnemy.DropList, this.PlayerWarrior.CurrentLocation.GlobalDropList,this.CelectedEnemy.DropCoefficient);             

                #endregion
                this.CelectedEnemy = null;
                this.ShowButtonsInLeftBorder();
            }
            if (this.CelectedEnemy != null && this.CelectedEnemy.IsAlive)
            {
                string Message = this.CelectedEnemy.AutoAttack(this.PlayerWarrior) + "\n";
                this.AddTextBlockToPanel(Message);
            }
        }

        private int ColorCounter = 0; //Для того щоб Бекграунд текста був різний
        /// <summary>
        /// Добавляє текстблок із текстом в стакпанел
        /// </summary>
        /// <param name="Text">текст, який треба добавити</param>
        public void AddTextBlockToPanel(string Text)
        {
            TextBlock tb = new TextBlock();
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = Text;
            tb.Background = this.ColorCounter % 2 == 0 ? Brushes.Gray : Brushes.LightGray;
            tb.FontSize = 14;           
            GWindow.RegisterName("textblock" + ColorCounter, tb);
            this.PanelForText.Children.Add(tb);
            this.ColorCounter++;
        }
        private void GameActivityTimer(object sender, EventArgs args)
        {
            this.RefreshElipseHealthFury();
        }     
        private void RefreshElipseHealthFury()
        {
            #region Оновлення еліпса зі здоров`ям гравця
            double Relation = 1 - (double)((double)this.PlayerWarrior.Health / (double)PlayerWarrior.MaxHealth);// Відношення поточного здоров`я до максимального
            GradientStop GradStop1 = new GradientStop(Colors.White, Relation - 0.01);      
            GradientStop GradStop2 = new GradientStop(Colors.Green, Relation);
            LinearGradientBrush GradBrush = new LinearGradientBrush();
            GradBrush.StartPoint = new Point(0.5, 0);
            GradBrush.EndPoint = new Point(0.5, 1);
            if (this.GWindow.FindName(nameof(GradStop1)) != null && this.GWindow.FindName(nameof(GradStop2)) != null)
            {
                this.GWindow.UnregisterName(nameof(GradStop1));
                this.GWindow.UnregisterName(nameof(GradStop2));
            }          
            this.GWindow.RegisterName(nameof(GradStop1), GradStop1);
            this.GWindow.RegisterName(nameof(GradStop2), GradStop2);
            GradBrush.GradientStops.Add(GradStop1);
            GradBrush.GradientStops.Add(GradStop2);
            this.GWindow.ElipseHealth.Fill = GradBrush;
            this.GWindow.LabelPlayerHealth.Content = this.PlayerWarrior.Health;
            #endregion

            if (this.CelectedEnemy == null)
                return;
            else
            {
                #region Оновлення еліпса із хп противника
                 Relation = 1 - (double)((double)this.CelectedEnemy.Health / (double)CelectedEnemy.MaxHealth); // Відношення поточного здоров`я до максимального
                 GradStop1 = new GradientStop(Colors.White, Relation - 0.01);
                 GradStop2 = new GradientStop(Colors.Green, Relation);
                 GradBrush = new LinearGradientBrush();
                 GradBrush.StartPoint = new Point(0.5, 0);
                 GradBrush.EndPoint = new Point(0.5, 1);
                if (this.GWindow.FindName(nameof(GradStop1) + "enemy") != null && this.GWindow.FindName(nameof(GradStop2) + "enemy") != null) 
                {
                    this.GWindow.UnregisterName(nameof(GradStop1) + "enemy");
                    this.GWindow.UnregisterName(nameof(GradStop2) + "enemy");
                }
                this.GWindow.RegisterName(nameof(GradStop1)+"enemy", GradStop1);
                this.GWindow.RegisterName(nameof(GradStop2)+"enemy", GradStop2);
                GradBrush.GradientStops.Add(GradStop1);
                GradBrush.GradientStops.Add(GradStop2);
                this.GWindow.ElipseEnemyHealth.Fill = GradBrush;
                this.GWindow.LabelEnemyHealth.Content = this.CelectedEnemy.Health;
                #endregion
            }

            if (this.PlayerWarrior.Fury == 0)
                return;
            else
            {
                Relation = 1 - (double)((double)this.PlayerWarrior.Fury / (double)100);// Відношення поточного здоров`я до максимального
                #region Оновлення еліпса з очками люті гравця
                Relation = 1 - (double)((double)this.PlayerWarrior.Fury / (double)100);// Відношення поточного здоров`я до максимального
                GradStop1 = new GradientStop(Colors.Crimson, Relation );
                GradStop2 = new GradientStop(Colors.White, Relation-0.01);
                GradBrush = new LinearGradientBrush();
                GradBrush.StartPoint = new Point(0.5, 0);
                GradBrush.EndPoint = new Point(0.5, 1);

                if (this.GWindow.FindName(nameof(GradStop1) + "fury") != null && this.GWindow.FindName(nameof(GradStop2) + "fury") != null)
                {
                    this.GWindow.UnregisterName(nameof(GradStop1) + "fury");
                    this.GWindow.UnregisterName(nameof(GradStop2) + "fury");
                }
                this.GWindow.RegisterName(nameof(GradStop1)+"fury", GradStop1);
                this.GWindow.RegisterName(nameof(GradStop2)+"fury", GradStop2);
                GradBrush.GradientStops.Add(GradStop1);
                GradBrush.GradientStops.Add(GradStop2);
                this.GWindow.ElipseFury.Fill = GradBrush;
                this.GWindow.LabelPlayerFury.Content = this.PlayerWarrior.Fury;
                #endregion
            }
        }
        /// <summary>
        /// Включає в себе появу кновки показати лут, генерація луту, показ вікна з лутом, отримання луту
        /// </summary>
        private void Looting(Dictionary<int,Item> LocalLoot,Dictionary<int,Item> GlobalLoot,float DropKoof)
        {
            #region Ініціалізація кнопки-бордера для показу лута

            Border ButtonLootBorder = new Border();
            ButtonLootBorder.CornerRadius = new CornerRadius(20);
            RadialGradientBrush gradient = new RadialGradientBrush();
            GradientStop stop1 = new GradientStop();
            stop1.Color = new Color() { R = 0xfF, G = 0x78, B = 0x4b,A=255  };
            stop1.Offset = 1;
            GradientStop stop2 = new GradientStop();
            stop2.Color = new Color() { R = 0xFF, G = 0xBD, B = 0xDE,A=255  };
            stop2.Offset = 0.451;
            GradientStop stop3 = new GradientStop();
            stop3.Color = new Color() { R = 0xFF, G = 0xCB, B = 0xDA, A = 255  };
            stop3.Offset = 0.708;
            gradient.GradientStops.Add(stop1); gradient.GradientStops.Add(stop2);gradient.GradientStops.Add(stop2);

            TextBlock BlockOfText = new TextBlock();
            BlockOfText.Text = "Показати здобич";
            BlockOfText.Margin = new Thickness(5, 15, 5, 5);
            BlockOfText.FontSize = 22.0;
            BlockOfText.TextWrapping = TextWrapping.Wrap;
            BlockOfText.FontFamily = new FontFamily("Georgia");
            BlockOfText.FontWeight = FontWeights.Bold;
            BlockOfText.TextAlignment = TextAlignment.Center;
            this.GWindow.RegisterName("LootTextBlock", BlockOfText);

            ButtonLootBorder.Child = BlockOfText;
            ButtonLootBorder.Margin = new Thickness(890, 625, 313, 17);
            ButtonLootBorder.Background = gradient;

            ButtonLootBorder.MouseDown += (sender, e)=>{
                ShowLootWindow showLootWnd = new 
                ShowLootWindow(LocalLoot, GlobalLoot, this.GWindow.InventoryWindow, DropKoof);
                showLootWnd.Show();
                ButtonLootBorder.Visibility = Visibility.Hidden;
            };

            this.GWindow.RegisterName("ButtonLoot", ButtonLootBorder);
            this.GWindow.Grid1.Children.Add(ButtonLootBorder);
            #endregion


        }
    }
    
    public partial class GameWindow : Window
    {
        public PlayersInventory InventoryWindow;
        public WindowEquipment WinEquip;
        private Activity Act;
        public GameWindow(string PlayerName)
        {         
            InitializeComponent();
            ////////////////////////////////////////////////////////////////////////////
            
            FileStream LoadFile = new FileStream(Activity.ThisDirectory.FullName + @"\Saves\" + PlayerName + @".dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            object player = bf.Deserialize(LoadFile);
            bool IsWarrior = player is Warrior;

            if (IsWarrior)
            {
                Act = new Activity((Warrior)player, this.PanelForText,this.Border1,this);
            }
            else
            {
                Act = new Activity((Priest)player, this.PanelForText,this.Border1,this);
                
            }
            //Процес гри
            if (IsWarrior)
            {
                this.InventoryWindow = new PlayersInventory(this.Act.PlayerWarrior.Inventory,this.WinEquip);
                this.WinEquip = new WindowEquipment(this.InventoryWindow, this.Act.PlayerWarrior.Equip);
                this.InventoryWindow.WindowForEquipment = this.WinEquip;// Костиль

                Act.WarriorMainActivity();
            }
            
         
            
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {          
            this.InventoryWindow.Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.WinEquip.Show();
        }
    }
}
