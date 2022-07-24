using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.Firestore.Models;

namespace UniAdmissionPlatform.Firestore
{
    public interface ICommentService
    {
        Task CreateEventComment(Comment comment);
        Task<PageResult<Comment>> GetEventComment(int eventId, int page, int limit);
        Task<PageResult<Comment>> GetUniversityComment(int universityId, int page, int limit);

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
            var collectionReference = _db.Collection("Comment").Document("Event").Collection("Comments");
            await collectionReference.AddAsync(comment);
        }

        public async Task<PageResult<Comment>> GetEventComment(int eventId, int page, int limit)
        {
            var snapshotAsync = await _db.Collection("Comment").Document("Event").Collection("Comments").WhereEqualTo("ReferenceId", eventId).GetSnapshotAsync();
            var total = snapshotAsync.Count;
            var querySnapshot = await _db.Collection("Comment").Document("Event").Collection("Comments").WhereEqualTo("ReferenceId", eventId).OrderByDescending("CreatedDate").OrderByDescending("UpdatedDate").Offset((page - 1) * limit).Limit(limit).GetSnapshotAsync();
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

            return new PageResult<Comment>
            {
                Limit = limit,
                Page = page,
                Total = total,
                List = comments
            };
        }

        public async Task<PageResult<Comment>> GetUniversityComment(int universityId, int page, int limit)
        {
            var snapshotAsync = await _db.Collection("Comment").Document("University").Collection("Comments").WhereEqualTo("ReferenceId", universityId).GetSnapshotAsync();
            var total = snapshotAsync.Count;
            var querySnapshot = await _db.Collection("Comment").Document("University").Collection("Comments").WhereEqualTo("ReferenceId", universityId).OrderByDescending("CreatedDate").OrderByDescending("UpdatedDate").Offset((page - 1) * limit).Limit(limit).GetSnapshotAsync();
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

            return new PageResult<Comment>
            {
                Limit = limit,
                Page = page,
                Total = total,
                List = comments
            };
        }


        public async Task CreateUniversityComment(Comment comment)
        {
            var collectionReference = _db.Collection("Comment").Document("University").Collection("Comments");
            await collectionReference.AddAsync(comment);
        }
    }
}