namespace Domain.Models
{
    public class BlDsItem
    {
        public int n { get; set; }
        public int qmd { get; set; }
        public int zmd { get; set; }
        public double pmd { get; set; }
        public double pmo { get; set; }
        public int zmo { get; set; }
        public int qmo { get; set; }
        public long rid { get; set; }
    }

    public class MarketwatchItem
    {
        public string lva { get; set; }
        public string lvc { get; set; }
        public string eps { get; set; }
        public string pe { get; set; }
        public string pmd { get; set; }
        public string pmo { get; set; }
        public string qtj { get; set; }
        public string pdv { get; set; }
        public string ztt { get; set; }
        public string qtc { get; set; }
        public string bv { get; set; }
        public string pc { get; set; }
        public string pcpc { get; set; }
        public string pmn { get; set; }
        public string pmx { get; set; }
        public string py { get; set; }
        public string pf { get; set; }
        public string pcl { get; set; }
        public string vc { get; set; }
        public string csv { get; set; }
        public string insID { get; set; }
        public string pMax { get; set; }
        public string pMin { get; set; }
        public string ztd { get; set; }
        public List<BlDsItem> blDs { get; set; }
        public string id { get; set; }
        public string insCode { get; set; }
        public string dEven { get; set; }
        public string hEven { get; set; }
        public string pClosing { get; set; }
        public string iClose { get; set; }
        public string yClose { get; set; }
        public string pDrCotVal { get; set; }
        public string zTotTran { get; set; }
        public string qTotTran5J { get; set; }
        public string qTotCap { get; set; }
    }

    public class Root
    {
        public List<MarketwatchItem> marketwatch { get; set; }
    }
}