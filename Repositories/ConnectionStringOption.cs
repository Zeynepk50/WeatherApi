using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class ConnectionStringOption //Program.cs de okuyacağız.
    {
        public const string Key = "ConnectionStrings";  //Program.cs'teki connectionStrings kısmındaki sabiti tanımlıyoruz.
        public string SqlServer { get; set; } = default!;

    }
}
