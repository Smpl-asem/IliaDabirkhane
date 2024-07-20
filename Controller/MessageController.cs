using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
[Route("[Action]")]
[ApiController]
public class MessageController : Controller
{
    private readonly Context db;
    public readonly MyFilter _myFilter;

    public MessageController(Context _db, MyFilter myFilter)
    {
        db = _db;
        _myFilter = myFilter;
    }



    [HttpPost]
    public IActionResult AddMessage(DtoMessage message)
    {
        using (var transaction = db.Database.BeginTransaction())
        {
            try
            {
                var newMessage = new Messages
                {
                    SerialNumber = message.SerialNumber,
                    SenderUserId = message.SenderUserId,
                    Subject = message.Subject,
                    BodyText = message.BodyText,
                    CreateDateTime = DateTime.Now
                };

                db.Messages_tbl.Add(newMessage);
                db.SaveChanges();


                int messageId = Convert.ToInt32(newMessage.Id);
                CreateMsgLog(messageId, (int)newMessage.SenderUserId, 3);

                foreach (var item in message.Resivers)
                {
                    db.Recivers_tbl.Add(new Recivers
                    {
                        ReciverId = item.ReciverId,
                        MessageId = messageId,
                        Type = item.Type,
                        CreateDateTime = DateTime.Now
                    });

                    CreateMsgLog(messageId, item.ReciverId, item.Type == "to" ? 4 : 5);
                }
                if (message.Atachments != null)
                {
                    foreach (var item in message.Atachments)
                    {
                        db.Attecheds_tbl.Add(new Atteched
                        {
                            FileName = item.FileName,
                            MessageId = messageId,
                            FilePath = Uploadimage.Upload(item.FilePath),
                            FileType = item.FileType,
                            CreateDateTime = DateTime.Now
                        });

                    }
                }

                db.SaveChanges();
                transaction.Commit();

                return Ok("Successful");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }




    //  [HttpGet]

    //     public async Task<IActionResult> Getallmessage([FromQuery] PaginationFilter filter, [FromQuery] Messages msg)
    //     {
    //        try
    //         {
    //             //AddLog


    //             if (!ModelState.IsValid)
    //             {
    //                 return BadRequest(new { status = 400, message = "عملیات با خطا مواجه شد" });
    //             }

    //             var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

    //             var pagedData = await _myFilter.PerformOperation(msg)
    //                .Include(x=> x.SenderUser)
    //                .Include(x=> x.Atteched)
    //                .Include(x=> x.Recivers)
    //                        .ThenInclude(r => r.Reciver)

    //                 .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
    //                 .Take(validFilter.PageSize)
    //                 .ToListAsync();

    //             var totalRecords = pagedData.Count();
    //             var totalCount = await _myFilter.PerformOperation(msg).CountAsync();
    //             var totalPage = (int)Math.Ceiling(totalCount / (double)validFilter.PageSize);
    //             return Ok(new PagedResponse<List<Messages>>(pagedData, validFilter.PageNumber, validFilter.PageSize, totalPage, totalCount));
    //         }
    //         catch (Exception ex)
    //         {
    //             return BadRequest(new { status = 400, message = "عملیات با خطا مواجه شد" + ex.Message });
    //         }
    //     }


    // [HttpGet]
    //     public IActionResult GetAllMessageNew([FromQuery] MessageDetailsFilter messageFilter, [FromQuery] ReciverDetailsFilter reciverFilter, [FromQuery] AttachDetailsFilter attachFilter, [FromQuery] PaginationFilter paginationFilter)
    //     {
    //         var query = db.Messages_tbl
    //             .Include(x => x.SenderUser)
    //             .Include(x => x.Recivers)
    //             .ThenInclude(x=>x.Reciver)
    //             .Include(x => x.Atteched)
    //             .AsQueryable();

    //         // Apply filters for Messages
    //         if (messageFilter.Id.HasValue)
    //             query = query.Where(m => m.Id == messageFilter.Id.Value);
    //         if (messageFilter.CreateDateTime.HasValue)
    //             query = query.Where(m => m.CreateDateTime == messageFilter.CreateDateTime.Value);
    //         if (!string.IsNullOrEmpty(messageFilter.SerialNumber))
    //             query = query.Where(m => m.SerialNumber.Contains(messageFilter.SerialNumber));
    //         if (messageFilter.SenderUserId.HasValue)
    //             query = query.Where(m => m.SenderUserId == messageFilter.SenderUserId.Value);
    //         if (!string.IsNullOrEmpty(messageFilter.Subject))
    //             query = query.Where(m => m.Subject.Contains(messageFilter.Subject));
    //         if (!string.IsNullOrEmpty(messageFilter.BodyText))
    //             query = query.Where(m => m.BodyText.Contains(messageFilter.BodyText));

    //         // Apply filters for Sender User
    //         if (!string.IsNullOrEmpty(messageFilter.Username_Sender))
    //             query = query.Where(m => m.SenderUser.Username.Contains(messageFilter.Username_Sender));


    //         if (!string.IsNullOrEmpty(messageFilter.FirstName_Sender))
    //             query = query.Where(m => m.SenderUser.FirstName.Contains(messageFilter.FirstName_Sender));
    //         if (!string.IsNullOrEmpty(messageFilter.LastName_Sender))
    //             query = query.Where(m => m.SenderUser.LastName.Contains(messageFilter.LastName_Sender));
    //         if (!string.IsNullOrEmpty(messageFilter.Phone_Sender))
    //             query = query.Where(m => m.SenderUser.Phone.Contains(messageFilter.Phone_Sender));
    //         if (!string.IsNullOrEmpty(messageFilter.Addres_Sender))
    //             query = query.Where(m => m.SenderUser.Addres.Contains(messageFilter.Addres_Sender));
    //         if (!string.IsNullOrEmpty(messageFilter.NatinalCode_Sender))
    //             query = query.Where(m => m.SenderUser.NatinalCode.Contains(messageFilter.NatinalCode_Sender));
    //         if (!string.IsNullOrEmpty(messageFilter.PerconalCode_Sender))
    //             query = query.Where(m => m.SenderUser.PerconalCode.Contains(messageFilter.PerconalCode_Sender));

    //         // Apply filters for Recivers
    //         if (reciverFilter != null)
    //         {
    //             query = query.Where(m => m.Recivers.Any(r =>
    //                 (!reciverFilter.ReciverId.HasValue || r.ReciverId == reciverFilter.ReciverId.Value) &&
    //                 (!reciverFilter.MessageId.HasValue || r.MessageId == reciverFilter.MessageId.Value) &&
    //                 (string.IsNullOrEmpty(reciverFilter.Type) || r.Type.Contains(reciverFilter.Type)) &&
    //                 (string.IsNullOrEmpty(reciverFilter.Username_Rciver) || r.Reciver.Username.Contains(reciverFilter.Username_Rciver)) &&
    //                 (string.IsNullOrEmpty(reciverFilter.FirstName_Reciver) || r.Reciver.FirstName.Contains(reciverFilter.FirstName_Reciver)) &&
    //                 (string.IsNullOrEmpty(reciverFilter.LastName_Reciver) || r.Reciver.LastName.Contains(reciverFilter.LastName_Reciver)) &&
    //                 (string.IsNullOrEmpty(reciverFilter.Phone_Reciver) || r.Reciver.Phone.Contains(reciverFilter.Phone_Reciver)) &&
    //                 (string.IsNullOrEmpty(reciverFilter.Addres_Reciver) || r.Reciver.Addres.Contains(reciverFilter.Addres_Reciver)) &&
    //                 (string.IsNullOrEmpty(reciverFilter.NatinalCode_Reciver) || r.Reciver.NatinalCode.Contains(reciverFilter.NatinalCode_Reciver)) &&
    //                 (string.IsNullOrEmpty(reciverFilter.PerconalCode_Reciver) || r.Reciver.PerconalCode.Contains(reciverFilter.PerconalCode_Reciver))
    //             ));
    //         }

    //         // Apply filters for Atteched
    //         if (attachFilter != null)
    //         {
    //             query = query.Where(m => m.Atteched.Any(a =>
    //                 (string.IsNullOrEmpty(attachFilter.FileName) || a.FileName.Contains(attachFilter.FileName)) &&
    //                 (string.IsNullOrEmpty(attachFilter.FilePath) || a.FilePath.Contains(attachFilter.FilePath)) &&
    //                 (string.IsNullOrEmpty(attachFilter.FileType) || a.FileType.Contains(attachFilter.FileType))
    //             ));
    //         }

    //         // Get total count before pagination
    //         var totalCount = query.Count();

    //         // Apply pagination
    //         var pagedData = query
    //             .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
    //             .Take(paginationFilter.PageSize)
    //             .OrderByDescending(x => x.Id)
    //             .ToList();

    //         // Calculate total pages
    //         var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

    //         var pagedResponse = new PagedResponse<List<Messages>>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

    //         return Ok(pagedResponse);
    //     }


    [HttpGet]
    [Authorize]
    public IActionResult GetAllMessageNew([FromQuery] MessageDetailsFilter messageFilter,
                                          [FromQuery] ReciverDetailsFilter reciverFilter,
                                          [FromQuery] AttachDetailsFilter attachFilter,
                                          [FromQuery] PaginationFilter paginationFilter)
    {
        int userId = (int)db.Users_tbl.FirstOrDefault(x => x.Username == User.FindFirstValue("username")).Id;
        var message3Filter = new messagefilter(userId);
        var query = db.Messages_tbl
            .Include(x => x.SenderUser)
            .Include(x => x.Recivers)
                .ThenInclude(x => x.Reciver)
            .Include(x => x.Atteched)
            .AsQueryable();
        message3Filter.ApplyMessageFilters(ref query, messageFilter);
        message3Filter.ApplyReciverFilters(ref query, reciverFilter);
        message3Filter.ApplyAttachFilters(ref query, attachFilter);

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => new
            {
                m.Id,
                SenderUser = new
                {
                    m.SenderUser.Id,
                    m.SenderUser.Username,
                    m.SenderUser.FirstName,
                    m.SenderUser.LastName,
                    // m.SenderUser.Phone,   حفظ حریم شخصی افراد با فاش نکردن اطلاعات حساس
                    // m.SenderUser.Addres,
                    // m.SenderUser.NatinalCode,
                    // m.SenderUser.PerconalCode,
                    // m.SenderUser.CreateDateTime
                },
                m.CreateDateTime,
                m.SerialNumber,
                m.Subject,
                m.BodyText,
                Recivers = m.Recivers.Select(r => new
                {
                    // r.Id,
                    // r.ReciverId,
                    // r.MessageId,
                    // r.Type,
                    // r.CreateDateTime,
                    Reciver = new
                    {
                        r.Reciver.Id,
                        r.Reciver.Username,
                        r.Reciver.FirstName,
                        r.Reciver.LastName,
                        r.Type
                        // r.Reciver.Phone,
                        // r.Reciver.Addres,
                        // r.Reciver.NatinalCode,
                        // r.Reciver.PerconalCode,
                        // r.Reciver.CreateDateTime
                    }
                }),
                Atteched = m.Atteched.Select(a => new
                {
                    a.Id,
                    a.FileName,
                    // a.MessageId,
                    a.FilePath,
                    a.FileType,
                    // a.CreateDateTime
                })
            })
            .ToList();
        foreach (var item in query)
        {
            foreach (var item2 in item.Recivers)
            {
                if (item2.ReciverId == userId && !db.msgLog_tbl.Any(x => x.UserId == userId && x.MessageId == item.Id && x.LogAction == 1))
                {
                    CreateMsgLog((int)item.Id, userId, 1);
                }
            }
        }

        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }

    [HttpDelete]
    [Authorize]
    public IActionResult TrashMessage(int MessageId)
    {
        Messages Check = db.Messages_tbl.Find(MessageId);
        int userId = (int)db.Users_tbl.FirstOrDefault(x => x.Username == User.FindFirstValue("username")).Id;

        if (Check == null)
            return Ok("Message Not Found");
        else if (Check.Trashed.Contains(userId))
            return Ok("Message Is Trash Already");
        else if (Check.Deleted.Contains(userId))
            return Ok("Message Is Not Avalable AnyMore");

        Check.Trashed.Add(userId);
        db.Messages_tbl.Update(Check);
        db.SaveChanges();
        CreateMsgLog((int)Check.Id, userId, 7);
        return Ok("Successfull");
    }

    [HttpDelete]
    [Authorize]
    public IActionResult UnTrashMessage(int MessageId)
    {
        Messages Check = db.Messages_tbl.Find(MessageId);
        int userId = (int)db.Users_tbl.FirstOrDefault(x => x.Username == User.FindFirstValue("username")).Id;

        if (Check == null)
            return Ok("Message Not Found");
        else if (Check.Trashed.Contains(userId))
        {
            Check.Trashed.Remove(userId);
            db.Messages_tbl.Update(Check);
            db.SaveChanges();
            CreateMsgLog((int)Check.Id, userId, 8);

            return Ok("Successfull");
        }
        else if (Check.Deleted.Contains(userId))
            return Ok("Message Is Not Avalable AnyMore");

        return Ok("Message Was Not a Trash");
    }

    [HttpDelete]
    public IActionResult DeleteMessage(int MessageId)
    {
        Messages Check = db.Messages_tbl.Find(MessageId);
        int userId = (int)db.Users_tbl.FirstOrDefault(x => x.Username == User.FindFirstValue("username")).Id;

        if (Check == null)
            return Ok("Message Not Found");
        else if (Check.Trashed.Contains(userId))
        {
            Check.Trashed.Remove(userId);
            Check.Deleted.Add(userId);
            db.Messages_tbl.Update(Check);
            db.SaveChanges();
            CreateMsgLog((int)Check.Id, userId, 2);

            return Ok("Successfull");
        }
        else if (Check.Deleted.Contains(userId))
            return Ok("Message Is Not Avalable AnyMore");

        return Ok("Message is not a Trash");
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetIndexMessage([FromQuery] PaginationFilter page)
    {
        // =-=-=- برای استفاده کردن از توکن
        int userId = (int)db.Users_tbl.FirstOrDefault(x => x.Username == User.FindFirstValue("username")).Id;
        return GetAllMessageNew(new MessageDetailsFilter { Trash = false }, new ReciverDetailsFilter { ReciverId = userId }, new AttachDetailsFilter(), page);
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetSendMessage([FromQuery] PaginationFilter page)
    {
        // =-=-=- برای استفاده کردن از توکن
        int userId = (int)db.Users_tbl.FirstOrDefault(x => x.Username == User.FindFirstValue("username")).Id;
        return GetAllMessageNew(new MessageDetailsFilter { SenderUserId = userId, Trash = false }, new ReciverDetailsFilter(), new AttachDetailsFilter(), page);
    }


    private void CreateMsgLog(int MessageId, int UserId, int LogAction)
    {
        // Add Log ---->
        db.msgLog_tbl.Add(new MessageLog
        {
            MessageId = MessageId,
            UserId = UserId,
            LogAction = LogAction,
            CreateDateTime = DateTime.Now
        });
        db.SaveChanges();
    }



}