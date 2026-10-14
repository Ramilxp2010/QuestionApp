namespace QuestionAppLibrary.Models
{
    public class BasicUser
    {
        public string Id { get; set; }

        public string ObjectId { get; set; }

        public string DisplayName { get; set; }

        public BasicUser()
        {

        }

        public BasicUser(User user)
        {
            Id = user.Id;
            ObjectId = user.ObjectIdentifier;
            DisplayName = user.DisplayName;
        }
    }
}
