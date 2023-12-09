using System.ComponentModel.DataAnnotations.Schema;

namespace AspektZadacaWebApi.Dtos
{
    public class ContactDto
    {
        public string Name { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
    }
}
