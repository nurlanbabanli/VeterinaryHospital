using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Doctor : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsConfirmed { get; set; }
        public double Raiting { get; set; }
        public string ImageUrl { get; set; }
    }
}
