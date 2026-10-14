namespace QuestionAppLibrary.Models
{
    public class BasicUserDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ObjectId { get; set; }

        public string DisplayName { get; set; }

        public BasicUserDocument()
        {

        }

        public BasicUserDocument(UserDocument user)
        {
            Id = user.Id;
            ObjectId = user.ObjectIdentifier;
            DisplayName = user.DisplayName;
        }
    }
}
