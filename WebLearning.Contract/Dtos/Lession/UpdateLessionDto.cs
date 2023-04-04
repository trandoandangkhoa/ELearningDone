namespace WebLearning.Contract.Dtos.Lession
{
    public class UpdateLessionDto
    {

        public string Name { get; set; }


        public string ShortDesc { get; set; }

        public bool Active { get; set; }
        //public IFormFile Image { get; set; }

        public DateTime DateCreated { get; set; }

        public string Alias { get; set; }


        public string Author { get; set; }
        public bool Notify { get; set; }

        public string DescNotify { get; set; }


    }
}
