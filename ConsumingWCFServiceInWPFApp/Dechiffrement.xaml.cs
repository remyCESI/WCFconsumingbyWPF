using Microsoft.Win32;
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
        public Dechiffrement()
        {
            InitializeComponent();
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
                    Textdisplay.Text = Textdisplay.Text + Environment.NewLine + File.ReadAllText(filename);

					//add in lfile some information like name and contains in order to send to middleware later by json
					lfile.Add(new ClassFile()
					{
						name = Path.GetFileName(filename),
						text = File.ReadAllText(filename)
					});
				}

				object[] files = new object[lfile.Count];

				var index = 0;
				foreach(ClassFile f in lfile)
				{
					files[index] = f.name;
					//files[index] = new Newtonsoft.Json.JsonConvert.SerializeObject(new { name = f.name, text = f.text });
					index++;
				}

				new MainWindow().GetDataFromFiles(files);
			}
		}
	}

	public class ClassFile
	{
		public string name;
		public string text;

		public ClassFile() { }
	}
}
