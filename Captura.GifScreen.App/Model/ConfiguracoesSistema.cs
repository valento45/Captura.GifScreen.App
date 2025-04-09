using Captura.GifScreen.App.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captura.GifScreen.App.Model
{
    public class ConfiguracoesSistema
    {

        public Keys TeclasCaptura { get; set; }
        public Keys TeclasGravacao { get; set; }
        public bool IniciarComSistema { get; set; }
        public static readonly string nomeApp = "GifScreenApp";
        public static readonly string caminho = Application.ExecutablePath;

        public ConfiguracoesSistema()
        {
            TeclasCaptura = Keys.Control | Keys.Alt | Keys.P;
            TeclasGravacao = Keys.Control | Keys.Alt | Keys.S;
            IniciarComSistema = true;
        }



        public bool Salvar()
        {
            try
            {
                if (!IniciarComSistema)
                {
                    if (AutoStartHelper.EstaRegistrado(nomeApp))
                        AutoStartHelper.RemoverAutoInicio(nomeApp);
                }
                else
                {
                    AutoStartHelper.RegistrarAutoInicio(nomeApp, caminho);
                }


                string json = JsonConvert.SerializeObject(this);
                File.WriteAllText(ObterCaminhoConfiguracao(), json);
                MessageBox.Show("Configurações foram salvas com sucesso!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar suas configurações, por favor execute como Administrador e tente novamente.", "OPS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }



        public static ConfiguracoesSistema ObterConfiguracoesDoSistema()
        {
            if (!File.Exists(ObterCaminhoConfiguracao()))
                return new ConfiguracoesSistema();

            string json = File.ReadAllText(ObterCaminhoConfiguracao());

            if (!string.IsNullOrEmpty(json))
                return JsonConvert.DeserializeObject<ConfiguracoesSistema>(json);
            else
                return new ConfiguracoesSistema();

        }

        public static string ConverterKeysParaTexto(Keys keys)
        {
            string texto = "";

            if (keys.HasFlag(Keys.Control))
                texto += "Ctrl + ";

            if (keys.HasFlag(Keys.Alt))
                texto += "Alt + ";

            if (keys.HasFlag(Keys.Shift))
                texto += "Shift + ";

            // Remove modificadores
            Keys keySemModificadores = keys & ~Keys.Control & ~Keys.Alt & ~Keys.Shift;

            // Adiciona a tecla principal
            texto += keySemModificadores.ToString();

            return texto;
        }


        private static string ObterCaminhoConfiguracao()
        {
            var path = Path.Combine(Application.StartupPath, "configuration.json");
            return path;
        }
    }
}
