namespace Nursery.Models
{
    public class MacsAccount
    {
        public int MacsAccountId { get; set; }
        public string MacDevice { get; set; }
        public string DeviceId { get; set; }
        public int? NurseryMemberId { get; set; }
        public virtual NurseryMember NurseryMember { get; set; }
    }
}
