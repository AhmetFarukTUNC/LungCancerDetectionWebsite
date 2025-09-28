using System;
using System.ComponentModel.DataAnnotations;

namespace LungCancer.Models
{
    
        public class Prediction
        {
            public int Id { get; set; }
            public string Filename { get; set; }
            public string PredictedClass { get; set; }
            public double Confidence { get; set; }
            public DateTime CreatedAt { get; set; }

            // UserId int olacak
            public int UserId { get; set; }
        }
    

}
