using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using TracConnectApi.Models;
//using TracConnectApi.Services;

namespace TracConnectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaqController : ControllerBase
    {
        private readonly MongoCRUD mongoDB; // = new MongoCRUD("FAQ");
        private readonly string tableName = "FAQ";


        public FaqController(MongoCRUD mongoCRUD)
        {
            mongoDB = mongoCRUD; // new MongoCRUD("TRACCONNECT", configuration.GetConnectionString("FAQ_MongoDB"));
            //this.mongoDB = new MongoCRUD("TRACCONNECT", configuration.GetConnectionString("FAQ_MongoDB"));
        }

        //public interface IFaqController
        //{
        //    string ConnectionString { get; set; }
        //}

        // GET api/faq
        [HttpGet]
        public ActionResult<IEnumerable<FaqModel>> Get()
        {
            List<FaqModel> faqList = new List<FaqModel>();
            faqList = this.mongoDB.LoadRecords<FaqModel>(tableName);
            return faqList;
            //return new string[] { "value1", "value2" };
        }

        [Route("GetValidFaqs")]     // api/GetValidFaqs
        public ActionResult<IEnumerable<FaqModel>> GetValidFaqs()
        {
            DateTime dt = DateTime.Now;
            List<FaqModel> faqList = new List<FaqModel>();
            faqList = this.mongoDB.db.GetCollection<FaqModel>(tableName).Find(
                new BsonDocument("$and", new BsonArray {
                    new BsonDocument("Questions.CreateDate",
                    new BsonDocument("$lte", DateTime.Now)),
                    new BsonDocument("$or",
                    new BsonArray
                        {
                            new BsonDocument("Questions.ExpirationDate",
                            new BsonDocument("$eq", BsonNull.Value)),
                            new BsonDocument("Questions.ExpirationDate",
                            new BsonDocument("$gte", DateTime.Now))
                        })  })
                ).ToList<FaqModel>();
            // removed invalid QuestionModel List
            foreach (FaqModel f in faqList)
            {
                QuestionModel q;
                for (int i=f.Questions.Count-1; i>=0; i--)  //foreach (QuestionModel q in f.Questions)
                {
                    q = f.Questions[i];
                    if (q.ExpirationDate <= dt)
                        f.Questions.Remove(q);
                }
            }
            //this.mongoDB.db.wh
            //var fillter = Builders<FaqModel>.Filter.Where(..Eq("Id", id);
            ////faqList = mongoDB.db.GetCollection<FaqModel>(tableName).Find( //Aggregate<FaqModel>(
            ////    Builders<FaqModel>.Filter.ElemMatch(faq => faq.Questions, questions => (questions.CreateDate <= dt &&
            ////    questions.ExpirationDate == null) || questions.ExpirationDate >= dt ));
            //    new BsonDocument("$and", new BsonArray {
            //        new BsonDocument("Questions.CreateDate",
            //        new BsonDocument("$lte", DateTime.Now)),
            //        new BsonDocument("$or",
            //        new BsonArray
            //            {
            //                new BsonDocument("Questions.ExpirationDate",
            //                new BsonDocument("$eq", BsonNull.Value)),
            //                new BsonDocument("Questions.ExpirationDate",
            //                new BsonDocument("$gte", DateTime.Now))
            //            })  })
            //    );//.ToList<FaqModel>();

            //return collection.Find(new BsonDocument()).ToList();

            return faqList;
            //return new string[] { "value1", "value2" };
        }

        // GET api/faq/category_type
        [HttpGet("{category}")]
        public ActionResult<FaqModel> Get(string category)
        {
            return this.mongoDB.LoadRecordByFeild<FaqModel>(tableName, "Category", category);
            //return "value";
        }

        // GET api/faq/id (Guid)
        [HttpGet("{id}")]
        [Route("GetFaqByGuid")]  // api/GetFaqByGuid
        public ActionResult<FaqModel> GetFaqByGuid(Guid id)
        {
            return this.mongoDB.LoadRecordById<FaqModel>(tableName, id);
            //return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}