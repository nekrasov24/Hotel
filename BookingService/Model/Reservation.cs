using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Model
{
    public class Reservation
    {
        [BsonId]
        [BsonElement("Id")]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        [BsonElement("UserId")]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        [BsonElement("RoomId")]
        [BsonRepresentation(BsonType.String)]
        public Guid RoomId { get; set; }
        [BsonElement("StartDateOfBooking")]
        [BsonRepresentation(BsonType.String)]
        public DateTime StartDateOfBooking { get; set; }
        [BsonElement("FinishDateOfBooking")]
        [BsonRepresentation(BsonType.String)]
        public DateTime FinishDateOfBooking { get; set; }
        [BsonElement("ReservStartDate")]
        [BsonRepresentation(BsonType.String)]
        public DateTime ReservStartDate { get; set; }
        [BsonElement("ReservFinishedDate")]
        [BsonRepresentation(BsonType.String)]
        public DateTime ReservFinishedDate { get; set; }
        [BsonElement("NumberOfNights")]
        [BsonRepresentation(BsonType.String)]
        public int NumberOfNights { get; set; }
        [BsonElement("AmountPaid")]
        [BsonRepresentation(BsonType.String)]
        public Decimal AmountPaid { get; set; }
    }
}
