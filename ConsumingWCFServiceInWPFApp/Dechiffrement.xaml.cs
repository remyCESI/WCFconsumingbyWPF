using ConsumingWCFServiceInWPFApp.DecryptProxy;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ConsumingWCFServiceInWPFApp
{
    /// <summary>
    /// Logique d'interaction pour Dechiffrement.xaml
    /// </summary>
    public partial class Dechiffrement : Window
	{
		private DecryptClient decryptClient;
		private AuthProxy.STC_MSG msg;

		public Dechiffrement()
        {
            InitializeComponent();
			this.decryptClient = new DecryptClient("decryptTcp");
		}

        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            //new Instance of openfiledialog to handle file throught graphic interface 
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                //you can only upload a file that is text
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                //we start the dialog box in document folder
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            //instance of List object ClassFile type
            List<ClassFile> lfile = new List<ClassFile>();

            if (openFileDialog.ShowDialog() == true)
            {
                //that means for each file upload do something 
                foreach (string filename in openFileDialog.FileNames)
                {
                    //we put the filename in a list in order to remind the user about his push
                    lbFiles.Items.Add(Path.GetFileName(filename));

                    //optional
                    // Textdisplay.Text = Textdisplay.Text + Environment.NewLine + File.ReadAllText(filename);

                    //add in lfile some information like name and contains in order to send to middleware later by json
                    lfile.Add(new ClassFile()
                    {
                        name = Path.GetFileName(filename),
                        text = File.ReadAllText(filename)
                    });
                }

                GetDataFromFiles(lfile);
            }
        }

		public void DisplayResult(string text = null)
		{
			if(text == null)
			{
				Textdisplay.Text = "";
			}
			else
			{
				Textdisplay.Text = Textdisplay.Text + Environment.NewLine + text;
			}
		}

		private void M_Decrypter(AuthProxy.STC_MSG msg, object[] file)
		{
			STC_MSG msgData = new STC_MSG
			{
				op_name = "decrypter",
				app_name = "Middleware",
				app_token = "apptoken",
				app_version = "2.0",
				op_info = "Demande de service de l'application 1 de version 2.0",
				data = file
			};

			DisplayResult(msgData.data[0].ToString());
			DisplayResult("en cours de traitement...");

			try
			{
				msgData = this.decryptClient.DecryptFiles(msgData);
			}
			catch (Exception e)
			{
				throw new Exception("Middleware error " + e.ToString());
			}

			if(msgData.data.Length > 2)
			{
				DisplayResult(msgData.data[0].ToString() + ": Traitement terminé !");

				/*
				Result r = new Result()
				{
					name = msgData.data[0].ToString(),
					text = msgData.data[1].ToString(),
					key = msgData.data[2].ToString()
				};

				DisplayResult("Filename = " + r.name + Environment.NewLine + "Text = " + r.text + Environment.NewLine + "Key = " + r.key);
				*/
			}
		}

		public void GetDataFromFiles(List<ClassFile> files)
		{
			// Clear by default
			DisplayResult();

			files.ForEach((ClassFile f) =>
			{
				object[] filesSent = new object[2];
				filesSent[0] = f.name;
				filesSent[1] = f.text;

				M_Decrypter(this.msg, filesSent);
			});

			MessageBox.Show("Files sent !");
		}
	}

	public class ClassFile
	{
		public string name;
		public string text;
	}

	public class Result
	{
		public string name;
		public string text;
		public string key;
	}
}