using System.ComponentModel.DataAnnotations.Schema;



    public class MessageDetailsFilter
    {
        //Message Model
        public bool? Trash { get; set; }
        public int? Id { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string? SerialNumber { get; set; }
        public int? SenderUserId { get; set; }
        public string? Subject { get; set; }
        public string? BodyText { get; set; }

        //User Model
        public string? Username_Sender { get; set; }
        // public string? Password_Sender { get; set; }
        // public string? Token_Sender { get; set; }
        public string? FirstName_Sender { get; set; }
        public string? LastName_Sender { get; set; }
        public string? Phone_Sender { get; set; }
        public string? Addres_Sender { get; set; }
        public string? NatinalCode_Sender { get; set; }
        public string? PerconalCode_Sender { get; set; }
    }

