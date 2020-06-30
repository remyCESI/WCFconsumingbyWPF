using ConsumingWCFServiceInWPFApp.proxy;
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

namespace ConsumingWCFServiceInWPFApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private STC_MSG msg;
        private AuthClient client;
        public MainWindow()
        {
            InitializeComponent();
            this.msg = new STC_MSG();
            client = new AuthClient("authTcp");
        }
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {

            string vlogin = txtLogin.Text;
            string vpwd = txtPwd.Text;

            if ((vlogin != "") && (vpwd != ""))
            {
                this.msg.user_login = vlogin;
                this.msg.user_psw = vpwd;

                M_auhentifier(this.msg);

                if (this.msg.op_statut == true)
                {

                    MessageBox.Show("Vous êtes connectés");
                }
                else
                {
                    MessageBox.Show("Vous n'êtes pas connectés");
                }
            }
            else
            {
                MessageBox.Show("Veuillez renseigner tous les champs");
            }


        }
        private void M_auhentifier(STC_MSG msg)
        {
            this.msg = msg;
            this.msg.op_name = "authentifier";
            this.msg.app_name = "Middleware";
            this.msg.app_token = "apptoken";
            this.msg.app_version = "2.0";
            this.msg.op_info = "Demande de service de l'application 1 de version 2.0";
            this.msg = this.client.Login(this.msg);
        }
    }
}
