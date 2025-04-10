namespace Captura.GifScreen.App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            Application.ThreadException += TrataExcecao;

            bool silentMode = args.Contains("/silent");
            if (silentMode)
            {
                var form = new Form1();
                form.WindowState = FormWindowState.Minimized;
                form.ShowInTaskbar = false;
                Application.Run(form);
            }
            else
                Application.Run(new Form1());
        }


        static void TrataExcecao(object sender, ThreadExceptionEventArgs e)
        {
            string directoryMyDocuments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "logs");

            if(!Directory.Exists(directoryMyDocuments)) 
                Directory.CreateDirectory(directoryMyDocuments);

            string caminhoLog = Path.Combine(directoryMyDocuments, "logsException.log");


            try
            {
                File.AppendAllText(caminhoLog, $"\r\n\r\n[Erro] - {DateTime.Now} \r\nMensagem: {e.Exception.Message} \r\nStackTrace: {e.Exception.StackTrace}");
            }
            catch { }
        }
    }
}