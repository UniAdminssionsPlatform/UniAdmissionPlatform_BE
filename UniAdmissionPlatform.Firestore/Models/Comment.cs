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
        [FirestoreProperty]
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
    }
}