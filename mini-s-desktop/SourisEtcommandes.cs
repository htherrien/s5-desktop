// Projet S5 - Les S de Sherbrooke, tous droits réservés
// Fichier : SourisEtCommandes.cs
// Auteur : Hugo Therrien
// Date : 21 novembre 2017
//
// Fichier classe de la souris et d'envoi de commandes Windows
// 

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;


namespace mini_s_desktop
{
    public class SourisEtCommandes
    {
        // TODO: Try to find a way to implement the command parsing using this enum
        public enum Mouvements
        {
            [Description("max")]
            MOUVEMENT_MAXIMISER = 0,
            [Description("min")]
            MOUVEMENT_MINIMISER = 1,
            [Description("ferm")]
            MOUVEMENT_FERMER = 2,
            NB_MOUVEMENTS,
            AUCUN_MOUVEMENT
        }

        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        static private extern uint SendInput(
        uint nInputs,
        INPUT[] pInputs,
        int cbSize);

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow", SetLastError = true)]
        static private extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static private extern bool ShowWindow(
        IntPtr hWnd,
        int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "PostMessage", SetLastError = true)]
        static private extern bool PostMessage(
        IntPtr   hWnd,
        uint   Msg,
        IntPtr  wParam,
        IntPtr  lParam);

        private const uint WM_CLOSE = 0x0010;

        public enum N_CMD_SHOW
        {

            SW_FORCEMINIMIZE = 11,
            SW_HIDE = 0,
            SW_MAXIMIZE = 3,
            SW_MINIMIZE = 6,
            SW_RESTORE = 9,
            SW_SHOW = 5,
            SW_SHOWDEFAULT = 10,
            SW_SHOWMAXIMIZED = 3,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOWNORMAL = 1
        }

        public enum TYPE_ENTREE
        {
            SOURIS = 0,
            CLAVIER = 1,
            MATERIEL = 2,
        }

        public enum EVENEMENTS_SOURIS
        {
            MOUSEEVENTF_ABSOLUTE = 0x8000,
            MOUSEEVENTF_HWHEEL = 0x01000,
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        };

        public SourisEtCommandes()
        {
            SerialPortManager.Instance.OnDataReceived += HandlerReceptionSerie;
        }

        private void HandlerReceptionSerie(object envoyeur, string donnees)
        {
            int x = 0, y = 0;
            char[] separateurs = {' ', ';'};
            foreach (string message in donnees.Split(separateurs))
            {
                string[] mot = message.Split(':');

                switch (mot[0])
                {
                    case "x":
                        x = int.Parse(mot[1]);
                        break;
                    case "y":
                        y = int.Parse(mot[1]);
                        break;
                    case "max":
                        MaximiserFenetre();
                        break;
                    case "min":
                        MinimiserFenetre();
                        break;
                    case "ferm":
                        FermerFenetre();
                        break;
                    case "cg":
                        ClicGauche();
                        break;
                    case "et":
                        EnregistrementTermine();
                        break;
                    default:
                        break;
                }
            }
            BougerSouris(x, y);
        }

        private void MinimiserFenetre()
        {
            ShowWindow(GetForegroundWindow(), (int)N_CMD_SHOW.SW_MINIMIZE);
        }

        private void MaximiserFenetre()
        {
            ShowWindow(GetForegroundWindow(), (int)N_CMD_SHOW.SW_MAXIMIZE);
        }

        private void FermerFenetre()
        {
            PostMessage(GetForegroundWindow(), WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        private void EnregistrementTermine()
        {
            Console.Write("Enregistrement terminé\n");
        }

        public static void ClicGauche()
        {
            INPUT commande = new INPUT();
            commande.type = (int)TYPE_ENTREE.SOURIS;
            commande.mi.dwFlags = (int)(EVENEMENTS_SOURIS.MOUSEEVENTF_LEFTDOWN | EVENEMENTS_SOURIS.MOUSEEVENTF_LEFTUP);
            commande.mi.dx = 0;
            commande.mi.dy = 0;
            commande.mi.mouseData = 0;

            INPUT[] commandeTab = { commande };
            SendInput(1, commandeTab, Marshal.SizeOf(commande));
        }

        private void BougerSouris(int x, int y)
        {
            INPUT commande = new INPUT();
            commande.type = (int)TYPE_ENTREE.SOURIS;
            commande.mi.dwFlags = (int)(EVENEMENTS_SOURIS.MOUSEEVENTF_MOVE);
            commande.mi.dx = x;
            commande.mi.dy = y;
            commande.mi.mouseData = 0;

            INPUT[] commandeTab = { commande };
            SendInput(1, commandeTab, Marshal.SizeOf(commande));
        }
    }
}
