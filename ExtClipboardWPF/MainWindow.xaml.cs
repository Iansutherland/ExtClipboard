using ExtClipboardRedis;
using ExtClipboardWPF.Services;
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
        private InterceptKeys keyboardIntercept;
        public MainWindow()
        {
            InitializeComponent();
            this.repository = new RedisRepository();
            this.keyboardIntercept = new InterceptKeys(InterceptKeyCallback);
            this.keyboardIntercept.StartHook();
        }

        private void InterceptKeyCallback(int keyCode)
        {
            string text = $"keycode Pressed: {keyCode}";
            this.listBox1.Items.Add(text);
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
            else
            {
                this.listBox1.Items.Add("No values in redis list \"testList\"");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetRedisData();
        }

        protected override void OnClosed(EventArgs e)
        {
            keyboardIntercept.EndHook();
        }
    }
}
