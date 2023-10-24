using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace автосервис
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if(SelectedService != null)
            {
                this._currentService = SelectedService;
            }
            DataContext = _currentService;

            var _currentClient = Yamaletdinov_AutoServiceEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }

        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");

            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");

            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if(_currentClientService.ID == 0)
            {
                Yamaletdinov_AutoServiceEntities.GetContext().ClientService.Add(_currentClientService);
            }
            try
            {
                Yamaletdinov_AutoServiceEntities.GetContext().SaveChanges();
                MessageBox.Show("изменения сохранены");
                Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }
        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            string filtered = new string(s.Where(c => char.IsDigit(c) || c == ':').ToArray());

            if (s != filtered)
            {
                TBEnd.Text = "";
                TBStart.Text = TBStart.Text.Substring(TBStart.Text.Length);
                return;
            }
            if (s.Length < 5 || !s.Contains(':'))
            {
                return;
            }
            else
            {
                string[] start = s.Split(':');
                int startHour = Convert.ToInt32(start[0]) * 60;
                int startMin = Convert.ToInt32(start[1]);

                int sum = startHour + startMin + Convert.ToInt32(_currentService.Duration);

                int EndHour = sum / 60;
                int EndMin = sum % 60;
                if (EndHour > 23)
                {
                    EndHour -= 24;
                    s = "0" + EndHour.ToString() + ":" + EndMin.ToString();
                }
                if (start[1].Contains('0') && startMin < 10)
                {
                    s = EndHour.ToString() + ":0" + EndMin.ToString();

                }
                else
                {
                    s = EndHour.ToString() + ":" + EndMin.ToString();
                }
                TBEnd.Text = s;
            }
        }

        private void TBEnd_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
    }
}
