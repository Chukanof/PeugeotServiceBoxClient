using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Infrastructure.EntityBase;

namespace WpfClient.Utils.CredentialsManager
{
    [Table("Credentials")]
    public class CredentialEntry : IEntity<int>
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
