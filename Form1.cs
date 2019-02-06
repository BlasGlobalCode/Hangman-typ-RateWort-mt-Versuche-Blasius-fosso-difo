using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuessTheWord
{
    //mit delegate kann ich die Methode (Z:15) zusammenfassen
    public delegate void CheckLetter(string woertern);
    
    public partial class Form1 : Form
    {
        string Namegenerator;
        string Spielername;
        //mit "cons" werden Werten eingefügt, die zur Kompilierzeit bekannt sind und sich während der Lebensdauer nicht verändern
        const int  MAX_Anzahl_Vom_Leben = 5;
        //Ein Ereignis, das jedes Mal aufgerufen wird, wenn eine Buchstabe erraten wird
        event CheckLetter ChkLtr;

        string eingabe;
        string GefehltenBuchstaben = "";

        //das Wort, der zu finden ist
        string wortzufinden = "";

        //Aktueller position der gefundenen Buchstaben im Wort
        string anzuzeigendesWort = "";

        //Zeichenarray von Woertern
        char[] wortzufindenArray;
        int[] wortzufindenbuchstabePosition;
        bool buchstabegefunden = false;

        //Zufallszahlengenerator-Klasse, um ein Wort zufällig aus der Wortliste zu erhalten
        Random rndm = new Random(0);

        //Auflistung Von Woertern
        List<string> Woerterliste = new List<string>();

        // Eine Liste der Indexpositionen, um zu verfolgen, welches Wort bereits gespielt wird
        List<int> wordsIndexAlreadyPlayed = new List<int>();

        int AnzahlgefehlteBuchstaben = 0;

        public Form1()
        {
            

            InitializeComponent();
            

            //Veranstaltung
            this.ChkLtr += new CheckLetter(Form1_ChkLtr);

            //Erstellt eine Wortliste
            WoerterlisteErstellen();

            //Start ein Neues Spiel
            neueStarten();
        }

        private void WoerterlisteErstellen()
        {

            //codierte Namenliste mit 19 Namen aus der Klasse
            Woerterliste.Add("Anton");
            Woerterliste.Add("Noah");
            Woerterliste.Add("Gideon");
            Woerterliste.Add("Jessica");
            Woerterliste.Add("Luca");
            Woerterliste.Add("Christoph");
            Woerterliste.Add("Blasius");
            Woerterliste.Add("AnnaLena");
            Woerterliste.Add("Nico");
            Woerterliste.Add("Esfrgdsfb");
            Woerterliste.Add("sgsdg");
            Woerterliste.Add("Erztrts");
            Woerterliste.Add("Lfgnb");
            Woerterliste.Add("Pbtrs");
            Woerterliste.Add("Dngtrz");
            Woerterliste.Add("h");
            Woerterliste.Add("zt");
            Woerterliste.Add("Mg");
            Woerterliste.Add("f");
            Woerterliste.Add("r");
            Woerterliste.Add("w");
        }

        private string GetNewWordFromPool()
        {
            bool neuswort = false;

            //Standardwort

            string Standard = "HANGMAN";
            
            try
            {
                while (neuswort == false)
                {
                    //-------------Wort zufällig aus Wörtern sammeln ------------
                    int index = rndm.Next();

                    //Um die Zahl in dem WordList-Bereich darzustellen
                    index = index % Woerterliste.Count;

                    //----------- keine wiederholte Woerten nehmen --------------------

                    //Hier habe ich den Lambda-Ausdruck verwendet, 
                    //um zu prüfen, ob das Wort bereits gespielt wurde oder nicht

                    //Ein Lambda-Ausdruck ist eine anonyme Funktion , 
                    //mit der Typen für Delegaten oder die Ausdrucksbaumstruktur erstellt werden können

                    if (!wordsIndexAlreadyPlayed.Exists(e => e == index))
                    {
                        Standard = Woerterliste[index];
                        wordsIndexAlreadyPlayed.Add(index);
                        neuswort = true;
                        break;
                    }
                    else
                    {
                        neuswort = false;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            return Standard.ToUpper();
        }

        private void neueStarten()
        {
            try
            {
                wortzufinden = GetNewWordFromPool();
                wortzufinden = wortzufinden.ToUpper();
                wortzufindenArray = wortzufinden.ToCharArray();

                wortzufindenbuchstabePosition = new int[wortzufinden.Length];

                //Zähler und Variablen zurücksetzen
                eingabe = "";
                anzuzeigendesWort = "";
                for (int i = 0; i < wortzufinden.Length; i++)
                {
                    anzuzeigendesWort += "-";
                }

                GefehltenBuchstaben = "";
                AnzahlgefehlteBuchstaben = 0;

                txteing.Text = anzuzeigendesWort.ToUpper();
                lblGefehlteWoertern.Text = GefehltenBuchstaben;
                lblgefehlteCounter.Text = MAX_Anzahl_Vom_Leben.ToString();
                //mit "Application.DoEvents();" wird diese Methode  von Anfang bis zum Ende ausgefuehrt
                Application.DoEvents();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        //Gestionnaire d'événements
        private void Form1_ChkLtr(string GegebeneBuchstaben)
        {
            try
            {                            
                if (AnzahlgefehlteBuchstaben < MAX_Anzahl_Vom_Leben)
                {

                    buchstabegefunden = false;
                    for (int i = 0; i < wortzufindenArray.Length; i++)
                    {
                        //buchstabegefunden = false;
                        if (GegebeneBuchstaben[0] == wortzufindenArray[i])
                        {
                            wortzufindenbuchstabePosition[i] = 1;
                            buchstabegefunden = true;
                        }
                    }
                    if (buchstabegefunden == false)
                    {

                        GefehltenBuchstaben += GegebeneBuchstaben + ", ";
                        AnzahlgefehlteBuchstaben++;
                        lblgefehlteCounter.Text = (MAX_Anzahl_Vom_Leben - AnzahlgefehlteBuchstaben).ToString();
                    }

                    anzuzeigendesWort = "";
                    for (int i = 0; i < wortzufindenArray.Length; i++)
                    {
                        if (wortzufindenbuchstabePosition[i] == 1)
                        {
                            anzuzeigendesWort += wortzufindenArray[i].ToString();
                        }
                        else
                        {
                            anzuzeigendesWort += "-";
                        }                    }

                    txteing.Text = anzuzeigendesWort.ToUpper();
                    lblGefehlteWoertern.Text = GefehltenBuchstaben;
                    if (wortzufindenbuchstabePosition.All(e => e == 1))
                    {
                        MessageBox.Show("Glückwunsch " + Spielername +"!"+" Du hast das Wort.", "Ergebnis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        neueStarten();
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, "+ Spielername + "  Du hast verloren und es wäre schön wenn du wieder spielen würdest " + "\nDas Richtige Wort war: " + wortzufinden, "Ergebnis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    neueStarten();
                }
                Application.DoEvents();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        // das ist eine Region, um die Einzelnen Buchstaben zu kontrollieren und weniger code
        #region Getting Alphabets
        private void button_A_Click(object sender, EventArgs e)
        {
            eingabe = "A"; 
            
            ChkLtr(eingabe);
        }

        private void button_B_Click(object sender, EventArgs e)
        {
            eingabe = "B";
            
            ChkLtr(eingabe);
        }

        private void button_C_Click(object sender, EventArgs e)
        {
            eingabe = "C";
            
            ChkLtr(eingabe);
        }

        private void button_D_Click(object sender, EventArgs e)
        {
            eingabe = "D";
            
            ChkLtr(eingabe);
        }

        private void button_E_Click(object sender, EventArgs e)
        {
            eingabe = "E";
            
            ChkLtr(eingabe);
        }

        private void button_F_Click(object sender, EventArgs e)
        {
            eingabe = "F";
            
            ChkLtr(eingabe);
        }

        private void button_G_Click(object sender, EventArgs e)
        {
            eingabe = "G";
            
            ChkLtr(eingabe);
        }

        private void button_H_Click(object sender, EventArgs e)
        {
            eingabe = "H";
            
            ChkLtr(eingabe);
        }

        private void button_I_Click(object sender, EventArgs e)
        {
            eingabe = "I";
            
            ChkLtr(eingabe);
        }

        private void button_J_Click(object sender, EventArgs e)
        {
            eingabe = "J";
            
            ChkLtr(eingabe);
        }

        private void button_K_Click(object sender, EventArgs e)
        {
            eingabe = "K";
            
            ChkLtr(eingabe);
        }

        private void button_L_Click(object sender, EventArgs e)
        {
            eingabe = "L";
            
            ChkLtr(eingabe);
        }

        private void button_M_Click(object sender, EventArgs e)
        {
            eingabe = "M";
            
            ChkLtr(eingabe);
        }

        private void button_N_Click(object sender, EventArgs e)
        {
            eingabe = "N";
            
            ChkLtr(eingabe);
        }

        private void button_O_Click(object sender, EventArgs e)
        {
            eingabe = "O";
            
            ChkLtr(eingabe);
        }

        private void button_P_Click(object sender, EventArgs e)
        {
            eingabe = "P";
            
            ChkLtr(eingabe);
        }

        private void button_Q_Click(object sender, EventArgs e)
        {
            eingabe = "Q";
            
            ChkLtr(eingabe);
        }

        private void button_R_Click(object sender, EventArgs e)
        {
            eingabe = "R";
            
            ChkLtr(eingabe);
        }

        private void button_S_Click(object sender, EventArgs e)
        {
            eingabe = "S";
            
            ChkLtr(eingabe);
        }

        private void button_T_Click(object sender, EventArgs e)
        {
            eingabe = "T";
            
            ChkLtr(eingabe);
        }

        private void button_U_Click(object sender, EventArgs e)
        {
            eingabe = "U";
            
            ChkLtr(eingabe);
        }
        
        private void button_V_Click(object sender, EventArgs e)
        {
            eingabe = "V";
            
            ChkLtr(eingabe);
        }

        private void button_W_Click(object sender, EventArgs e)
        {
            eingabe = "W";
            
            ChkLtr(eingabe);
        }

        private void button_X_Click(object sender, EventArgs e)
        {
            eingabe = "X";
            
            ChkLtr(eingabe);
        }

        private void button_Y_Click(object sender, EventArgs e)
        {
            eingabe = "Y";
            
            ChkLtr(eingabe);
        }

        private void button_Z_Click(object sender, EventArgs e)
        {
            eingabe = "Z";
            
            ChkLtr(eingabe);
        }
        //mit der region wird ein Methode ganz bis zum Ende ausgeführt

#endregion

        private void button_LoadNewWord_Click(object sender, EventArgs e)
        {
            btnExit.BackColor = Color.SpringGreen;
            btnname.Visible = false;
            panel2.Visible = true;
            txtSpielername.Text = "";
            neueStarten();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {

            btnExit.BackColor = Color.White;
            Spielername = txtSpielername.Text.ToString();
            if(Spielername == "")
            {
                
                panel2.Visible = false;
                btnname.Text = "Anonym".ToString();
                btnname.Visible = true;
            }
            else
            {
                panel2.Visible = false;
                btnname.Text = Spielername.ToString();
                btnname.Visible = true;
            }
        }

        private void btnname_Click(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            const String message = "möchtest du wirklich das Spiel verlassen ? ";
            const string capteur = "Spiel beenden";
            var ergeb = MessageBox.Show(message, capteur, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ergeb == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
