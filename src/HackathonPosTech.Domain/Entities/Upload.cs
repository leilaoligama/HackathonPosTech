namespace HackathonPosTech.Domain.Entities
{
    public class Upload
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string CaminhoUpload { get; set; }
        public string CaminhoZip { get; set; }
    }
}
