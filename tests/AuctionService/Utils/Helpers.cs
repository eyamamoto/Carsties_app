﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuctionService.UnitTest.Utils
{
    public class Helpers
    {
        public static ClaimsPrincipal GetClaimsPrincipal()
        {
            var claims = new List<Claim> { new Claim((ClaimTypes.Name), "test") };
            var identity = new ClaimsIdentity(claims, "testing");

            return new ClaimsPrincipal(identity);
        }
    }
}
