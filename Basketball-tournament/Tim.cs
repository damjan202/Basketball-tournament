using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketball_tournament
{
    public class Tim
    {
        public String Team { get; set; }
        public String ISOCode { get; set; }
        public int FIBARanking { get; set; }
        public int Pobeda { get; set; }
        public int Poraza { get; set; }
        public int Bodovi { get; set; }
        public int PostignutiKosevi { get; set; }
        public int PrimljeniKosevi { get; set; }
        public int KosRazlika { get; set; }
        public List<String> IgraoSa { get; set; }
        public List<int> KosRazlikaIgra { get; set; }
        public Tim() 
        {
            this.Team = String.Empty;
            this.ISOCode = String.Empty;
            this.FIBARanking = 0;
            this.Pobeda = 0;
            this.Poraza = 0;
            this.Bodovi = 0;
            this.PostignutiKosevi = 0;
            this.PrimljeniKosevi = 0;
            this.KosRazlika = 0;
            this.IgraoSa = new List<string>();
            this.KosRazlikaIgra = new List<int>();
        }
        public override string ToString()
        {
            return this.Team + "\t" + this.Pobeda + "/" + this.Poraza + "/" + this.Bodovi + "/" + this.PostignutiKosevi + "/" + this.PrimljeniKosevi + "/" + this.KosRazlika;
        }
    }
}
