using DAL.Models.Main;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{

    //allows to reuse facilities across multiple hotels without duplicating
    public class HotelFacility
    {
        public int HotelId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Hotel Hotel { get; set; }

        public int FacilityId { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public Facility Facility { get; set; }
    }

}
