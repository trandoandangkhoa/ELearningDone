namespace WebLearning.Contract.Dtos.Notification
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }

        public Guid TargetNotificationId { get; set; }

        public Guid FatherTargetId { get; set; }

        public string FatherAlias { get; set; }

        public Guid RoleId { get; set; }

        public string Alias { get; set; }

        public string TargetName { get; set; }

        public string TargetImagePath { get; set; }

        public DateTime DateCreated { get; set; }

        public string AccountName { get; set; }

        public bool Notify { get; set; }

        public string DescNotify { get; set; }

    }
}
