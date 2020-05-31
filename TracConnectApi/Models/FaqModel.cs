using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracConnectApi.Models
{
    public class FaqModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<QuestionModel> Questions { get; set; } //= new List<QuestionModel>(); //new ArrayList();
    }

    public class QuestionModel
    {
        public int? Position { get; set; }
        public string Author { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        //public QuestionModel() { }
        //public QuestionModel(QuestionModel qm)
        //{
        //    this.Position = qm.Position;
        //    this.Author = qm.Author;
        //    this.CreateDate = qm.CreateDate;
        //    this.ExpirationDate = qm.ExpirationDate;
        //    this.Question = qm.Question;
        //    this.Answer = qm.Answer;
        //}
    }
}
