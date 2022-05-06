namespace iRead.API.Models.Order
{
    public class NewOrder
    {
        public int UserId { get; set; }
        public List<int> Books { get; set; }
    }
}
