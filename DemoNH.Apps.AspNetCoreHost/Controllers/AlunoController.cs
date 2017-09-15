using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoNH.Core.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoNH.Apps.AspNetCoreHost.Controllers
{
    [Route("api/[controller]")]
    public class AlunoController : Controller
    {
        private readonly IAlunoService alunoService;

        public AlunoController(IAlunoService alunoService)
        {
            this.alunoService = alunoService;
        }


        // GET: api/values
        [HttpGet]
        public void Get()
        {
            this.alunoService.Execute();
        }

    }
}
