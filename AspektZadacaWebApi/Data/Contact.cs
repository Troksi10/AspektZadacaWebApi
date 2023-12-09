using System.ComponentModel.DataAnnotations.Schema;

namespace AspektZadacaWebApi.Data
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
    }
}
