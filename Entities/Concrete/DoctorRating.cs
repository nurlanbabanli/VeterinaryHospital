using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class DoctorRating : IEntity
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int RatingFromPatient { get; set; }
        public string CommentFromPatient { get; set; }
        public bool ConfirmedRating { get; set; }
        public bool ConfirmedComment { get; set; }
    }
}
