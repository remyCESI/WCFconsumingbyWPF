namespace ConsumingWCFServiceInWPFApp
{
    public class cFile
    {
        public string name { get; set; }
        public string text { get; set; }

        public cFile() { }

        public cFile(string name, string text)
        {
            this.name = name;
            this.text = text;
        }
    }
}