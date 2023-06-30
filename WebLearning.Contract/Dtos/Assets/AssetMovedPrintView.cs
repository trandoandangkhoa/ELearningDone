namespace WebLearning.Contract.Dtos.Assets
{
    public class AssetMovedPrintView
    {

        public string Receiver { get; set; }
        public string RoleReciverName { get; set; }
        public string Sender { get; set; }
        public string RoleSenderName { get; set; }

        public List<string> OldDep { get; set; } = new List<string>();
        public string NewDep { get; set; }
        public string ReasonMove { get; set; }
        public List<AssetMovedTicket> AssetMovedTickets { get; set; }
    }
}
