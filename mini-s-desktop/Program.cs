// Projet S5 - Les S de Sherbrooke, tous droits réservés
// Fichier : Program.cs
// Auteur : Hugo Therrien
// Date : 21 novembre 2017
//
// Fichier contenant le main
// 

using System;
using System.Threading;
using System.ComponentModel;
public static class EnumHelper
{
    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
}

namespace mini_s_desktop
{
    class Program
    {
        static void Main(string[] args)
        {

            SerialPortManager.Instance.Open("COM1");
            SourisEtCommandes souris = new SourisEtCommandes();

            Console.Write("Veuillez choisir le mouvement à enregistrer:\n0. maximiser\n1. minimiser\n2. fermer\n");

            for (;;)
            {
                int mouvement;
                                string entree = Console.ReadLine();
                if (!int.TryParse(entree, out mouvement))
                {
                    Console.Write("La commande \"" + entree + "\" est invalide\n");
                }
                else if(mouvement >= 0 && mouvement <= 2)
                {
                    char[] message = { Convert.ToChar(mouvement) };
                    SerialPortManager.Instance.SendChars(message, 1);
                    Console.Write("Veuillez appuyer sur le bouton pouce sur le gant pour démarrer l'enregistrement\n", mouvement);
                }
                SourisEtCommandes.ClicGauche();
            }

        }
    }
}
