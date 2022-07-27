select E.Id                       as EventID,
       E.Name                     as EventName,
       A.FirstName                as FirstName,
       IFNULL(A.MiddleName, ' ')               as MiddleName,
       A.LastName                 as LastName,
       A.PhoneNumber              as PhoneNumber,
       IFNULL(A.EmailContact, '') as EmailContact,
       A.DateOfBirth              as DateOfBirth,
       HS.Id                      as HighSchoolId,
       HS.Name                    as HighSchoolname,
       W.Name                     as WardName,
       D.Name                     as DistrictName,
       P.Name                     as ProvinceName
from Event E
         join FollowEvent FE on E.Id = FE.EventId
         join Student S on FE.StudentId = S.Id
         join Account A on S.Id = A.Id
         join HighSchool HS on A.HighSchoolId = HS.Id
         join Ward W on A.WardId = W.Id
         join District D on W.DistrictId = D.Id
         join Province P on D.ProvinceId = P.Id
where EventId = 1;