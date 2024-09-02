using Basketball_tournament;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        Grupe grupe = Funkcije.UcitajJson();

        List<RezultatMeca> grupaA = Funkcije.SimulacijaGrupe(grupe.A);
        List<RezultatMeca> grupaB = Funkcije.SimulacijaGrupe(grupe.B);
        List<RezultatMeca> grupaC = Funkcije.SimulacijaGrupe(grupe.C);

        Funkcije.IspisGrupe(grupaA, grupaB, grupaC);

        Console.WriteLine("Konačan plasman u grupama:");
        Console.WriteLine("\tGrupa A (Ime - pobede/porazi/bodovi/postignuti koševi/primljeni koševi/koš razlika):");
        Funkcije.IspisPlasmana(grupe.A);

        Console.WriteLine("\tGrupa B (Ime - pobede/porazi/bodovi/postignuti koševi/primljeni koševi/koš razlika):");
        Funkcije.IspisPlasmana(grupe.B);

        Console.WriteLine("\tGrupa C (Ime - pobede/porazi/bodovi/postignuti koševi/primljeni koševi/koš razlika):");
        Funkcije.IspisPlasmana(grupe.C);

        List<Tim> timoviProsliGrupnu = new List<Tim>();
        timoviProsliGrupnu = Funkcije.EliminacionaFaza(grupe.A, grupe.B, grupe.C);

        Sesir sesir = new Sesir();
        sesir = Funkcije.KreiranjeSesira(timoviProsliGrupnu);

        Funkcije.IspisSesira(sesir);

        List<Tim> parovi = new List<Tim>();
        parovi = Funkcije.PravljenjeParovaEliminacione(sesir);

        Funkcije.IspisEliminacione(parovi);

        List<Tim> timoviProsliCetrvt = new List<Tim>();
        timoviProsliCetrvt = Funkcije.SimulacijaCetvrtfinala(parovi);

        List<RezultatMeca> rezultatiPolufinala = new List<RezultatMeca>();
        rezultatiPolufinala = Funkcije.PolufinaleSimulacija(parovi);

        List<RezultatMeca> rezultatiFinala = new List<RezultatMeca>();
        rezultatiFinala = Funkcije.Finale(rezultatiPolufinala);

        Funkcije.IspisMedalja(rezultatiFinala);
    }
}