using Google.Cloud.Firestore;
using Timestamp = Google.Cloud.Firestore.Timestamp;

namespace UniAdmissionPlatform.Firestore.Models
{
    [FirestoreData]
    public class Comment
    {
        public string Id { get; set; }
        [FirestoreProperty]
        public int ReferenceId { get; set; }
        [FirestoreProperty]
        public int UserId { get; set; }
        public string UserName { get; set; }
        [FirestoreProperty]
        public string CreatedDateString { get; set; }
        [FirestoreProperty]
        public Timestamp CreatedDate { get; set; }
        [FirestoreProperty]
        public Timestamp UpdatedDate { get; set; }
        [FirestoreProperty]
        public string UpdatedDateString { get; set; }
        [FirestoreProperty]
        public string Content { get; set; }

        public string Role { get; set; }

        public string AvatarUrl { get; set; } =
            "https://firebasestorage.googleapis.com/v0/b/uni-admission-platform.appspot.com/o/image%2F11825594-e757-4979-9f9c-11420d6ae1017%2F23%2F2022%204%3A34%3A38%20AM.jpg?alt=media&token=0cf8e921-3f37-49df-ae39-ad9164ee2d7c";
    }
}