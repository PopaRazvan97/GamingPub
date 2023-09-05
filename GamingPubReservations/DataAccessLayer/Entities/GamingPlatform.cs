using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class GamingPlatform : BaseEntity
    {
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<GamingPub> GamingPubs { get; set; }

        public List<Reservation> Reservations { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is GamingPlatform platform &&
                   Id == platform.Id &&
                   Name == platform.Name &&
                   EqualityComparer<ICollection<GamingPub>>.Default.Equals(GamingPubs, platform.GamingPubs) &&
                   EqualityComparer<List<Reservation>>.Default.Equals(Reservations, platform.Reservations);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, GamingPubs, Reservations);
        }
    }
}