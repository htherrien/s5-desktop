// Projet S5 - Les S de Sherbrooke, tous droits réservés
// Fichier : SourisEtCommandes.cs
// Auteur : Hugo Therrien
// Date : 21 novembre 2017
//
// Fichier classe de la souris et d'envoi de commandes Windows
// 

using System;

namespace mini_s_desktop
{
    public class SourisEtCommandes
    {
        public SourisEtCommandes()
        {
            SerialPortManager.Instance.OnDataReceived += Handler_ReceptionSerie;
        }
        private void Handler_ReceptionSerie(object envoyeur, string donnees)
        {
            Console.WriteLine("Reçu: {0}", donnees);
        }
    }
}
