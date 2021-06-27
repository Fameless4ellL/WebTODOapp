using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTODOapp.Models
{
    public class ToDoModel
    {
        public int id { get; set; }
        [StringLength(255, ErrorMessage = "title cannot be longer than 255 characters.")]
        public string title { get; set; }
        public bool completed { get; set; }
        [StringLength(20)]
        public string color { get; set; }
        private DateTime _date = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime Date_of_creation { get { return _date; } set { _date = value; } }

        // public string imgUrl { get; set; }


        public ToDoModel()
        {
            color = "white";
        }
    }
}
