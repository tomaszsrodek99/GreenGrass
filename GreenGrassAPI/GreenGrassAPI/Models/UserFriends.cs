using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenGrassAPI.Models
{
    public class UserFriends
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserNickname { get; set; }
        public int FriendId { get; set; }
        public string FriendNickname { get; set; }

        [NotMapped]
        public int NumberOfPlants
        {
            get
            {
                return Plants?.Count ?? 0;
            }
        }
        public ICollection<Plant> Plants { get; set; }
    }
}
