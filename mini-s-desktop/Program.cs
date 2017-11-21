// Projet S5 - Les S de Sherbrooke, tous droits réservés
// Fichier : Program.cs
// Auteur : Hugo Therrien
// Date : 21 novembre 2017
//
// Fichier contenant le main
// 


using System.Threading;

namespace mini_s_desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPortManager.Instance.Open("COM1");
            SourisEtCommandes souris = new SourisEtCommandes();

            SerialPortManager.Instance.SendString("Projet S5\n");

            for (;;)
            {
                ;
            }

        }
    }
}
