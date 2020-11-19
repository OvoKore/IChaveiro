using IChaveiro.Validators;

namespace IChaveiro.Models
{
    public class Locksmith
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string company_name { get; set; }
        public string cell_phone { get; set; }
        public string cnpj { get; set; }
#nullable enable
        public string? state_registration { get; set; }
#nullable disable

        public Locksmith()
        {
        }

        public Locksmith(LocksmithVal _locksmith)
        {
            cnpj = _locksmith.cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            email = _locksmith.email.ToLower();
            password = Utils.Crypt(_locksmith.password);
            company_name = _locksmith.company_name;
            cell_phone = _locksmith.cell_phone.Replace("(", "").Replace(")", "").Replace("-", "");
            state_registration = string.IsNullOrEmpty(_locksmith.state_registration) ? null : _locksmith.state_registration;
        }
    }
}