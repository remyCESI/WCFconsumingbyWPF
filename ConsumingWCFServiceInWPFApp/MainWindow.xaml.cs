using ConsumingWCFServiceInWPFApp.AuthProxy;
using ConsumingWCFServiceInWPFApp.DecryptProxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
namespace ConsumingWCFServiceInWPFApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthProxy.STC_MSG msg;
        private AuthClient authClient;

		private Dechiffrement pdechiffrement;

		public MainWindow()
        {
            InitializeComponent();

            this.msg = new AuthProxy.STC_MSG();
            this.authClient = new AuthClient("authTcp");
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

                    this.pdechiffrement = new Dechiffrement
                    {
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };

                    pdechiffrement.Show();
                    Hide();
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

        private void M_auhentifier(AuthProxy.STC_MSG msg)
        {
            this.msg = msg;
            this.msg.op_name = "authentifier";
            this.msg.app_name = "Middleware";
            this.msg.app_token = "apptoken";
            this.msg.app_version = "2.0";
            this.msg.op_info = "Demande de service de l'application 1 de version 2.0";

            this.msg = this.authClient.Login(this.msg);
        }
    }
}
