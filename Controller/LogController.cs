using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("[Action]")]
[ApiController]
public class LogController : Controller
{
    private readonly Context db;
    public LogController(Context _db)
    {
        db = _db;
    }

    [HttpGet]
    public IActionResult GetAllUserLog([FromQuery] PaginationFilter paginationFilter , int? LogAction)
    {
        var query = db.userLogs_tbl.AsQueryable();

        if(LogAction.HasValue){//چک کردن بر اساس عملیات و اکشن
            query = query.Where(x=> x.LogAction == (int)LogAction);
        }

        query.Include(x => x.User);
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => new
            {
                m.Id,
                User = new
                {
                    m.User.Id,
                    m.User.Username,
                    m.User.FirstName,
                    m.User.LastName,
                    m.User.Profile
                },
                LogAction = userCodeToAction(m.LogAction, m.isSucces),
                m.CreateDateTime
            })
            .ToList();
        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }
    [HttpGet]
    public IActionResult GetAllUserLogOneLine([FromQuery] PaginationFilter paginationFilter , int? LogAction)
    {
        var query = db.userLogs_tbl.AsQueryable();

        if(LogAction.HasValue){//چک کردن بر اساس عملیات و اکشن
            query = query.Where(x=> x.LogAction == (int)LogAction);
        }

        query.Include(x => x.User);
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => $"{m.User.Username} {userCodeToAction(m.LogAction, m.isSucces)} {m.CreateDateTime}")
            .ToList();
        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }
    [HttpGet]
    public IActionResult GetAllMessageLog([FromQuery] PaginationFilter paginationFilter , int? LogAction)
    {
        var query = db.msgLog_tbl.AsQueryable();

        if(LogAction.HasValue){//چک کردن بر اساس عملیات و اکشن
            query = query.Where(x=> x.LogAction == (int)LogAction);
        }

        query.Include(x => x.User)
        .Include(x => x.Message)
            .ThenInclude(x => x.SenderUser)
        .Include(x => x.Message)
            .ThenInclude(x => x.Recivers)
                .ThenInclude(x => x.Reciver)
        .Include(x => x.Message)
            .ThenInclude(x => x.Atteched);


        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => new
            {
                m.Id,
                User = new
                {
                    m.User.Id,
                    m.User.Username,
                    m.User.FirstName,
                    m.User.LastName,
                    m.User.Profile
                },
                Message = new
                {
                    messageId = m.Message.Id,
                    m.Message.Subject,
                    m.Message.BodyText,
                    Sender = new
                    {
                        m.Message.SenderUser.Id,
                        m.Message.SenderUser.Username,
                        m.Message.SenderUser.FirstName,
                        m.Message.SenderUser.LastName,
                        m.Message.SenderUser.Profile,
                    },
                    Recivers = m.Message.Recivers.Select(x => new
                    {
                        x.Reciver.Id,
                        x.Reciver.Username,
                        x.Reciver.FirstName,
                        x.Reciver.LastName,
                        x.Reciver.Profile
                    }),
                    Atteched = m.Message.Atteched.Select(x => new
                    {
                        x.Id,
                        x.FileName,
                        x.FilePath,
                        x.FileType,
                    })
                },
                Action = msgCodeToAction(m.LogAction),
                m.Message.CreateDateTime

            })
            .ToList();
        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }
    [HttpGet]
    public IActionResult GetAllMessageLogOneLine([FromQuery] PaginationFilter paginationFilter , int? LogAction)
    {
        var query = db.msgLog_tbl.AsQueryable();

        if(LogAction.HasValue){//چک کردن بر اساس عملیات و اکشن
            query = query.Where(x=> x.LogAction == (int)LogAction);
        }

        query.Include(x => x.User)
        .Include(x => x.Message)
            .ThenInclude(x => x.SenderUser)
        .Include(x => x.Message)
            .ThenInclude(x => x.Recivers)
                .ThenInclude(x => x.Reciver)
        .Include(x => x.Message)
            .ThenInclude(x => x.Atteched);


        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => $"{m.User.Username} {msgCodeToAction(m.LogAction)} [{m.Message.Id}] {m.Message.Subject} ({m.Message.BodyText.Substring(0, 26)}) {m.CreateDateTime}")
            .ToList();
        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }
    [HttpGet]
    public IActionResult GetOneUserLog([FromQuery] PaginationFilter paginationFilter, int UserId , int? LogAction)
    {
        var query = db.userLogs_tbl.AsQueryable();

        if(LogAction.HasValue){//چک کردن بر اساس عملیات و اکشن
            query = query.Where(x=> x.LogAction == (int)LogAction);
        }

        query
        .Where(x => x.UserId == UserId)
        .Include(x => x.User);
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => new
            {
                m.Id,
                User = new
                {
                    m.User.Id,
                    m.User.Username,
                    m.User.FirstName,
                    m.User.LastName,
                    m.User.Profile
                },
                LogAction = userCodeToAction(m.LogAction, m.isSucces),
                m.CreateDateTime
            })
            .ToList();
        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }
    [HttpGet]
    public IActionResult GetOneUserLogOneLine([FromQuery] PaginationFilter paginationFilter, int UserId,int? LogAction)
    {
        var query = db.userLogs_tbl.AsQueryable();

        if(LogAction.HasValue){//چک کردن بر اساس عملیات و اکشن
            query = query.Where(x=> x.LogAction == (int)LogAction);
        }

        query.Where(x => x.UserId == UserId)
        .Include(x => x.User);
        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => $"{m.User.Username} {userCodeToAction(m.LogAction, m.isSucces)} {m.CreateDateTime}")
            .ToList();
        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }
    
    private bool msgFilter(int? UserId , int? MessageId , int UID , int MsgID){
            if (UserId.HasValue && MessageId.HasValue)
            {
                return false;
            }
            else if(UserId.HasValue){
                return UID == (int)UserId;
            }
            else if(MessageId.HasValue){
                return MsgID == MessageId;
            }
            else{
                return false;
            }
    }

    [HttpGet]
    public IActionResult GetOneMessageLog([FromQuery] PaginationFilter paginationFilter, int? UserId, int? MessageId , int? LogAction)
    {
        var query = db.msgLog_tbl.AsQueryable();

        if(LogAction.HasValue){//چک کردن بر اساس عملیات و اکشن
            query = query.Where(x=> x.LogAction == (int)LogAction);
        }

        query
        .Where(x => msgFilter(UserId , MessageId , x.UserId , x.MessageId))
        .Include(x => x.User)
        .Include(x => x.Message)
            .ThenInclude(x => x.SenderUser)
        .Include(x => x.Message)
            .ThenInclude(x => x.Recivers)
                .ThenInclude(x => x.Reciver)
        .Include(x => x.Message)
            .ThenInclude(x => x.Atteched);


        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => new
            {
                m.Id,
                User = new
                {
                    m.User.Id,
                    m.User.Username,
                    m.User.FirstName,
                    m.User.LastName,
                    m.User.Profile
                },
                Message = new
                {
                    messageId = m.Message.Id,
                    m.Message.Subject,
                    m.Message.BodyText,
                    Sender = new
                    {
                        m.Message.SenderUser.Id,
                        m.Message.SenderUser.Username,
                        m.Message.SenderUser.FirstName,
                        m.Message.SenderUser.LastName,
                        m.Message.SenderUser.Profile,
                    },
                    Recivers = m.Message.Recivers.Select(x => new
                    {
                        x.Reciver.Id,
                        x.Reciver.Username,
                        x.Reciver.FirstName,
                        x.Reciver.LastName,
                        x.Reciver.Profile
                    }),
                    Atteched = m.Message.Atteched.Select(x => new
                    {
                        x.Id,
                        x.FileName,
                        x.FilePath,
                        x.FileType,
                    })
                },
                Action = msgCodeToAction(m.LogAction),
                m.Message.CreateDateTime

            })
            .ToList();
        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }
    [HttpGet]
    public IActionResult GetOneMessageLogOneLine([FromQuery] PaginationFilter paginationFilter, int? UserId, int? MessageId , int? LogAction)
    {
        var query = db.msgLog_tbl.AsQueryable();

        if(LogAction.HasValue){//چک کردن بر اساس عملیات و اکشن
            query = query.Where(x=> x.LogAction == (int)LogAction);
        }

        query
        .Where(x => msgFilter(UserId , MessageId , x.UserId , x.MessageId))
        .Include(x => x.User)
        .Include(x => x.Message)
            .ThenInclude(x => x.SenderUser)
        .Include(x => x.Message)
            .ThenInclude(x => x.Recivers)
                .ThenInclude(x => x.Reciver)
        .Include(x => x.Message)
            .ThenInclude(x => x.Atteched);


        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / paginationFilter.PageSize);

        var pagedData = query
            .OrderByDescending(x => x.Id)
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize)
            .Select(m => $"{m.User.Username} {msgCodeToAction(m.LogAction)} [{m.Message.Id}] {m.Message.Subject} ({m.Message.BodyText.Substring(0, 26)}) {m.CreateDateTime}")
            .ToList();
        var pagedResponse = new PagedResponse<object>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize, totalPages, totalCount);

        return Ok(pagedResponse);
    }

    static private string msgCodeToAction(int code)
    {
        switch (code)
        { //1) read / 2) delete / 3) send / 4) Recive as to / 5) Recive As CC / 6)upload / 7) trash / 8) Untrash
            case 1:
                return "Read";
            case 2:
                return "Delete";
            case 3:
                return "Send";
            case 4:
                return "Recive (to)";
            case 5:
                return "Recive (CC)";
            case 6:
                return "Upload";
            case 7:
                return "Trash";
            case 8:
                return "Untrash";
            default:
                return "WTF ? how You Get THERE ???";

        }
    }
    static private string userCodeToAction(int code, bool done)
    {
        switch (code)
        { // 1) Login / 2) Logout / 3) Register / 4) ResetPassword / 5) Verify password / 6) Update
            case 1:
                return done ? "Login" : "Faild To Login ";
            case 2:
                return done ? "Logout" : "Faild To Logout ";
            case 3:
                return done ? "Register" : "Faild To Register ";
            case 4:
                return done ? "ResetPassword" : "Faild To ResetPassword ";
            case 5:
                return done ? "VerifyPassword" : "Faild To VerifyPassword ";
            case 6:
                return done ? "UpdateData" : "Faild To UpdateData ";
            default:
                return "WTF ? how You Get THERE ???";
        }
    }

}