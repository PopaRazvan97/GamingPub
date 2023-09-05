using BusinessLayer.Dtos;
using BusinessLayer.Infos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapping
{
    public static class ReservationMappingExtensions
    {
        public static Reservation ToReservation(this AddOrUpdateReservationDto reservationDto)
        {
            return new Reservation
            {
                StartDate = reservationDto.StartDate,
                EndDate = reservationDto.EndDate,
                GamingPlatformId = reservationDto.GamingPlatformId,
                GamingPubId = reservationDto.GamingPubId,
                UserId = reservationDto.UserId,
            };
        }

        public static ReservationInfo ToReservationInfo(this Reservation reservation)
        {
            return new ReservationInfo
            {
                UserId = reservation.UserId,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                GamingPlatformId = reservation.GamingPlatformId,
                GamingPubId = reservation.GamingPubId
            };
        }

        public static void Copy(this Reservation source, Reservation destination)
        {
            destination.StartDate = source.StartDate;
            destination.EndDate = source.EndDate;
            destination.GamingPlatformId = source.GamingPlatformId;
            destination.GamingPlatform = source.GamingPlatform == null ? null : source.GamingPlatform;
            destination.GamingPub = source.GamingPub == null ? null : source.GamingPub;
            destination.GamingPubId = source.GamingPubId;
            destination.UserId = source.UserId;
        }
    }
}