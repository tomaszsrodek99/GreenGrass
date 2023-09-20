namespace GreenGrassAPI.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int SenderId { get; set; } // Identyfikator wysyłającego zaproszenie
        public int ReceiverId { get; set; } // Identyfikator odbiorcy zaproszenia
        public DateTime DateSent { get; set; } // Data wysłania zaproszenia
        public bool IsAccepted { get; set; } // Flaga wskazująca, czy zaproszenie zostało zaakceptowane
    }

}
