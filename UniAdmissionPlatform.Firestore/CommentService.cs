using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using UniAdmissionPlatform.BusinessTier.Generations.Repositories;
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
        private readonly IAccountRepository _accountRepository;
        public CommentService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
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
            
            AddAccountInfo(comments);

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

            AddAccountInfo(comments);

            return new PageResult<Comment>
            {
                Limit = limit,
                Page = page,
                Total = total,
                List = comments
            };
        }

        private void AddAccountInfo(List<Comment> comments)
        {
            var accounts = _accountRepository.Get().Where(a => comments.Select(c => c.UserId).Contains(a.Id))
                .ToDictionary(a => a.Id, a => a);

            foreach (var comment in comments.Where(comment => accounts.ContainsKey(comment.UserId)))
            {
                var account = accounts[comment.UserId];
                comment.UserName = account.FirstName + ' ' + account.MiddleName + ' ' + account.LastName;
                comment.AvatarUrl = account.ProfileImageUrl;
                comment.Role = account.RoleId;
            }
        }


        public async Task CreateUniversityComment(Comment comment)
        {
            var collectionReference = _db.Collection("Comment").Document("University").Collection("Comments");
            await collectionReference.AddAsync(comment);
        }
    }
}