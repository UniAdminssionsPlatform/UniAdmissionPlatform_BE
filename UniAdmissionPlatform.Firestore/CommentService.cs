using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using UniAdmissionPlatform.Firestore.Models;

namespace UniAdmissionPlatform.Firestore
{
    public interface ICommentService
    {
        Task CreateEventComment(Comment comment);
        Task<List<Comment>> GetEventComment(int eventId, int page, int limit);
        Task<List<Comment>> GetUniversityComment(int universityId, int page, int limit);

        Task CreateUniversityComment(Comment comment);
    }
    
    public class CommentService : ICommentService
    {
        private readonly FirestoreDb _db;
        public CommentService()
        {
            _db = FirestoreDb.Create("uni-admission-platform");
        }

        public async Task CreateEventComment(Comment comment)
        {
            var collectionReference = _db.Collection("Comment").Document("Event").Collection(comment.ReferenceId.ToString());
            await collectionReference.AddAsync(comment);
        }

        public async Task<List<Comment>> GetEventComment(int eventId, int page, int limit)
        {
            var querySnapshot = await _db.Collection("Comment").Document("Event").Collection(eventId.ToString()).OrderByDescending("CreatedDate").OrderByDescending("UpdatedDate").Offset((page - 1) * limit).Limit(limit).GetSnapshotAsync();
            var comments = new List<Comment>();
            var querySnapshotDocuments = querySnapshot.Documents;
            foreach (var querySnapshotDocument in querySnapshotDocuments)
            {
                if (querySnapshotDocument.Exists)
                {
                    var commentRaw = querySnapshotDocument.ToDictionary();
                    var json = JsonConvert.SerializeObject(commentRaw);
                    var comment = JsonConvert.DeserializeObject<Comment>(json);
                    comment!.Id = querySnapshotDocument.Id;
                    comments.Add(comment);
                }
            }

            return comments;
        }

        public async Task<List<Comment>> GetUniversityComment(int universityId, int page, int limit)
        {
            var querySnapshot = await _db.Collection("Comment").Document("University").Collection(universityId.ToString()).OrderByDescending("CreatedDate").OrderByDescending("UpdatedDate").Offset((page - 1) * limit).Limit(limit).GetSnapshotAsync();
            var comments = new List<Comment>();
            var querySnapshotDocuments = querySnapshot.Documents;
            foreach (var querySnapshotDocument in querySnapshotDocuments)
            {
                if (querySnapshotDocument.Exists)
                {
                    var commentRaw = querySnapshotDocument.ToDictionary();
                    var json = JsonConvert.SerializeObject(commentRaw);
                    var comment = JsonConvert.DeserializeObject<Comment>(json);
                    comment!.Id = querySnapshotDocument.Id;
                    comments.Add(comment);
                }
            }

            return comments;
        }


        public async Task CreateUniversityComment(Comment comment)
        {
            var collectionReference = _db.Collection("Comment").Document("University").Collection("Comments");
            await collectionReference.AddAsync(comment);
        }
    }
}