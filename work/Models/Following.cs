using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace work.Models
{
    public class Following
    {

        [Key]
        [Column(Order = 1)]
        public string FollowerId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string FolloweeId { get; set; }
        [ForeignKey("FollowerId")]
        public ApplicationUser Follower { get; set; }
        [ForeignKey("FolloweeId")]
        public ApplicationUser Followee { get; set; }
    }
}