public class messagefilter
{
    int userId;
    public void ApplyMessageFilters(ref IQueryable<Messages> query, MessageDetailsFilter filter)
    {
        // remove All Delete
        query = query.Where(x=> !x.Deleted.Contains(userId));
        
        // Check Trash
        if (filter.Trash.HasValue)
        {
            if (filter.Trash == true)
            {
                query = query.Where(m => m.Trashed.Contains(userId) && !m.Deleted.Contains(userId));
            }
            else
            {
                query = query.Where(m => !m.Trashed.Contains(userId) && !m.Deleted.Contains(userId));
            }
        }

        //check Other

        if (filter.Id.HasValue)
            query = query.Where(m => m.Id == filter.Id.Value);
        if (filter.CreateDateTime.HasValue)
            query = query.Where(m => m.CreateDateTime == filter.CreateDateTime.Value);
        if (!string.IsNullOrEmpty(filter.SerialNumber))
            query = query.Where(m => m.SerialNumber.Contains(filter.SerialNumber));
        if (filter.SenderUserId.HasValue)
            query = query.Where(m => m.SenderUserId == filter.SenderUserId.Value);
        if (!string.IsNullOrEmpty(filter.Subject))
            query = query.Where(m => m.Subject.Contains(filter.Subject));
        if (!string.IsNullOrEmpty(filter.BodyText))
            query = query.Where(m => m.BodyText.Contains(filter.BodyText));
        if (!string.IsNullOrEmpty(filter.Username_Sender))
            query = query.Where(m => m.SenderUser.Username.Contains(filter.Username_Sender));
        if (!string.IsNullOrEmpty(filter.FirstName_Sender))
            query = query.Where(m => m.SenderUser.FirstName.Contains(filter.FirstName_Sender));
        if (!string.IsNullOrEmpty(filter.LastName_Sender))
            query = query.Where(m => m.SenderUser.LastName.Contains(filter.LastName_Sender));
        if (!string.IsNullOrEmpty(filter.Phone_Sender))
            query = query.Where(m => m.SenderUser.Phone.Contains(filter.Phone_Sender));
        if (!string.IsNullOrEmpty(filter.Addres_Sender))
            query = query.Where(m => m.SenderUser.Addres.Contains(filter.Addres_Sender));
        if (!string.IsNullOrEmpty(filter.NatinalCode_Sender))
            query = query.Where(m => m.SenderUser.NatinalCode.Contains(filter.NatinalCode_Sender));
        if (!string.IsNullOrEmpty(filter.PerconalCode_Sender))
            query = query.Where(m => m.SenderUser.PerconalCode.Contains(filter.PerconalCode_Sender));
    }

    public void ApplyReciverFilters(ref IQueryable<Messages> query, ReciverDetailsFilter filter)
    {
        if (filter.ReciverId.HasValue)
            query = query.Where(x => x.Recivers.Any(r => r.ReciverId == filter.ReciverId.Value));
        if (filter.MessageId.HasValue)
            query = query.Where(x => x.Recivers.Any(r => r.MessageId == filter.MessageId.Value));
        if (!string.IsNullOrEmpty(filter.Type))
            query = query.Where(x => x.Recivers.Any(r => r.Type.Contains(filter.Type)));
        if (!string.IsNullOrEmpty(filter.Username_Rciver))
            query = query.Where(x => x.Recivers.Any(r => r.Reciver.Username.Contains(filter.Username_Rciver)));
        if (!string.IsNullOrEmpty(filter.FirstName_Reciver))
            query = query.Where(x => x.Recivers.Any(r => r.Reciver.FirstName.Contains(filter.FirstName_Reciver)));
        if (!string.IsNullOrEmpty(filter.LastName_Reciver))
            query = query.Where(x => x.Recivers.Any(r => r.Reciver.LastName.Contains(filter.LastName_Reciver)));
        if (!string.IsNullOrEmpty(filter.Phone_Reciver))
            query = query.Where(x => x.Recivers.Any(r => r.Reciver.Phone.Contains(filter.Phone_Reciver)));
        if (!string.IsNullOrEmpty(filter.Addres_Reciver))
            query = query.Where(x => x.Recivers.Any(r => r.Reciver.Addres.Contains(filter.Addres_Reciver)));
        if (!string.IsNullOrEmpty(filter.NatinalCode_Reciver))
            query = query.Where(x => x.Recivers.Any(r => r.Reciver.NatinalCode.Contains(filter.NatinalCode_Reciver)));
        if (!string.IsNullOrEmpty(filter.PerconalCode_Reciver))
            query = query.Where(x => x.Recivers.Any(r => r.Reciver.PerconalCode.Contains(filter.PerconalCode_Reciver)));
    }

    public void ApplyAttachFilters(ref IQueryable<Messages> query, AttachDetailsFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.FileName))
            query = query.Where(a => a.Atteched.Any(x => x.FileName.Contains(filter.FileName)));
        if (!string.IsNullOrEmpty(filter.FilePath))
            query = query.Where(a => a.Atteched.Any(x => x.FilePath.Contains(filter.FilePath)));
        if (!string.IsNullOrEmpty(filter.FileType))
            query = query.Where(a => a.Atteched.Any(x => x.FileType.Contains(filter.FileType)));


    }
    public messagefilter(int _userId)
    {
        userId = _userId;
    }

}