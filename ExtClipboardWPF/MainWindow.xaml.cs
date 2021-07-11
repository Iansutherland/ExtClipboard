using ExtClipboardRedis;
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

namespace ExtClipboardWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RedisRepository repository;
        public MainWindow()
        {
            InitializeComponent();
            this.repository = new RedisRepository();
        }

        private void GetRedisData()
        {
            var redisValues = this.repository.GetAllList("testList");
            if(redisValues != null && redisValues.Length != 0)
            {
                foreach(var redisValue in redisValues)
                {
                    this.listBox1.Items.Add(redisValue);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetRedisData();
        }
    }
}
