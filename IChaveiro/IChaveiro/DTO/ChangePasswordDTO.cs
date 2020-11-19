namespace IChaveiro.DTO
{
    public class ChangePasswordDTO
    {
        public string old_password { get; set; }
        public string new_password { get; set; }

        public ChangePasswordDTO()
        {
        }
    }
}
