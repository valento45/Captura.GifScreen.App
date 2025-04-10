using Captura.GifScreen.App.Configuration;
using Captura.GifScreen.App.Model;
using ImageMagick;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Captura.GifScreen.App
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int HOTKEY_ID = 9000;
        const int HOTKEY_ID_GRAVACAO = 9001;
        const uint MOD_CONTROL = 0x0002; // Ctrl
        const uint MOD_ALT = 0x0001;     // Alt
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;


        static List<string> capturedFrames = new List<string>();
        static string outputGifPath = "gif_{0}.gif";
        private bool _gravando = false;
        private ContextMenuStrip contextMenu;
        Keys currentHotkeyGravacao;
        Keys currentHotkeyCaptura;
        static Thread captureThread;
        private ConfiguracoesSistema _configuracoesSistema;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            string nomeApp = "GifScreenApp";
            string caminho = Application.ExecutablePath;

            if (!AutoStartHelper.EstaRegistrado(nomeApp))
            {
                AutoStartHelper.RegistrarAutoInicio(nomeApp, caminho);
                //AutoStartHelper.CriarTarefaAgendada(caminho);
            }

        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;

            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                CapturarTela(); // Chama sua função de captura
            }
            else if (m.WParam.ToInt32() == HOTKEY_ID_GRAVACAO)
            {
                if (_gravando)
                    PararGravacao();

                else
                    GravarTela();

            }

            base.WndProc(ref m);
        }



        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            //UnregisterHotKey(this.Handle, HOTKEY_ID);
            //UnregisterHotKey(this.Handle, HOTKEY_ID_GRAVACAO);
            base.OnFormClosing(e);

            e.Cancel = true;
            this.Hide();
        }


        private void Form1_Shown(object sender, EventArgs e)
        {

            CriarOuVerificarPasta();
            SetupNotifyIcon();


            ObterEPopularCamposConfiguracoesAtuais();
            this.Hide();
        }

        private void ObterEPopularCamposConfiguracoesAtuais()
        {
            var configuracao = ConfiguracoesSistema.ObterConfiguracoesDoSistema();

            chkIniciarComWindows.Checked = configuracao.IniciarComSistema;
            txtAtalhoCaptura.Text = ConfiguracoesSistema.ConverterKeysParaTexto(configuracao.TeclasCaptura);
            txtAtalhoGravacao.Text = ConfiguracoesSistema.ConverterKeysParaTexto(configuracao.TeclasGravacao);


            _configuracoesSistema = configuracao;

            RegistrarAtalho(_configuracoesSistema.TeclasCaptura, HOTKEY_ID);
            RegistrarAtalho(_configuracoesSistema.TeclasGravacao, HOTKEY_ID_GRAVACAO);
        }


        private void CapturarTela()
        {

            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                string caminho = Path.Combine(ObterPastaCapturas(), $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                bitmap.Save(caminho, ImageFormat.Png);
                Clipboard.SetImage(bitmap);

                notifyIcon1.ShowBalloonTip(2000, "Captura", $"Imagem salva em: {caminho}", ToolTipIcon.Info);
            }
        }

        private void GravarTela()
        {
            if (captureThread != null)
            {
                if (captureThread.IsAlive)
                {
                    notifyIcon1.ShowBalloonTip(2000, "Atenção", $"Aguarde finalizar o processamento do GIF anterior para iniciar um novo", ToolTipIcon.Info);
                    return;
                }
            }

            _gravando = true;
            capturedFrames.Clear();
            int duration = 5000; // Duração da gravação (5 segundos)
            int frameRate = 10;  // 10 FPS
            int interval = 1000 / frameRate;



            notifyIcon1.ShowBalloonTip(2000, "Gravando", $"Gravação iniciada...", ToolTipIcon.Info);

            DateTime startTime = DateTime.Now;
            //while ((DateTime.Now - startTime).TotalMilliseconds < duration)

            captureThread = new Thread(() =>
            {
                while (_gravando)
                {
                    CaptureFrame();
                    Thread.Sleep(interval);
                }
            });

            captureThread.IsBackground = true;
            captureThread.Start();


        }

        private void PararGravacao()
        {
            _gravando = false;

            captureThread?.Join();
            notifyIcon1.ShowBalloonTip(2000, "Quase lá!", $"Criando seu GIF...", ToolTipIcon.Info);


            CreateGif();

        }
        private void CaptureFrame()
        {

            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;

            string frame = $"frame_{Guid.NewGuid()}.png";

            var directoryPath = Path.Combine(ObterPastaCapturas(), "temp");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string framePath = Path.Combine(directoryPath, $"frame_{Guid.NewGuid()}.png");

            //using (Bitmap bitmap = new Bitmap(800, 600))
            using (Bitmap bitmap = new Bitmap(screenBounds.Width, screenBounds.Height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                bitmap.Save(framePath, ImageFormat.Png);
            }

            capturedFrames.Add(framePath);
        }

        private void CreateGif()
        {
            if (captureThread != null)
            {
                if (captureThread.IsAlive)
                {
                    notifyIcon1.ShowBalloonTip(2000, "Atenção", $"Aguarde finalizar o processamento do GIF anterior para iniciar um novo", ToolTipIcon.Info);
                    return;
                }
            }


            captureThread = new Thread(() =>
            {
                using (MagickImageCollection collection = new MagickImageCollection())
                {
                    foreach (var framePath in capturedFrames)
                    {
                        using (var img = new MagickImage(framePath))
                        {
                            img.AnimationDelay = 12; // Tempo de exibição do frame
                            img.Quantize(new QuantizeSettings { Colors = 64 }); //Reduz brilho por performance
                            collection.Add(img.Clone());
                        }
                    }




                    //collection.Optimize();
                    //collection.OptimizeTransparency();

                    var caminhoCompleto = Path.Combine(ObterPastaCapturas(), string.Format(outputGifPath, DateTime.Now.ToString("ddMMyyyy HHmmss")));


                    collection.Write(caminhoCompleto);


                    notifyIcon1.ShowBalloonTip(2000, "Pronto!", $"GIF salvo em: {ObterPastaCapturas()}", ToolTipIcon.Info);
                }

                // Remover os frames temporários
                foreach (var file in capturedFrames)
                {
                    System.IO.File.Delete(file);
                }


            });
            captureThread.IsBackground = true;
            captureThread.Start();



        }

        private void CriarOuVerificarPasta()
        {
            string meusDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string nomeDaPasta = "Capturas GifScreen";

            string caminhoCompleto = Path.Combine(meusDocumentos, nomeDaPasta);

            if (!Directory.Exists(caminhoCompleto))
                Directory.CreateDirectory(caminhoCompleto);
        }

        private string ObterPastaCapturas()
        {
            string meusDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string nomeDaPasta = "Capturas GifScreen";

            string caminhoCompleto = Path.Combine(meusDocumentos, nomeDaPasta);

            return caminhoCompleto;
        }


        private void SetupNotifyIcon()
        {
            // Cria o menu de contexto com um item "Fechar"
            contextMenu = new ContextMenuStrip();
            var fecharItem = new ToolStripMenuItem("Sair");
            fecharItem.Click += (s, e) =>
            {
                Application.Exit();
                Process.GetCurrentProcess().Kill();
            };

            contextMenu.Items.Add(fecharItem);

            notifyIcon1.Icon = new Icon(Path.Combine(AppContext.BaseDirectory, "Resources", "icon GifScreen.ico"));
            notifyIcon1.ContextMenuStrip = contextMenu;
            notifyIcon1.Visible = true;

            // Opcional: clique duplo no ícone
            notifyIcon1.DoubleClick += (s, e) =>
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            };


            notifyIcon1.BalloonTipClicked -= NotifyIcon1_BalloonTipClicked;
            notifyIcon1.BalloonTipClicked += NotifyIcon1_BalloonTipClicked;

        }



        private void btSalvar_Click(object sender, EventArgs e)
        {

            if (currentHotkeyGravacao != Keys.None)
                RegistrarAtalho(currentHotkeyGravacao, HOTKEY_ID_GRAVACAO);


            if (currentHotkeyCaptura != Keys.None)
                RegistrarAtalho(currentHotkeyCaptura, HOTKEY_ID);


            if (_configuracoesSistema == null)
                _configuracoesSistema = ConfiguracoesSistema.ObterConfiguracoesDoSistema();

            _configuracoesSistema.IniciarComSistema = chkIniciarComWindows.Checked;
            _configuracoesSistema.TeclasGravacao = currentHotkeyGravacao;
            _configuracoesSistema.TeclasCaptura = currentHotkeyCaptura;

            _configuracoesSistema.Salvar();
        }

        private void RegistrarAtalho(Keys keyData, int hotkeyId)
        {

            UnregisterHotKey(this.Handle, hotkeyId);

            // Extrai tecla e modificadores
            uint modifiers = 0;
            Keys keyCode = keyData & Keys.KeyCode;

            if (keyData.HasFlag(Keys.Control)) modifiers |= MOD_CONTROL;
            if (keyData.HasFlag(Keys.Shift)) modifiers |= MOD_SHIFT;
            if (keyData.HasFlag(Keys.Alt)) modifiers |= MOD_ALT;

            var sucesso = RegisterHotKey(this.Handle, hotkeyId, modifiers, keyCode);

            if (!sucesso)
            {
                MessageBox.Show($"Erro ao registrar hotkey: {keyData}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void NotifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (Directory.Exists(ObterPastaCapturas()))
            {
                try
                {


                    var psi = new ProcessStartInfo
                    {
                        FileName = ObterPastaCapturas(),
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch (Exception ex) { }
            }
        }

        private void txtAtalhoCaptura_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true; // Impede que a tecla apareça no TextBox

            List<string> keys = new List<string>();

            if (e.Control) keys.Add("Ctrl");
            if (e.Alt) keys.Add("Alt");
            if (e.Shift) keys.Add("Shift");

            // Ignora caso só tenha modificadores
            if (e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.Menu)
            {
                keys.Add(e.KeyCode.ToString());
                txtAtalhoCaptura.Text = string.Join(" + ", keys);

                // Armazena a combinação
                currentHotkeyCaptura = e.KeyData;
            }
        }

        private void txtAtalhoGravacao_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true; // Impede que a tecla apareça no TextBox

            List<string> keys = new List<string>();

            if (e.Control) keys.Add("Ctrl");
            if (e.Alt) keys.Add("Alt");
            if (e.Shift) keys.Add("Shift");

            // Ignora caso só tenha modificadores
            if (e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.Menu)
            {
                keys.Add(e.KeyCode.ToString());
                txtAtalhoGravacao.Text = string.Join(" + ", keys);

                // Armazena a combinação
                currentHotkeyGravacao = e.KeyData;
            }
        }
    }
}
