using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captura.GifScreen.App.Configuration
{
    public static class AutoStartHelper
    {
        public static void RegistrarAutoInicio(string nomeAplicativo, string caminhoExe)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            rk.SetValue(nomeAplicativo, $"\"{caminhoExe}\"");
        }

        public static void RemoverAutoInicio(string nomeAplicativo)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (rk.GetValue(nomeAplicativo) != null)
            {
                rk.DeleteValue(nomeAplicativo, false);
            }
        }

        public static bool EstaRegistrado(string nomeAplicativo)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);
            return rk.GetValue(nomeAplicativo) != null;
        }
    }
}
