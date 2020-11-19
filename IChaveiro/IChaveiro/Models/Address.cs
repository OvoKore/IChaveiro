using IChaveiro.Validators;

namespace IChaveiro.Models
{
    public class Address
    {
        public int id { get; set; }
        public bool main { get; set; }
        public string name { get; set; }
        public string cep { get; set; }
        public string uf { get; set; }
        public string cidade { get; set; }
        public string bairro { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }

        public Address()
        {
        }

        public Address(AddressVal _addressValidator)
        {
            name = _addressValidator.name;
            cep = _addressValidator.cep;
            uf = _addressValidator.uf;
            cidade = _addressValidator.cidade;
            bairro = _addressValidator.bairro;
            logradouro = _addressValidator.logradouro;
            numero = _addressValidator.numero;
            complemento = _addressValidator.complemento;
        }
    }
}
