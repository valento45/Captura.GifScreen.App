using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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



            using (var rk1 = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                rk1?.SetValue(nomeAplicativo, $"\"{caminhoExe}\"");
            }
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

        public static void CriarTarefaAgendada(string caminhoExe)
        {
            string taskName = "GifScreenAppAutoStart";
            //string exePath = "\"C:\\Program Files\\GifScreenApp\\GifScreenApp.exe\"";
            string exePath = caminhoExe;
            string arguments = "/silent";

            var psi = new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = $"/Create /TN \"{taskName}\" /TR {exePath} {arguments} /SC ONLOGON /RL HIGHEST /F",
                UseShellExecute = true,
                Verb = "runas" // Isso força a execução como admin (vai abrir o UAC)
            };

            try
            {
                Process.Start(psi);
                Console.WriteLine("Tarefa agendada criada com sucesso!");
            }
            catch
            {
                Console.WriteLine("O usuário cancelou o UAC ou ocorreu um erro.");
            }
        }

        public static void ExcluirTarefaAgendada()
        {
            string taskName = "GifScreenAppAutoStart";

            var psi = new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = $"/Delete /TN \"{taskName}\" /F", // /F força a exclusão sem confirmação
                UseShellExecute = true,
                Verb = "runas" // Executa como administrador (vai abrir o UAC)
            };

            try
            {
                Process.Start(psi);
                Console.WriteLine("Tarefa agendada excluída com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir tarefa agendada: {ex.Message}");
            }
        }
    }
}
