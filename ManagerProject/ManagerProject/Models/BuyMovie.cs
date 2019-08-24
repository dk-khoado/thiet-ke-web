using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerProject.Models
{
    public class BuyMovie
    {
        public int IDMovie { get; set; }
        public string NameMovie { get; set; }
        public string Director { get; set; }
        public int Ticket { get; set; }  
        public int amount { get; set; }     
    }
}