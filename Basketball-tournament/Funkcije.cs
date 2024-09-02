using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basketball_tournament
{
    public class Funkcije
    {
        public static Grupe UcitajJson()
        {
            String putanja = "JSon\\groups.json";
            Grupe grupe = new Grupe();
            if (File.Exists(putanja))
            {
                try
                {
                    String jsonSadrzaj = File.ReadAllText(putanja);
                    grupe = JsonSerializer.Deserialize<Grupe>(jsonSadrzaj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Greska prilikom otvaranja datoteke: " + ex.Message.ToString());
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Datoteka nedostaje");
                Environment.Exit(0);
            }

            return grupe;
        }


        private static double IzracunavanjeVerovatnocePobede(int rankingA, int rankingB)
        {
            double razlikaFIBA = rankingA - rankingB;

            return 1.0 / (1.0 + Math.Exp(razlikaFIBA / 10.0));
        }


        public static RezultatMeca SimulacijaMeca(Tim timA, Tim timB)
        {
            double verovatnocaA = IzracunavanjeVerovatnocePobede(timA.FIBARanking, timB.FIBARanking);
            double verovatnocaB = 1.0 - verovatnocaA;

            var rnd = new Random();
            bool timaApobeda = rnd.NextDouble() < verovatnocaA;

            int timARez = rnd.Next(70, 110);
            int timBRez = rnd.Next(70, 110);

            while (true)
            {
                if (timARez != timBRez)
                {
                    break;
                }
                else
                {
                    timARez = rnd.Next(70, 110);
                    timBRez = rnd.Next(70, 110);
                }
            }

            if (timARez < timBRez && timaApobeda)
            {
                int temp = timARez;
                timARez = timBRez;
                timBRez = temp;
            }
            else if (!timaApobeda)
            {
                int temp = timARez;
                timARez = timBRez;
                timBRez = temp;
            }

            return new RezultatMeca
            {
                Pobednik = timaApobeda ? timA : timB,
                Gubitnik = timaApobeda ? timB : timA,
                KosevaPobednik = timaApobeda ? timARez : timBRez,
                KosevaGubitnik = timaApobeda ? timBRez : timARez
            };
        }


        public static List<RezultatMeca> SimulacijaGrupe(List<Tim> grupa)
        {
            List<RezultatMeca> rezGrupe = new List<RezultatMeca>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = i+1; j <= 3; j++)
                {
                    rezGrupe.Add(SimulacijaMeca(grupa[i], grupa[j]));
                    if (grupa[i] == rezGrupe[rezGrupe.Count - 1].Pobednik)
                    {
                        grupa[i].Pobeda += 1;
                        grupa[i].PostignutiKosevi += rezGrupe[rezGrupe.Count - 1].KosevaPobednik;
                        grupa[i].PrimljeniKosevi += rezGrupe[rezGrupe.Count - 1].KosevaGubitnik;
                        grupa[i].IgraoSa.Add(grupa[j].ISOCode);
                        grupa[i].KosRazlikaIgra.Add(rezGrupe[rezGrupe.Count - 1].KosevaPobednik - rezGrupe[rezGrupe.Count - 1].KosevaGubitnik);

                        grupa[j].Poraza += 1;
                        grupa[j].PostignutiKosevi += rezGrupe[rezGrupe.Count - 1].KosevaGubitnik;
                        grupa[j].PrimljeniKosevi += rezGrupe[rezGrupe.Count - 1].KosevaPobednik;
                        grupa[j].IgraoSa.Add(grupa[i].ISOCode);
                        grupa[j].KosRazlikaIgra.Add(rezGrupe[rezGrupe.Count - 1].KosevaPobednik - rezGrupe[rezGrupe.Count - 1].KosevaGubitnik);
                    }
                    else
                    {
                        grupa[i].Poraza += 1;
                        grupa[i].PostignutiKosevi += rezGrupe[rezGrupe.Count - 1].KosevaGubitnik;
                        grupa[i].PrimljeniKosevi += rezGrupe[rezGrupe.Count - 1].KosevaPobednik;
                        grupa[i].IgraoSa.Add(grupa[j].ISOCode);
                        grupa[i].KosRazlikaIgra.Add(rezGrupe[rezGrupe.Count - 1].KosevaPobednik - rezGrupe[rezGrupe.Count - 1].KosevaGubitnik);

                        grupa[j].Pobeda += 1;
                        grupa[j].PostignutiKosevi += rezGrupe[rezGrupe.Count - 1].KosevaPobednik;
                        grupa[j].PrimljeniKosevi += rezGrupe[rezGrupe.Count - 1].KosevaGubitnik;
                        grupa[j].IgraoSa.Add(grupa[i].ISOCode);
                        grupa[j].KosRazlikaIgra.Add(rezGrupe[rezGrupe.Count - 1].KosevaPobednik - rezGrupe[rezGrupe.Count - 1].KosevaGubitnik);
                    }
                }
            }

            return rezGrupe;
        }


        public static void IspisGrupe(List<RezultatMeca> grupaA, List<RezultatMeca> grupaB, List<RezultatMeca> grupaC)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Grupna faza - " + (i + 1) + ". kolo:");

                Console.WriteLine("\tGrupa A:");
                Console.WriteLine("\t\t" + grupaA[i].Pobednik.Team + " - " + grupaA[i].Gubitnik.Team + "(" + grupaA[i].KosevaPobednik + ":" + grupaA[i].KosevaGubitnik + ")");
                Console.WriteLine("\t\t" + grupaA[5 - i].Pobednik.Team + " - " + grupaA[5 - i].Gubitnik.Team + "(" + grupaA[5 - i].KosevaPobednik + ":" + grupaA[5 - i].KosevaGubitnik + ")");

                Console.WriteLine("\tGrupa B:");
                Console.WriteLine("\t\t" + grupaB[i].Pobednik.Team + " - " + grupaB[i].Gubitnik.Team + "(" + grupaB[i].KosevaPobednik + ":" + grupaB[i].KosevaGubitnik + ")");
                Console.WriteLine("\t\t" + grupaB[5 - i].Pobednik.Team + " - " + grupaB[5 - i].Gubitnik.Team + "(" + grupaB[5 - i].KosevaPobednik + ":" + grupaB[5 - i].KosevaGubitnik + ")");

                Console.WriteLine("\tGrupa C:");
                Console.WriteLine("\t\t" + grupaC[i].Pobednik.Team + " - " + grupaC[i].Gubitnik.Team + "(" + grupaC[i].KosevaPobednik + ":" + grupaC[i].KosevaGubitnik + ")");
                Console.WriteLine("\t\t" + grupaC[5 - i].Pobednik.Team + " - " + grupaC[5 - i].Gubitnik.Team + "(" + grupaC[5 - i].KosevaPobednik + ":" + grupaC[5 - i].KosevaGubitnik + ")" + "\n");
            }
        }


        public static void IspisPlasmana(List<Tim> grupa)
        {
            for (int i = 0; i < grupa.Count; i++)
            {
                grupa[i].Bodovi = (grupa[i].Pobeda * 2) + grupa[i].Poraza;
                grupa[i].KosRazlika = (grupa[i].PostignutiKosevi - grupa[i].PrimljeniKosevi);
            }

            for (int i = 0; i < grupa.Count - 1; i++)
            {
                for (int j = i + 1; j < grupa.Count; j++)
                {
                    if (grupa[i].Bodovi < grupa[j].Bodovi)
                    {
                        Tim tmp = new Tim();
                        tmp = grupa[i];
                        grupa[i] = grupa[j];
                        grupa[j] = tmp;
                    }

                    if (grupa[i].Bodovi == grupa[j].Bodovi)
                    {
                        for (int k = 0; k < grupa[i].IgraoSa.Count; k++)
                        {
                            if (grupa[i].IgraoSa[k] == grupa[j].ISOCode)
                            {
                                if (grupa[i].KosRazlikaIgra[k] < 0)
                                {
                                    Tim tmp = new Tim();
                                    tmp = grupa[i];
                                    grupa[i] = grupa[j];
                                    grupa[j] = tmp;
                                }
                            }
                        }
                    }

                }
            }

            int pom = 0;
            for (int i = 0; i < grupa.Count - 1; i++)
            {
                for (int j = i + 1; j < grupa.Count; j++)
                {
                    if (grupa[i].Bodovi == grupa[j].Bodovi)
                    {
                        pom++;
                    }
                }
            }

            if (pom > 2)
            {
                List<Tim> nereseni = new List<Tim>();
                for (int i = 0; i < grupa.Count; i++)
                {
                    for (int j = i + 1; j < grupa.Count; j++)
                    {
                        if (grupa[i].Bodovi == grupa[j].Bodovi && !nereseni.Contains(grupa[i]))
                        {
                            nereseni.Add(grupa[i]);
                        }
                        if (grupa[i].Bodovi == grupa[j].Bodovi && !nereseni.Contains(grupa[j]))
                        {
                            nereseni.Add(grupa[j]);
                        }
                    }
                }

                int[] pomNiz = { 0, 0, 0 };
                for (int i = 0; i < nereseni.Count; i++)
                {
                    for (int j = 0; j < nereseni[i].IgraoSa.Count; j++)
                    {
                        if (nereseni[i].IgraoSa[j] == nereseni[i].ISOCode)
                        {
                            pomNiz[i] += nereseni[i].PostignutiKosevi;
                        }
                    }
                }

                for (int i = 0; i < nereseni.Count; i++)
                {
                    for (int j = 0; j < nereseni.Count; j++)
                    {
                        if (pomNiz[i] < pomNiz[j])
                        {
                            Tim pom1 = nereseni[i];
                            nereseni[i] = nereseni[j];
                            nereseni[j] = pom1;
                        }
                    }
                }

                for (int i = 0; i < grupa.Count; i++)
                {
                    if (grupa[i].Bodovi == nereseni[0].Bodovi)
                    {
                        grupa[i] = nereseni[0];
                        grupa[i + 1] = nereseni[1];
                        grupa[i + 2] = nereseni[2];
                        break;
                    }
                }
            }
            for (int i = 0; i < grupa.Count; i++)
            {
                Console.WriteLine("\t\t" + (i + 1) + " " + grupa[i].ToString());
            }
            Console.WriteLine("\n");
        }


        public static List<Tim> EliminacionaFaza(List<Tim> grupaA, List<Tim> grupaB, List<Tim> grupaC)
        {
            List<Tim> prosliGrupnu = new List<Tim>();
            prosliGrupnu.Add(grupaA[0]);
            prosliGrupnu.Add(grupaB[0]);
            prosliGrupnu.Add(grupaC[0]);

            for (int i = 0; i < prosliGrupnu.Count; i++)
            {
                for (int j = 0; j < prosliGrupnu.Count; j++)
                {
                    if (prosliGrupnu[i].Bodovi == prosliGrupnu[j].Bodovi)
                    {
                        if (prosliGrupnu[i].KosRazlika == prosliGrupnu[j].KosRazlika)
                        {
                            if (prosliGrupnu[i].PostignutiKosevi > prosliGrupnu[j].PostignutiKosevi)
                            {
                                Tim pom = new Tim();
                                pom = prosliGrupnu[i];
                                prosliGrupnu[i] = prosliGrupnu[j];
                                prosliGrupnu[j] = pom;
                            }
                        }
                        else
                        {
                            if (prosliGrupnu[i].KosRazlika > prosliGrupnu[j].KosRazlika)
                            {
                                Tim pom = new Tim();
                                pom = prosliGrupnu[i];
                                prosliGrupnu[i] = prosliGrupnu[j];
                                prosliGrupnu[j] = pom;
                            }
                        }
                    }
                    else
                    {
                        if (prosliGrupnu[i].Bodovi > prosliGrupnu[j].Bodovi)
                        {
                            Tim pom = new Tim();
                            pom = prosliGrupnu[i];
                            prosliGrupnu[i] = prosliGrupnu[j];
                            prosliGrupnu[j] = pom;
                        }
                    }
                }
            }

            prosliGrupnu.Add(grupaA[1]);
            prosliGrupnu.Add(grupaB[1]);
            prosliGrupnu.Add(grupaC[1]);

            for (int i = 3; i < prosliGrupnu.Count; i++)
            {
                for (int j = 3; j < prosliGrupnu.Count; j++)
                {
                    if (prosliGrupnu[i].Bodovi == prosliGrupnu[j].Bodovi)
                    {
                        if (prosliGrupnu[i].KosRazlika == prosliGrupnu[j].KosRazlika)
                        {
                            if (prosliGrupnu[i].PostignutiKosevi > prosliGrupnu[j].PostignutiKosevi)
                            {
                                Tim pom = new Tim();
                                pom = prosliGrupnu[i];
                                prosliGrupnu[i] = prosliGrupnu[j];
                                prosliGrupnu[j] = pom;
                            }
                        }
                        else
                        {
                            if (prosliGrupnu[i].KosRazlika > prosliGrupnu[j].KosRazlika)
                            {
                                Tim pom = new Tim();
                                pom = prosliGrupnu[i];
                                prosliGrupnu[i] = prosliGrupnu[j];
                                prosliGrupnu[j] = pom;
                            }
                        }
                    }
                    else
                    {
                        if (prosliGrupnu[i].Bodovi > prosliGrupnu[j].Bodovi)
                        {
                            Tim pom = new Tim();
                            pom = prosliGrupnu[i];
                            prosliGrupnu[i] = prosliGrupnu[j];
                            prosliGrupnu[j] = pom;
                        }
                    }
                }
            }

            prosliGrupnu.Add(grupaA[2]);
            prosliGrupnu.Add(grupaB[2]);
            prosliGrupnu.Add(grupaC[2]);

            for (int i = 6; i < prosliGrupnu.Count; i++)
            {
                for (int j = 6; j < prosliGrupnu.Count; j++)
                {
                    if (prosliGrupnu[i].Bodovi == prosliGrupnu[j].Bodovi)
                    {
                        if (prosliGrupnu[i].KosRazlika == prosliGrupnu[j].KosRazlika)
                        {
                            if (prosliGrupnu[i].PostignutiKosevi > prosliGrupnu[j].PostignutiKosevi)
                            {
                                Tim pom = new Tim();
                                pom = prosliGrupnu[i];
                                prosliGrupnu[i] = prosliGrupnu[j];
                                prosliGrupnu[j] = pom;
                            }
                        }
                        else
                        {
                            if (prosliGrupnu[i].KosRazlika > prosliGrupnu[j].KosRazlika)
                            {
                                Tim pom = new Tim();
                                pom = prosliGrupnu[i];
                                prosliGrupnu[i] = prosliGrupnu[j];
                                prosliGrupnu[j] = pom;
                            }
                        }
                    }
                    else
                    {
                        if (prosliGrupnu[i].Bodovi > prosliGrupnu[j].Bodovi)
                        {
                            Tim pom = new Tim();
                            pom = prosliGrupnu[i];
                            prosliGrupnu[i] = prosliGrupnu[j];
                            prosliGrupnu[j] = pom;
                        }
                    }
                }
            }

            return prosliGrupnu;
        }


        public static Sesir KreiranjeSesira(List<Tim> timoviProsliGrupnu)
        {
            Sesir sesir = new Sesir();
            sesir.D = new List<Tim>();
            sesir.E = new List<Tim>();
            sesir.F = new List<Tim>();
            sesir.G = new List<Tim>();
            timoviProsliGrupnu.RemoveAt(8);
            for (int i = 0; i < timoviProsliGrupnu.Count; i++)
            {
                if (i < 2)
                {
                    sesir.D.Add(timoviProsliGrupnu[i]);
                }
                else if (i < 4)
                {
                    sesir.E.Add(timoviProsliGrupnu[i]);
                }
                else if (i < 6)
                {
                    sesir.F.Add(timoviProsliGrupnu[i]);
                }
                else
                {
                    sesir.G.Add(timoviProsliGrupnu[i]);
                }
            }

            return sesir;
        }


        public static void IspisSesira(Sesir sesir)
        {
            Console.WriteLine("Šeširi:");
            
            Console.WriteLine("\tŠešir D");
            foreach (Tim i in sesir.D)
            {
                Console.WriteLine("\t\t" + i.Team);
            }

            Console.WriteLine("\tŠešir E");
            foreach (Tim i in sesir.E)
            {
                Console.WriteLine("\t\t" + i.Team);
            }

            Console.WriteLine("\tŠešir F");
            foreach (Tim i in sesir.F)
            {
                Console.WriteLine("\t\t" + i.Team);
            }

            Console.WriteLine("\tŠešir G");
            foreach (Tim i in sesir.G)
            {
                Console.WriteLine("\t\t" + i.Team);
            }
        }


        public static List<Tim> PravljenjeParovaEliminacione(Sesir sesir)
        {
            List<int> parovi = new List<int>();
            parovi.Add(0);
            if (sesir.D[0].IgraoSa.Contains(sesir.G[0].ISOCode) && !sesir.D[1].IgraoSa.Contains(sesir.G[0].ISOCode))
            {
                parovi.Add(1);
                parovi.Add(1);
                parovi.Add(0);
            }
            else if (sesir.D[0].IgraoSa.Contains(sesir.G[1].ISOCode) && !sesir.D[1].IgraoSa.Contains(sesir.G[1].ISOCode))
            {
                parovi.Add(0);
                parovi.Add(1);
                parovi.Add(1);
            }
            else
            {
                Random rnd = new Random();
                parovi.Add(rnd.Next(0, 2));
                parovi.Add(1);
                if (parovi[1] == 0)
                    parovi.Add(1);
                else 
                    parovi.Add(0);
            }

            parovi.Add(0);
            if (sesir.E[0].IgraoSa.Contains(sesir.F[0].ISOCode) && !sesir.E[1].IgraoSa.Contains(sesir.F[0].ISOCode))
            {
                parovi.Add(1);
                parovi.Add(1);
                parovi.Add(0);
            }
            else if (sesir.E[0].IgraoSa.Contains(sesir.F[1].ISOCode) && !sesir.E[1].IgraoSa.Contains(sesir.F[1].ISOCode))
            {
                parovi.Add(0);
                parovi.Add(1);
                parovi.Add(1);
            }
            else
            {
                Random rnd = new Random();
                parovi.Add(rnd.Next(0, 2));
                parovi.Add(1);
                if (parovi[1] == 0)
                    parovi.Add(1);
                else
                    parovi.Add(0);
            }

            List<Tim> timovi = new List<Tim>();
            timovi.Add(sesir.D[parovi[0]]);
            timovi.Add(sesir.G[parovi[1]]);
            timovi.Add(sesir.D[parovi[2]]);
            timovi.Add(sesir.G[parovi[3]]);
            timovi.Add(sesir.E[parovi[4]]);
            timovi.Add(sesir.F[parovi[5]]);
            timovi.Add(sesir.E[parovi[6]]);
            timovi.Add(sesir.F[parovi[7]]);

            return timovi;
        }


        public static void IspisEliminacione(List<Tim> timovi)
        {
            Console.WriteLine("\n\nEliminaciona faza:");
            for (int i = 0; i < timovi.Count - 1; i+=2)
            {
                Console.WriteLine("\t" + timovi[i].Team + " - " + timovi[i+1].Team);
                if(i == 2)
                {
                    Console.WriteLine("\n");
                }
            }
        }


        public static List<Tim> SimulacijaCetvrtfinala(List<Tim> timovi)
        {
            Console.WriteLine("\nČetvrtfinale:");
            List<RezultatMeca> rezultati = new List<RezultatMeca>();
            int pom = 0;
            for (int i = 0; i < timovi.Count - 1; i += 2)
            {
                rezultati.Add(SimulacijaMeca(timovi[i], timovi[i + 1]));
                Console.WriteLine("\t" + rezultati[pom].Pobednik.Team + " - " + rezultati[pom].Gubitnik.Team + "(" + rezultati[pom].KosevaPobednik + ":" + rezultati[pom].KosevaGubitnik + ")");
                pom++;
            }

            for (int i = 0; i < rezultati.Count; i++)
            {
                for (int j = 0; j < timovi.Count; j++)
                {
                    if (timovi[j] == rezultati[i].Gubitnik)
                    {
                        timovi.RemoveAt(j);
                    }
                }
            }

            return timovi;
        }


        public static List<RezultatMeca> PolufinaleSimulacija(List<Tim> timovi)
        {
            Console.WriteLine("\nPolufinale:");
            int pom = 0;
            List<RezultatMeca> rezultati = new List<RezultatMeca>();
            for (int i = 0; i < timovi.Count - 1; i += 2)
            {
                rezultati.Add(SimulacijaMeca(timovi[i], timovi[i + 1]));
                Console.WriteLine("\t" + rezultati[pom].Pobednik.Team + " - " + rezultati[pom].Gubitnik.Team + "(" + rezultati[pom].KosevaPobednik + ":" + rezultati[pom].KosevaGubitnik + ")");
                pom++;
            }

            return rezultati;
        }


        public static List<RezultatMeca> Finale(List<RezultatMeca> rezultati)
        {
            List<RezultatMeca> rezultatiFinale = new List<RezultatMeca>();

            Console.WriteLine("\nUtakmica za treće mesto:");
            List<Tim> timovi = new List<Tim>();
            for (int i = 0; i < rezultati.Count; i++)
            {
                timovi.Add(rezultati[i].Gubitnik);
            }

            rezultatiFinale.Add(SimulacijaMeca(timovi[0], timovi[1]));
            Console.WriteLine("\t" + rezultatiFinale[0].Pobednik.Team + " - " + rezultatiFinale[0].Gubitnik.Team + "(" + rezultatiFinale[0].KosevaPobednik + ":" + rezultatiFinale[0].KosevaGubitnik + ")");
            timovi.Clear();

            Console.WriteLine("\nFinale:");
            for (int i = 0; i < rezultati.Count; i++)
            {
                timovi.Add(rezultati[i].Pobednik);
            }
            rezultatiFinale.Add(SimulacijaMeca(timovi[0], timovi[1]));
            Console.WriteLine("\t" + rezultatiFinale[1].Pobednik.Team + " - " + rezultatiFinale[1].Gubitnik.Team + "(" + rezultatiFinale[1].KosevaPobednik + ":" + rezultatiFinale[1].KosevaGubitnik + ")");

            return rezultatiFinale;
        }


        public static void IspisMedalja(List<RezultatMeca> rezultatiFinale)
        {
            Console.WriteLine("\n\nMedalje:");
            Console.WriteLine("\t1. " + rezultatiFinale[1].Pobednik.Team);
            Console.WriteLine("\t2. " + rezultatiFinale[1].Gubitnik.Team);
            Console.WriteLine("\t3. " + rezultatiFinale[0].Pobednik.Team);
        }
    }
}
