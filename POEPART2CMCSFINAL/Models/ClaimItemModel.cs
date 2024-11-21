namespace POEPART2CMCSFINAL.Models
{
    public class ClaimItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateClaimed { get; set; }
        public string status { get; set; }
        public int Hours { get; set; }
        public double rate { get; set; }
        public double TotalFee { get; set; }


    }
}
