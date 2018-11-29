using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Projet_IA_Quentin_Juliette_Laurie
{
    class Node4 : GenericNode
    {
        public int x; // Lignes
        public int y; // Colonnes
        //les booléens sont vrais si le point est un successeur d'un ou plusieurs checkpoints
        public bool c1;
        public bool c2;
        public bool c3;
        public bool c4;

        public Node4(int x, int y, bool c1, bool c2, bool c3, bool c4)
        {
            this.x = x;
            this.y = y;
            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c3;
            this.c4 = c4;
        }
        public double CalculeHCostLocal(int[] cell)
        {

            double cout = 0;
            int ecartX = Math.Abs(cell[0] - x);
            int ecartY = Math.Abs(cell[0] - y);
            int min = Math.Min(ecartX, ecartY);
            cout += Math.Sqrt(2) * min;
            ecartX -= min;
            ecartY -= min;
            cout += ecartX + ecartY; // sachant que un des deux écarts = 0
            if (!EndState() && !(x == Formulaire.check1[0] && y == Formulaire.check1[1])
                    && !(x == Formulaire.check2[0] && y == Formulaire.check2[1])
                    && !(x == Formulaire.check3[0] && y == Formulaire.check3[1])
                    && !(x == Formulaire.check4[0] && y == Formulaire.check4[1])
                    && !(x == Formulaire.celluleDepartCheck[0] && y == Formulaire.celluleDepartCheck[1]))
                Formulaire.matrice[x, y] = 6;
            return cout;
        }

        public override double CalculeHCost()
        {
            double cout = 0;
            //soit on rentre parce qu'on a déjà visité tous les points
            if (c1 && c2 && c3 && c4)
            {
                cout = CalculeHCostLocal(Formulaire.celluleDepartCheck);
            }

            //soit il faut au minimum aller au checkpoint le plus loin et rentrer
            else
            {
                List<bool> listeCheck = new List<bool> { c1, c2, c3, c4 };
                List<int[]> mesCheck = new List<int[]> { Formulaire.check1, Formulaire.check2, Formulaire.check3, Formulaire.check4 };
                List<double> listeCout = new List<double> { };
                for (int i = 3; i >= 0; i--)
                {
                    if (listeCheck[i])
                    {
                        mesCheck.RemoveAt(i);
                    }
                    else
                    {
                        double coutLocal = CalculeHCostLocal(mesCheck[i]);
                        listeCout.Add(coutLocal);
                    }
                }
                //demicoup = valeur du cout intermédiaire
                double demiCout = listeCout.Max(); // Renvoie valeur du demiCout
                int indice = 0;
                for (int i = 0; i < listeCout.Count(); i++)
                {
                    if (listeCout[i] == demiCout) { indice = i; } // Récupère l'indice de la liste où la valeur est maximale
                }
                // Puis ajouter le cout vers le point initial (= coutRetour)
                int sauvX = x;
                int sauvY = y;
                x = mesCheck[indice][0];
                y = mesCheck[indice][1];
                double coutRetour = CalculeHCostLocal(Formulaire.celluleDepartCheck);
                x = sauvX;
                y = sauvY;
                cout = demiCout + coutRetour;
            }
            return (cout);
        }

        //la case est finale ?
        public override bool EndState()
        {
            bool testPosition = this.x == Formulaire.celluleDepartCheck[0] && this.y == Formulaire.celluleDepartCheck[1];
            return (testPosition && c1 && c2 && c3 && c4);
        }

        //cout pour aller à la case suivante (racine(2) si diagonale)
        public override double GetArcCost(GenericNode N)
        {
            double distance = Math.Sqrt(2);
            Node4 N2 = (Node4)N;
            if ((N2.x == this.x + 1 || N2.x == this.x - 1) && N2.y == this.y)
            {
                distance = 1;
            }
            if ((N2.y == this.y + 1 || N2.y == this.y - 1) && N2.x == this.x)
            {
                distance = 1;
            }
            return distance;
        }

        public override List<GenericNode> GetListSucc()
        {

            List<GenericNode> lsucc = new List<GenericNode>();

            // Regarder à droite
            if (y + 1 < 20 && Formulaire.matrice[x, y + 1] != 0)
            {
                lsucc.Add(new Node4(x, y + 1, c1, c2, c3, c4));
            }
            // Regarder à Gauche
            if (y - 1 > -1 && Formulaire.matrice[x, y - 1] != 0)
            {
                lsucc.Add(new Node4(x, y - 1, c1, c2, c3, c4));
            }
            // Regarder en haut
            if (x - 1 > -1 && Formulaire.matrice[x - 1, y] != 0)
            {
                lsucc.Add(new Node4(x - 1, y, c1, c2, c3, c4));
            }
            // Regarder en Bas
            if (x + 1 < 20 && Formulaire.matrice[x + 1, y] != 0)
            {
                lsucc.Add(new Node4(x + 1, y, c1, c2, c3, c4));
            }
            // Regarder en diagonale haut gauche
            if (x - 1 > -1 && y - 1 > -1 && Formulaire.matrice[x - 1, y - 1] != 0)
            {
                lsucc.Add(new Node4(x - 1, y - 1, c1, c2, c3, c4));
            }
            // Regarder en diagonale haut droit
            if (x - 1 > -1 && y + 1 < 20 && Formulaire.matrice[x - 1, y + 1] != 0)
            {
                lsucc.Add(new Node4(x - 1, y + 1, c1, c2, c3, c4));
            }
            // Regarder en diagonale Bas gauche
            if (x + 1 < 20 && y - 1 > -1 && Formulaire.matrice[x + 1, y - 1] != 0)
            {
                lsucc.Add(new Node4(x + 1, y - 1, c1, c2, c3, c4));
            }
            // Regarder en diagonale Bas droit
            if (x + 1 < 20 && y + 1 < 20 && Formulaire.matrice[x + 1, y + 1] != 0)
            {
                lsucc.Add(new Node4(x + 1, y + 1, c1, c2, c3, c4));
            }
            //Si le point est un checkpoint, on passe à true ce checkpoint
            //On utilise des if et non des else if car on peut faire entre 1 et 4 checkpoints en mettant 2 fois le même point
            foreach (Node4 elmt in lsucc)
            {
                if (elmt.x == Formulaire.check1[0] && elmt.y == Formulaire.check1[1])
                {
                    elmt.c1 = true;
                }
                if (elmt.x == Formulaire.check2[0] && elmt.y == Formulaire.check2[1])
                {
                    elmt.c2 = true;
                }
                if (elmt.x == Formulaire.check3[0] && elmt.y == Formulaire.check3[1])
                {
                    elmt.c3 = true;
                }
                if (elmt.x == Formulaire.check4[0] && elmt.y == Formulaire.check4[1])
                {
                    elmt.c4 = true;
                }
            }

            return lsucc;
        }

        public override bool IsEqual(GenericNode N)
        {
            Node4 N2 = (Node4)N;
            return (this.x == N2.x && this.y == N2.y && this.c1 == N2.c1 && this.c2 == N2.c2 && this.c3 == N2.c3 && this.c4 == N2.c4);
        }
        
        public override string ToString()
        {
            return (x + 1) + " " + (y + 1);
        }
    }
}
