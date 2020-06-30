using ConsumingWCFServiceInWPFApp.AuthService;
using ConsumingWCFServiceInWPFApp.DecryptService;
using System;
using System.Windows;
namespace ConsumingWCFServiceInWPFApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthService.STC_MSG msg;
        private AuthClient authClient;
        private DecryptClient decryptClient;

        public MainWindow()
        {
            InitializeComponent();
            this.msg = new AuthService.STC_MSG();
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

                    Dechiffrement pdechiffrement = new Dechiffrement();
                    pdechiffrement.Show();

                    M_Decrypter(this.msg);
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

        private void M_auhentifier(AuthService.STC_MSG msg)
        {
            this.msg = msg;
            this.msg.op_name = "authentifier";
            this.msg.app_name = "Middleware";
            this.msg.app_token = "apptoken";
            this.msg.app_version = "2.0";
            this.msg.op_info = "Demande de service de l'application 1 de version 2.0";

            this.msg = this.authClient.Login(this.msg);
        }

        private void M_Decrypter(AuthService.STC_MSG msg)
        {
            DecryptService.STC_MSG msgData = new DecryptService.STC_MSG
            {
                op_name = "authentifier",
                app_name = "Middleware",
                app_token = "apptoken",
                app_version = "2.0",
                op_info = "Demande de service de l'application 1 de version 2.0"
            };

            this.msg = new AuthService.STC_MSG()
            {
                // To complete with all the informations
                data = msgData.data
            };
        }
    }
}
