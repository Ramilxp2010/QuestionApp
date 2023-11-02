using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionAppLibrary.Models
{
    public class VoteModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public UserModel Author { get; set; }

        public QuestionModel Question { get; set; }

    }
}
