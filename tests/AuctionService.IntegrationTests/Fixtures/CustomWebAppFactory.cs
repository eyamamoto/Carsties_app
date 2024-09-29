using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionService.IntegrationTests.Fixtures
{
    //cria uma instancia da nossa aplicação que permite reutilizar durante as outras execuções dos testes
    //high value tests
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {

    }
}
