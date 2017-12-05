// Projet S5 - Les S de Sherbrooke, tous droits réservés
// Fichier : Program.cs
// Auteur : Hugo Therrien
// Date : 21 novembre 2017
//
// Fichier contenant le main
// 

using System;
using System.Threading;

namespace mini_s_desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPortManager.Instance.Open("COM1");
            SourisEtCommandes souris = new SourisEtCommandes();

            for (;;)
            {
                Console.Write("Veuillez choisir le mouvement à enregistrer (1-2):");
                int mouvement = int.Parse(Console.ReadLine());
                if(mouvement >= 1 && mouvement <= 2)
                {
                    char[] message = { Convert.ToChar(mouvement) };
                    SerialPortManager.Instance.SendChars(message, 1);
                    Console.Write("Le mouvement {0} est en cours d'enregistrement\n", mouvement);
                    Thread.Sleep(1000*64/40);// Sleep for the recroding period
                    Console.Write("Le mouvement {0} est enregistré\n", mouvement);
                }           
            }

        }
    }
}
